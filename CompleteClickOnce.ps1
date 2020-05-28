$applicationName = "RCS.WpfShop"

$buildPath = "$env:SYSTEM_DEFAULTWORKINGDIRECTORY\$applicationName\bin\$env:BUILDCONFIGURATION"
Write-Host "buildPath = $buildPath"

$publishPath = "$buildPath\app.publish"

$exeName = "$applicationName.exe"
Remove-Item "$publishPath\$exeName"

# Debugging
#Write-Host "Start of $publishPath  ======================="
#Get-ChildItem -Path $publishPath -Recurse

$modulesPath = "$buildPath\Modules"
Write-Host "modulesPath = $modulesPath"

$applicationPath = Resolve-Path "$publishPath\Application Files\*" |  Select-Object -Last 1
Write-Host "applicationPath = $applicationPath"

# Resulting spaces should not be an issues when expanded as parameters.
Copy-Item -Path $modulesPath -Destination $applicationPath -Recurse

# This is apparently not possible in the command line version of Mage, but is in the GUI.
# Better leave out the .deploy extension everywhere, because Mage changes the identity to it too on Update (BUG). It is not needed anyway.
#Get-ChildItem $applicationPath\Modules\* | Rename-Item -NewName { $_.Name + ".deploy" }

# Used a relative path here, also tried just the filename.
#$iconFile = "Images\Main.ico"
#Write-Host "iconFile = $iconFile"

$deploymentManifest = "$publishPath\$applicationName.application"
Write-Host "deploymentManifest = $deploymentManifest"

$deploymentManifestCopy = "$applicationPath\$applicationName.application"
Write-Host "deploymentManifestCopy = $deploymentManifestCopy"

# Appears not to be there.
#Remove-Item $deploymentManifestCopy

$applicationManifest = "$applicationPath\$exeName.manifest"
Write-Host "applicationManifest = $applicationManifest"

$certFile = $env:PFXFILE_SECUREFILEPATH
Write-Host "certFile = $certFile"

$env:Path += ";C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools"

# Note the IconFile option apparently is only available on command line and needed for Programs & Features.
# Note the option is only allowed on the applicationManifest.
# Note the iconFile option turned out to be already present in the applicationManifest, apparently taken from the application.
# So far the IconFile option did not work and is deactivated.
# -IconFile $iconFile
# Note locked variables are not accessible.
mage -update $applicationManifest -FromDirectory $applicationPath -CertFile $certFile -Password "$env:PFXPASSWORD"
mage -update $deploymentManifest -appmanifest $applicationManifest -CertFile $certFile -Password "$env:PFXPASSWORD"

Copy-Item -Path $deploymentManifest -Destination $deploymentManifestCopy

#Write-Host "Modules added and manifests updated to $publishPath  ======================="
#Get-ChildItem -Path $publishPath -Recurse

Compress-Archive -Path $publishPath\* -DestinationPath "$publishPath\CyclOne.ClickOnce.zip"

Write-Host "Archived into $publishPath ======================="
Get-ChildItem -Path $publishPath 
