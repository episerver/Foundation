## NOTE: this branch is deprecated -- use the [main branch](https://github.com/episerver/Foundation/tree/main) for CMS 12 / .NET 5.

---

## Foundation 

Foundation offers a starting point that is intuitive, well-structured and modular allowing developers to explore CMS, Commerce, Personalization, Search amd Navigaion, Data Platform and Experimentation.

---

## Prerequisites

You will need these to run locally on your machine.

[Net 5](https://dotnet.microsoft.com/download/dotnet/5.0) sdk is required to use with visual studio.  Runtime maybe sufficent to just run the application.
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
git clone git@github.com:episerver/Foundation.git
cd foundation
git checkout main
setup.cmd 
dotnet run
```

### Mac

```
Open a Terminal window
git clone git@github.com:episerver/Foundation.git
cd foundation
git checkout main
./setup.sh
dotnet run
```

### Linux

```
Open a bash terminal window
git clone git@github.com:episerver/Foundation.git
cd foundation
git checkout main
./setup.sh
dotnet run
```

---