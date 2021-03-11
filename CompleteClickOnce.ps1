$applicationName = "RCS.WpfShop"

$buildPath = "$env:SYSTEM_DEFAULTWORKINGDIRECTORY\$applicationName\bin\$env:BUILDCONFIGURATION\$env:BUILDTARGETFRAMEWORK"
Write-Host "buildPath = $buildPath"

$publishPath = "$env:SYSTEM_DEFAULTWORKINGDIRECTORY\..\publish\ClickOnce"

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
Copy-Item -Path $modulesPath -Destination $applicationPath -Recurse -Force

$deploymentManifest = "$publishPath\$applicationName.application"
Write-Host "deploymentManifest = $deploymentManifest"

$applicationManifest = "$applicationPath\$applicationName.dll.manifest"
Write-Host "applicationManifest = $applicationManifest"

$certFile = $env:PFXFILE_SECUREFILEPATH
Write-Host "certFile = $certFile"

# Used for Mage.
$env:Path += ";C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools"

#mage -update $applicationManifest -FromDirectory $applicationPath -CertFile $certFile -Password "$env:PFXPASSWORD"

# Create entirely new manifest, because of problems by the update option.
# Those may have been caused by using the deploy extension. Resulting in double listings or conflicts in identity.
# Leave out the deploy extension (in the profile) while publishing.
# UseManifestForTrust false only uses the deploymentManifest for names, description, etcetera. It is not needed in the applicationManifest and would have to be specified again.
Remove-Item $applicationManifest
mage -New Application -UseManifestForTrust false -ToFile $applicationManifest -FromDirectory $applicationPath -CertFile $certFile -Password "$env:PFXPASSWORD"

# Update the hash.
mage -update $deploymentManifest -appmanifest $applicationManifest -CertFile $certFile -Password "$env:PFXPASSWORD"

#Write-Host "Modules added and manifests updated to $publishPath  ======================="
#Get-ChildItem -Path $publishPath -Recurse

Set-Location -Path $publishPath

$zipFile = "$publishPath\CyclOne.ClickOnce.zip"

# Clean.
if (Test-Path $zipFile) {Remove-Item $zipFile}

# Got to get rid of the other redundantly generated files.
Compress-Archive -Path "Application Files",  Launcher.exe, $deploymentManifest, setup.exe -DestinationPath $zipFile

Write-Host "Archived into $publishPath ======================="
Get-ChildItem -Path $publishPath 
