# Xpo SyncFramework Docs
Documentation for SyncFramework. A set of XPO providers for database synchronization created by BitFrameworks and Xari

Xpo SyncFramework is a commercial product, you can get pricing information by contacting us at info@xari.io

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
dotnet new -i BitXari.XafSyncTemplate.21.1.13.nupkg
```
#### --or--

Install the template from a nuget feed
```<language>
dotnet new -i BitXari.XafSyncTemplate::21.1.13
```

#### --or--

Install the template from a nuget feed
```<language>
dotnet new -i BitXari.XafSyncTemplate::21.1.13 --nuget-source https://nuget.bitframeworks.com/nuget/public/v3/index.json
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
dotnet new XafXamarinOfflineSync --ios --android --name YourProjectName
```

### How to connect your emulators to the Sync Server running in your computer

There are many ways to connect your emulators to the sync server running in your computer, our preferred option is using Ngrok
but you can also follow the official Microsoft documentation, all links below

**Connect to local web services**
https://docs.microsoft.com/en-us/xamarin/cross-platform/deploy-test/connect-to-local-web-services

**Ngrok exposes local servers behind NATs and firewalls to the public internet over secure tunnels.**
https://ngrok.com/

Use ngrok quickly and easily from within Visual Studio. ngrok allows you to expose a local server behind a NAT or firewall to the internet. "Demo without deploying."
https://marketplace.visualstudio.com/items?itemName=DavidProthero.NgrokExtensions

### How to upgrade nuget versions for your project

When you create a solution using this template you will get a build props file as the one in the link below

[Build Props File](DemoApp/Directory.Build.props)


```<language>
<Project>
	<PropertyGroup>
		<!--DevExpress version-->
		<DevExpressVersion>21.1.3</DevExpressVersion>
		<!--SyncFramework Version-->
		<SyncFrameworkVersion>21.1.3.28</SyncFrameworkVersion>
		<!--demo or licensed-->
		<NugetReferences>demo</NugetReferences>
		<!--Additional references-->
		<BITXpo>21.1.3.5841</BITXpo>
		<BITAspNetCore>21.1.3.5841</BITAspNetCore>
		<BITDataTransferRestClientNet>21.1.3.5841</BITDataTransferRestClientNet>
	</PropertyGroup>
</Project>

```
Using this file you can easily change the versions of your nuget packages

- **DevExpressVersion**: set the version of devexpress references
- **SyncFrameworkVersion**: set the version of the SyncFramework
- **NugetReferences**: use demo to reference the trial version of the SyncFramework otherwise use licensed if you own the full version of the framework
- **BITXpo**: Infrastructure needed for network communication https://github.com/egarim/BitFrameWorks/tree/master/src/Xpo/BIT.Data.Xpo
- **BITAspNetCore**: Infrastructure needed for network communication https://github.com/egarim/BitFrameWorks/tree/master/src/Xpo/BIT.AspNetCore.Xpo
- **BITDataTransferRestClientNet**:  needed for network communication https://github.com/egarim/BitFrameWorks/tree/master/src/Core/BIT.Data.Functions.RestClientNet

### Getting started: Create your first offline app with synchronization

If you are new using our SyncFramework solution follow this tutorial to create your first app

[Getting Started](Docs/GettingStarted.md) 

### General recommendations

**Databases**
- Don't share delta databases between applications

**Android**
- Make sure that you provide an unique name for your application package in [AndroidManifest](/DemoApp/DemoApp.Mobile.Android/Properties/AndroidManifest.xml) Otherwise you might end up using an old database for your application and you will get exceptions when processing the deltas

### Tutorials

| Source  | Video |
| ------------- | ------------- |
| Content Cell  | Content Cell  |
| Content Cell  | Content Cell  |