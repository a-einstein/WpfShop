## WpfShop

Submitted to code analysis by **[Better Code Hub](https://bettercodehub.com)**.  
Current score: [![BCH compliance](https://bettercodehub.com/edge/badge/a-einstein/WpfShop)](https://bettercodehub.com)  

#### Purpose
* Creating a working desktop application for test and demo purpose based on WPF.
* Manage the code by Git and GitHub. Some changes, branches, and merges are deliberately created for this reason.

#### Notes
* This is a near equivalent of my [PortableShop](https://github.com/a-einstein/PortableShop).
* For the time being, the needed data service is hosted on Azure and configured to in this application. That means that this code can be build and run right away, if it can make use of an internet connection.

#### Aspects
* C# + XAML.
* Prism.
* MEF.
* Regions.
* MVVM.
* Make use of an Azure service.
* Client-server.
* WCF + SSL.
* Asynchronisity.
* Globalized resources.
* Basic styling.
* Unit testing.

#### Prerequisites
* The application assumes the presence of my [AdventureWorks services](https://github.com/a-einstein/AdventureWorks/blob/master/README.md), to which a service connection should be configured.
