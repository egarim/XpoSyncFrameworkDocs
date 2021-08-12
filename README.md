# Xpo SyncFramework Docs
Documentation for SyncFramework. A set of XPO providers for  database synchronization created by BitFrameworks and Xari




#### Requirements

The official nuget feed from developer Express is required since all references comes from the nuget feed, more information here.

https://docs.devexpress.com/GeneralInformation/116042/installation/install-devexpress-controls-using-nuget-packages/obtain-your-nuget-feed-url

The official nuget feed from BitFrameworks is required. Use the following command line to add the repository
```<language>
dotnet nuget add source https://nuget.bitframeworks.com/nuget/public/v3/index.json -n bitframeworks
```

Here you can find more information on how to add a nuget package source.
https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-nuget-add-source


### How to install the template

Install the template from a nuget file
```<language>
dotnet new -i BitXari.XafSyncTemplate.21.1.11.nupkg
```
#### --or--

Install the template from a nuget feed
```<language>
dotnet new -i BitXari.XafSyncTemplate::21.1.11
```

#### --or--

Install the template from a nuget feed
```<language>
dotnet new -i BitXari.XafSyncTemplate::21.1.11 --nuget-source https://nuget.bitframeworks.com/nuget/public/v3/index.json
```

### How to uninstall the template


```<language>
dotnet new -u BitXari.XafSyncTemplate
```

More information about uninstalling templates here https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-new-uninstall

### How to see template help information

```<language>
dotnet new XafXamarinOfflineSync --help
```

### How to create a new project
To create a new Xaf App (windows forms net 5 and blazor net 5) application with a SyncServer

```<language>
dotnet new XafXamarinOfflineSync --name YourProjectName
```
To create a new Xaf App (windows forms net 5 and blazor net 5) application with a SyncServer and Android client

```<language>
dotnet new XafXamarinOfflineSync --name YourProjectName --android
```

To create a new Xaf App (windows forms net 5 and blazor net 5) application with a SyncServer and iOs client

```<language>
dotnet new XafXamarinOfflineSync --name YourProjectName --ios
```

To create a new Xaf App (windows forms net 5 and blazor net 5) application with a SyncServer and iOs and Android clients

```<language>
dotnet new XafXamarinOfflineSync --name YourProjectName --xamarinall
```

### Current issues

There is a problem with the template when you do not include all the possible projects. For example, if you create a Xaf only application using the following line

```<language>
dotnet new XafXamarinOfflineSync --name YourProjectName
```
The solution will show that the xamarin projects have not been loaded and it's an error because the xamarin projects are not actually there.
So you need to remove these projects manually
