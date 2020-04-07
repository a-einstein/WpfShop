## WpfShop

#### Description
Working desktop application for test and demo purpose based on WPF. It simulates limited shopping capabilities based on the AdventureWorks database.

#### News
* The ClickOnce deliverable is plug and play again. See the installation notes.
* Makes use of own certified domain for webservice.
* Integrated with Azure Devops build and release pipelines.
* Mocked the data service for testing, applying Moq.
* Added integrated GUI test by means of WinAppDriver/Selenium/Appium.

#### Purpose
* Explore various techniques based on C# and WPF.
* Manage the code by Git and GitHub.
* Explore continuous integration by using combinations of Git, GitHub, TeamCity and Azure DevOps.
* Explore Scrum process management by integration with Jira and Azure DevOps.

#### Prerequisites
* The application must be configured for a running instance of my [AdventureWorks services](https://github.com/a-einstein/AdventureWorks/blob/master/README.md).

#### Notes
* This is a near equivalent of my [PortableShop](https://github.com/a-einstein/PortableShop).
* Submitted to code analysis by [Better Code Hub](https://bettercodehub.com). Current score: [![BCH compliance](https://bettercodehub.com/edge/badge/a-einstein/WpfShop)](https://bettercodehub.com)  
* Connected to automated Azure Devops build and release pipelines. Current build status for the master branch: [![Build Status](https://dev.azure.com/RcsProjects/WpfShop/_apis/build/status/Build?branchName=master)](https://dev.azure.com/RcsProjects/WpfShop/_build/latest?definitionId=12&branchName=master)

#### Aspects
* WPF.
* C# + XAML.
* MVVM.
* Prism/Unity Modules & Regions
* Client-server.
* Azure service.
* WCF + SSL + domain + certificate.
* asynchronicity.
* Globalized resources.
* User controls.
* Attached behaviours.
* Basic styling.
* Basic exploration of unit testing by means of MS Unit Test Framework.
* Serious start on integrated GUI testing by means of WinAppDriver/Selenium/Appium.
* Mocking data for testing, applying Moq.
* Transformation of configurations.
* Click Once installation and update.

#### Installation
The application is plug & play, but use of the data service is on request. Contact the developer ahead. 
* Download the latest Cyclone.zip under Assets at the **[releases page](https://github.com/a-einstein/WpfShop/releases).**
* Extract if needed.
* Launch setup.exe, allow it to continue if needed. 
* The application should start up right away, but may be hidden behind other windows.
* The start menu gets a folder added: *Programs / RCS / Shopping*.
* Uninstallation can be done by: *Control Panel / Program and Features / CyclOne*.
* Updates are currently not reported, but can be done manually after uninstalling.

