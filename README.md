## WpfShop

Submitted to code analysis by **[Better Code Hub](https://bettercodehub.com)**.  
Current score: [![BCH compliance](https://bettercodehub.com/edge/badge/a-einstein/WpfShop)](https://bettercodehub.com)  

#### News
This application is now plug & play.
* It can be installed directly on Windows from my releases, see Installation.
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
* Basic styling.
* Unit testing.
* Click Once installation.

#### Installation
* Download and extract the latest ZIP file in [releases](https://github.com/a-einstein/WpfShop/releases)
* Run *setup*. Note one may have to create exceptions in protection programs on the go.
* The application should start up right away, but may be hidden behind other windows.
* The start menu also gets a folder added: *Programs / RCS / Shopping*.
* Uninstallation is by: *Control Panel / Program and Features / CyclOne*.
