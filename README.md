# XpoSyncFrameworkDocs
Documentation for SyncFramework a set of XPO providers offline database synchronization created by BitFrameworks and Xari



-To install from template
dotnet new --install C:\Users\Joche\Desktop\SynFrameworkTemplate\LobApp

-to install from nuget
dotnet new -i BitXari.XafSyncTemplate.1.0.0.nupkg


-to create a new project
XafXamarinOfflineSync

dotnet new LobSync --name ErpDemo

-to uninstall
dotnet new -u 
then copy the uninstall line

-to create a new project from the template
dotnet new XafXamarinOfflineSync --name YourProjectName


#### Requirements

The official nuget feed from developer Express is required since all references comes from the nuget feed, more information here.
https://docs.devexpress.com/GeneralInformation/116042/installation/install-devexpress-controls-using-nuget-packages/obtain-your-nuget-feed-url

The official nuget feed from BitFrameworks is required.
you the use the following command line to add the repository
dotnet nuget add source https://nuget.bitframeworks.com/nuget/public/v3/index.json -n bitframeworks

Here you can find more information on how to add a nuget package source.
https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-nuget-add-source


#### How to install the template

Install the template from a nuget file
dotnet new -i BitXari.XafSyncTemplate.1.0.0.nupkg::1.0


help
dotnet new XafXamarinOfflineSync --help

#### --or--

Install the template from a nuget feed
dotnet new -i BitXari.XafSyncTemplate::1.0

#### --or--

Install the template from a nuget feed
dotnet new -i BitXari.XafSyncTemplate::21.1.11 --nuget-source https://nuget.bitframeworks.com/nuget/public/v3/index.json


#### How to create a new project
-To create a new project from the template

dotnet new XafXamarinOfflineSync --name YourProjectName

