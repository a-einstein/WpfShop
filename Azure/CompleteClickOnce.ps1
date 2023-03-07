$applicationName = "RCS.WpfShop"

$buildPath = "$env:SYSTEM_DEFAULTWORKINGDIRECTORY\$applicationName\bin\$env:BUILDCONFIGURATION\*"
Write-Host "buildPath = $buildPath"

# Note Azure build explicitly needs this, the publishing profile is not enough.
$publishPath = "$env:SYSTEM_DEFAULTWORKINGDIRECTORY\..\publish\ClickOnce"

# Debugging
#Write-Host "Start of $publishPath  ======================="
#Get-ChildItem -Path $publishPath -Recurse

$modulesPath = "$buildPath\Modules"
Write-Host "modulesPath = $modulesPath"

# Note the version is added to the path.
$applicationPath = Resolve-Path "$publishPath\Application Files\*" |  Select-Object -Last 1
Write-Host "applicationPath = $applicationPath"

# Use MapFileExtensions false in publishing profile to avoid .deploy extensions, which complicate copying.
# Strangely, those still occur from withing Visual Studio.
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

# Mage documentation
# https://docs.microsoft.com/en-us/dotnet/framework/tools/mage-exe-manifest-generation-and-editing-tool

# Create entirely new manifest, because of problems by the update option.
# - Updating the whole $applicationPath gives double entries.
# - Updating $modulesPath does take in the subdirectory.
# UseManifestForTrust false only uses the deploymentManifest for names, description, etcetera. It is not needed in the applicationManifest and would have to be specified again.
# Use Algorithm to minimalise differences in manifests.

mage -New Application -UseManifestForTrust false -ToFile $applicationManifest -FromDirectory $applicationPath -CertFile $certFile -Password "$env:PFXPASSWORD" -Algorithm "sha256RSA"

# Update the hash.
mage -update $deploymentManifest -appmanifest $applicationManifest -CertFile $certFile -Password "$env:PFXPASSWORD" -Algorithm "sha256RSA"

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
