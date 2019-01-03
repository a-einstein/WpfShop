## WpfShop

#### Description
Working desktop application for test and demo purpose based on WPF. It simulates limited shopping capabilities based on the AdventureWorks database.

#### News
* Added integrated GUI test by means of WinAppDriver/Selenium/Appium.
* The data service on Azure is no longer functional.

#### Purpose
* Explore various techniques based on C# and WPF.
* Manage the code by Git and GitHub. Some changes, branches, and merges are deliberately created for this reason.
* Explore continuous integration by using combinations of Git, GitHub, TeamCity and Visual Studio Team Services (VSTS).
* Explore Scrum process management by integration with Jira and Visual Studio Team Services.

#### Prerequisites
* The application must be configured for a running instance of my [AdventureWorks services](https://github.com/a-einstein/AdventureWorks/blob/master/README.md).

#### Notes
* This is a near equivalent of my [PortableShop](https://github.com/a-einstein/PortableShop).
* Submitted to code analysis by [Better Code Hub](https://bettercodehub.com). Current score: [![BCH compliance](https://bettercodehub.com/edge/badge/a-einstein/WpfShop)](https://bettercodehub.com)  

#### Aspects
* WPF.
* C# + XAML.
* MVVM.
* Prism/Unity Modules & Regions
* Client-server.
* Azure service.
* WCF + SSL.
* asynchronicity.
* Globalized resources.
* User controls.
* Attached behaviours.
* Basic styling.
* Basic exploration of unit testing by means of MS Unit Test Framework.
* Serious start on integrated GUI testing by means of WinAppDriver/Selenium/Appium.
* Click Once installation and update.

#### Installation
Currently one has to both compile this client as well as the data service, and create the database.

The application no longer is plug & play. Just for its own sake the application can still be directly installed. It is still configured to use my data service on Azure, so it will fail to work.
* Open the **[install page](https://rcsadventureworac85.blob.core.windows.net/wpfshop-releases/latest/install.htm).**
* If needed, install the prerequisites by the 'Install' button.
* Click the **'launch'** hyperlink, 'Open' to launch the installer, 'Install', allow it to continue when needed. 
* The application should start up right away, but may be hidden behind other windows.
* The start menu gets a folder added: *Programs / RCS / Shopping*.
* Uninstallation can be done by: *Control Panel / Program and Features / CyclOne*.
* Updates are reported at startup, and can be accepted or skipped. (Reporting is not very reliable.)
* Reverting to the previous version can also be done through the Control Panel.
