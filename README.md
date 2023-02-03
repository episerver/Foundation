<a href="https://github.com/episerver/Foundation"><img src="https://www.optimizely.com/globalassets/02.-global-images/navigation/optimizely_logo_navigation.svg" title="Foundation" alt="Foundation"></a>

# Latest version
  This version is built for net6.0.  For the legacy full framework version please use the [full-framework/master](https://github.com/episerver/Foundation/tree/full-framework/master)

## Foundation 

Foundation offers a starting point that is intuitive, well-structured and modular allowing developers to explore CMS, Commerce, Personalization, Search amd Navigaion, Data Platform and Experimentation.

---

## Prerequisites

You will need these to run locally on your machine.

[Net 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) sdk is required to use with visual studio.  Runtime maybe sufficent to just run the application.

[Node JS](https://nodejs.org/en/download/)

Mac/Linux

[Docker](https://docs.docker.com/desktop/mac/install/)

Windows

[Sql Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

---

## The Solution

`Foundation has a default username and password of admin@example.com / Episerver123!`

---

## Installation

### Windows

```
open command prompt as administrator
git clone https://github.com/episerver/Foundation.git
cd foundation
git checkout main
setup.cmd 
dotnet run --project .\src\Foundation\Foundation.csproj
```

### Mac

```
Open a Terminal window
git clone https://github.com/episerver/Foundation.git
cd Foundation
git checkout main
chmod u+x setup.sh
./setup.sh
dotnet run --project ./src/Foundation/Foundation.csproj
```

### Linux

```
Open a bash terminal window
git clone https://github.com/episerver/Foundation.git
cd Foundation
git checkout main
chmod u+x setup.sh
./setup.sh
dotnet run --project ./src/Foundation/Foundation.csproj
```

### View the site

After completing the setup steps and running the solution, access the site at <a href="http://localhost:5000">http://localhost:5000</a>.

To change the default port, modify the file <a href="https://github.com/episerver/Foundation/blob/main/src/Foundation/Properties/launchSettings.json">/src/Foundation/Properties/launchSettings.json</a>.

---
