Mosey
===========
[![License](http://img.shields.io/:license-apache-blue.svg?style=flat-square)](http://www.apache.org/licenses/LICENSE-2.0.html)

This repository is the Mosey demo site.

# Prerequisites
-------------

1. Visual Studio 2017 or higher - [VS Downloads](https://visualstudio.microsoft.com/downloads/)
2. Sql Server Express or Developer or Sql Azure Server - [SQL Downloads](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (If using Sql Azure [download sqlcmd](https://docs.microsoft.com/en-us/sql/tools/sqlcmd-utility?view=sql-server-2017))
3. Nodejs - [Download](https://nodejs.org/en/download/)
3. IIS Role - [Instructions](https://help.k2.com/onlinehelp/k2blackpearl/icg/4.7/default.htm#Con_Role_Services_IIS.htm)

# Installation
------------

Run Build.bat and supply requested parameters. The build process will log to the following files.

```
BuildLogs\Build.log
BuildLogs\Database.log
BuildLogs\IIS.log
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

Login with admin@example/store account once it's finished.