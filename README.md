<a href="https://github.com/episerver/Foundation"><img src="http://ux.episerver.com/images/logo.png" title="Foundation" alt="Foundation"></a>

## Foundation

Foundation offers a starting point that is intuitive, well-structured and modular allowing developers to select Episerver products as projects to include or exclude from their solution. 
Including as of now projects for CMS, Commerce, Personalization, Find and Social, with the rest to follow.

For documentation on Episerver Foundation, see [the documentation repository](https://github.com/episerver/Foundation-docs). Note that this is till work-in-progress.

You can request a demo of the project by one of our Episerver experts on [Get a demo](https://www.episerver.com/get-a-demo/).

[![Build status](https://dev.azure.com/episerver-foundation/Foundation/_apis/build/status/Foundation-Release)](https://dev.azure.com/episerver-foundation/Foundation/_build/latest?definitionId=1)
[![License](http://img.shields.io/:license-apache-blue.svg?style=flat-square)](http://www.apache.org/licenses/LICENSE-2.0.html)

---

## Table of Contents

- [System requirements](#system-requirements)
- [Pre-installation set-up](#pre-installation-set-up)
- [Installation](#installation)
- [Troubleshooting](#troubleshooting)
- [Modular set-up](#modular-set-up)
- [Contributing](#contributing)

---

## System requirements

* Visual Studio 2017 or higher - [Download](https://visualstudio.microsoft.com/downloads/)
* SQL Server Express or Developer or SQL Azure Server - [Download](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (If using SQL Azure [download sqlcmd](https://docs.microsoft.com/en-us/sql/tools/sqlcmd-utility?view=sql-server-2017))
* Microsoft SQL Server Management Studio - [Download](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15)
* Nodejs - [Download](https://nodejs.org/en/download/)
* Microsoft Internet Information Server (IIS) - [Download](https://www.iis.net/downloads)

See also the general [Episerver system requirements](https://world.episerver.com/documentation/system-requirements/) on Episerver World.

---

## Pre-installation set-up

### SQL server

1. In Microsoft SQL Server Management Studio, connect to your SQL server:
![SQL server login](https://i.ibb.co/dW5n5wQ/SQLServer-Log-In.png)
2. Right-click on your server and select Properties.
3. Under **Security**, make sure that **SQL Server and Windows Authentication mode** is selected:
![SQL server authentication](https://i.ibb.co/2Sktyrb/SQLServer-Authentication.png")

### IIS settings

How to find the IIS settings depends on the system where you are running IIS.

1.	Go to your IIS settings. If you are running IIS locally on your Windows machine, you find these under **Control Panel** > **Programs** > **Programs and Features** > **Turn Windows features on or off**. 
2.	Check that the following features have been enabled:
  *	Under Application Development:
    *	ASP .NET
    * NET Extensibility
    * ASP
    * ISAPI Extensions
    *	ISAPI Filters
  *	Common HTTP Features (Installed) node (all are required):
    *	Static Content (Installed)
    *	Default Document (Installed)
    *	Directory Browsing (Installed
    *	HTTP Errors (Installed) (Installed)
    *	HTTP Redirection
  *	Under the Performance (Installed) node:
    *	Static Content Compression (Installed)
    *	Dynamic Content Compression (Installed)
  *	Under the Security (Installed) node:
    *	URL Authorization (Installed)

![IIS settings](https://i.ibb.co/cNTmzc2/ISSSettings.png)

---

## Installation

The installation files on GitHub contain a batch file that will install the Foundation project with all products and set up an empty demo site. After the installation, you can fetch demo content from a remote repository to create a Mosey demo site, a fictitious fashion retail company.

1.	Download the ZIP file from the Foundation project's **master** branch on GitHub and extract the files, or clone the project from GitHub to a local folder using the command prompt and the git command ```git clone https://github.com/episerver/Foundation foundation  ``` (the _foundation_ part specifies the folder where to put the files):

Download ZIP file

![Download Zip file](https://i.ibb.co/SB38p3z/Git-Hub-Zip.png)

Or clone project using Git

![Clone project](https://i.ibb.co/23tJmNm/Git-Cloning.png)

> **_Note:_** It is recommended that you store the project in a folder directly under C:, in a folder where your user has full access rights:

![Folder access rights](https://i.ibb.co/Wkcbr9m/Folder-Access-Rights.png)

2.	Right-click on the batch file called **setup.cmd** and select **Run as administrator**:

![Run batch file](https://i.ibb.co/SBFfLzt/Run-Batch-File.png)

3.	The installation starts and you are asked to provide the following parameters:

| Parameter | Description |
|-----------|-------------|
|Application name: | The name of the application. Note: The application name should contain only letters and numbers as it used as the prefix to create the website and database components.|
|Public domain name for foundation:| Domain name for the application, for example, foundation.com.|
|Public domain name for Commerce Manager: | Domain name for the Commerce Manager application, for example, commerce.foundation.com.|
|License path:| If you have a license file, add the path to it. Otherwise you can add that later.|
|SQL server name:| SQL server instance name. Add the same server name as the one you connected to in the [Pre-installation set-up](#pre-installation-set-up) steps for the SQL server. If using Azure SQL, provide the full dns name for your Azure SQL instance |
|sqlcmd command: | SQL command to execute, by default ```-S . -E ```. This can generally be left as is. If using Azure SQL, pass username and password as ```-U <user> -P <password>```|

![Build parameters](https://i.ibb.co/WcKGLVh/Build-Parameters.png)

4.	The build process executes a number of steps and logs both to the console and to the log files. The automatic build steps are:
```
•	Set permissions on the folder to everyone full control
•	Restore NuGet packages
•	npm install
•	gulp Saas task
•	Build solution
•	Install Databases
•	Create two application pools
•	Create two websites
•	Update host file
•	Copy License file
•	Update commerce manager url for access from cms
•	create connectionstrings files
•	Start the site to finish setup in browser
```
![Build progress](https://i.ibb.co/GvZBcYY/Build-Progress.png)

5.	When the installation is finished, a setup page is opened in your browser. If not, enter the URL http://_yourdomainname_/setup manually.
6.	If the setup page throws an error, open your host file, found under **C:\Windows\System32\drivers\etc**, and add the two domain names you entered during the installation. Reload the page in your browser.
![Example host file](https://i.ibb.co/Ss79b55/Host-File-Example.png)

7.	In the setup page under Import Content, select **Remote Site File: Mosey** and **Remote Catalog File: Foundation_Fashion** to import the Mosey demo site content.
![Demo content import](https://i.ibb.co/WG6bVcx/Demo-Content-Import.png)

8.	Click **Submit** and the Mosey demo site is displayed.
![Mosey start page](https://i.ibb.co/F5BHtb3/Mosey-Start-Page.png)

9.	Log in with user: **admin@example.com** and password: **store** to access the Episerver user interface.  

> **_Note:_** A **resetup.cmd** file has been created in your project which you can run to re-install the database.

10. Developer licenses for evaluation purposes can be obtained from the [Episerver License Center](https://license.episerver.com/). Place the **License.config** file in your webroot. 

## Troubleshooting
### The installation fails
* Check that you have full access rights to the project folder.
* Check that you meet [the system requirements](#system-requirements).
* Check your SQL authentication settings as described in [SQL Server](#sql-server).
* Check your IIS settings so that they match those specified in [IIS settings](#iis-settings).
* Check the log files:
  ```
  Build\Logs\Build.log
  Build\Logs\Database.log
  Build\Logs\IIS.log
  ```
### The site does not start
* Foundation does not include any website, pages or catalogs in its initial state, so it is not possible to start a site until you have imported or created content. Go to URL http://_yourdomainname_/setup to import content.
* Check that the site is actually running by navigating to http://_yourdomainname_/episerver/cms.

## Modular set-up

The Foundation project is set up to include all Episerver’s main products. Each product is set up as a project of its own inside the main project, so if you don’t want all products, you can simply remove their projects.

## Contributing
As this is an open-source project, we encourage you to contribute to the source code and the documentation. See the [Contribution guidelines for this project](docs/CONTRIBUTING.md).

---
