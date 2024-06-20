#### Visual Studio publish profiles (.pubxml files)
https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/visual-studio-publish-profiles

#### Command line (Azure)
https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-publish 


Explicit settings 'not honoured outside Visual Studio'.
Keep  synchronised.
- TargetFramework

		/p:TargetFramework="<TargetFramework>"

- RuntimeIdentifier

    	/p:RuntimeIdentifier="<RuntimeIdentifier>"

Not present at command line.

- Replaced by \<ProductName>, keep synchronised.

    	/p:PublisherName="<PublisherName>" - 


#### In all cases Mage has to be used to add the Modules to the manifest
https://learn.microsoft.com/en-us/dotnet/framework/tools/mage-exe-manifest-generation-and-editing-tool