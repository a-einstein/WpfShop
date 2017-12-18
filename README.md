## WpfShop

Submitted to code analysis by **[Better Code Hub](https://bettercodehub.com)**.  
Current score: [![BCH compliance](https://bettercodehub.com/edge/badge/a-einstein/WpfShop)](https://bettercodehub.com)  

#### News
This application is now plug & play.
* It can be installed directly on Windows from Azure, see Installation.
* It uses my data service permanently running on Azure.

#### Purpose
* Creating a working desktop application for test and demo purpose based on WPF.
* Manage the code by Git and GitHub. Some changes, branches, and merges are deliberately created for this reason.

#### Prerequisites
* The application must be configured for a running instance of my [AdventureWorks services](https://github.com/a-einstein/AdventureWorks/blob/master/README.md).

#### Notes
* This is a near equivalent of my [PortableShop](https://github.com/a-einstein/PortableShop).

#### Aspects
* C# + XAML.
* MVVM.
* Prism/MEF Modules & Regions
* Client-server.
* Azure service.
* WCF + SSL.
* asynchronicity.
* Globalized resources.
* User controls.
* Attached behaviours.
* Basic styling.
* Unit testing.
* Click Once installation and update.

#### Installation
* Open the [install page](https://rcsadventureworac85.blob.core.windows.net/wpfshop-releases/latest/install.htm).
* If needed, install the prerequisites by the 'Install' button.
* Click the 'launch' **hyperlink**, 'Open' to launch the installer, 'Install', allow it to continue when needed. 
* The application should start up right away, but may be hidden behind other windows.
* The start menu gets a folder added: *Programs / RCS / Shopping*.
* Uninstallation can be done by: *Control Panel / Program and Features / CyclOne*.
* Updates are reported at startup, and can be accepted or skipped. (Reporting is not very reliable.)
* Reverting to the previous version can also be done through the Control Panel.
