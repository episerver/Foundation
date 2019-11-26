<a href="https://github.com/episerver/Foundation"><img src="http://ux.episerver.com/images/logo.png" title="Foundation" alt="Foundation"></a>

## Foundation

Foundation offers a starting point that is intuitive, well-structured and modular allowing developers to select Episerver products as projects to include or exclude from their solution. 
Including as of now projects for CMS, Commerce, Personalisation, Find and Social, with the rest to follow.

[![Build status](https://dev.azure.com/episerver-foundation/Foundation/_apis/build/status/Foundation-Release)](https://dev.azure.com/episerver-foundation/Foundation/_build/latest?definitionId=1)
[![License](http://img.shields.io/:license-apache-blue.svg?style=flat-square)](http://www.apache.org/licenses/LICENSE-2.0.html)

---

## Table of Contents

- [Foundation](#foundation)
- [Table of Contents](#table-of-contents)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Building](#building)
- [Contributing](#contributing)

---

## Prerequisites

1. Visual Studio 2017 or higher - [VS Downloads](https://visualstudio.microsoft.com/downloads/)
2. Sql Server Express or Developer or Sql Azure Server - [SQL Downloads](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (If using Sql Azure [download sqlcmd](https://docs.microsoft.com/en-us/sql/tools/sqlcmd-utility?view=sql-server-2017))
3. Nodejs - [Download](https://nodejs.org/en/download/)
4. IIS Role - [Instructions](https://help.k2.com/onlinehelp/k2blackpearl/icg/4.7/default.htm#Con_Role_Services_IIS.htm)

---

## Installation

Run setup.cmd and supply requested parameters. The build process will log to the console and the following files.
Note that application name should contain only letters and numbers as it used as the prefix to create the website and database components.
```
Build\Logs\Build.log
Build\Logs\Database.log
Build\Logs\IIS.log
```

The build process will do the following for you

```
Set permissions on the folder to everyone full control
Restore nuget packages
npm install
gulp Saas task
Build solution
Install Databases
Create two application pools
Create two websites
Update host file
Copy License file
Update commerce manager url for access from cms
create connectionstrings files
Start the site to finish setup in browser
```

Login with admin@example.com/store account once it's finished.  
Please note a resetup.cmd file will be created which you can run to easily re-install the database.

---

## Building 

Run build.cmd To build the solution and run the default gulp task to build the client resources.

## Contributing
[Contribution guidelines for this project](docs/CONTRIBUTING.md)

---
