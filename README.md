## WpfShop

#### Description
Working desktop application for test and demo purpose based on WPF. It simulates limited shopping capabilities based on the AdventureWorks database. It can be installed plug & play (see **Installation**).

#### News
This project is now integrated with Visual Studio Team Services (VSTS) too.

#### Purpose
* Explore various techniques based on C# and WPF.
* Manage the code by Git and GitHub. Some changes, branches, and merges are deliberately created for this reason.
* Explore continuous integration by using buildmanager TeamCity.
* Explore Scrum process management by integration with Jira or Visual Studio Team Services (VSTS).

#### Prerequisites
* The application must be configured for a running instance of my [AdventureWorks services](https://github.com/a-einstein/AdventureWorks/blob/master/README.md).
* Currently this is met by configuration to my service on Azure.

#### Notes
* This is a near equivalent of my [PortableShop](https://github.com/a-einstein/PortableShop).
* Submitted to code analysis by [Better Code Hub](https://bettercodehub.com). Current score: [![BCH compliance](https://bettercodehub.com/edge/badge/a-einstein/WpfShop)](https://bettercodehub.com)  

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
* MS Unit Test Framework.
* Click Once installation and update.

#### Installation
The application currently is plug & play. The application can be installed from Azure, while it already configured to use my data service also running there.
* Open the **[install page](https://rcsadventureworac85.blob.core.windows.net/wpfshop-releases/latest/install.htm).**
* If needed, install the prerequisites by the 'Install' button.
* Click the 'launch' **hyperlink**, 'Open' to launch the installer, 'Install', allow it to continue when needed. 
* The application should start up right away, but may be hidden behind other windows.
* The start menu gets a folder added: *Programs / RCS / Shopping*.
* Uninstallation can be done by: *Control Panel / Program and Features / CyclOne*.
* Updates are reported at startup, and can be accepted or skipped. (Reporting is not very reliable.)
* Reverting to the previous version can also be done through the Control Panel.
