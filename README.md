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
dotnet new -i BitXari.XafSyncTemplate.21.1.3.29.nupkg
```
#### --or--

Install the template from a nuget feed
```<language>
dotnet new -i BitXari.XafSyncTemplate::21.1.3.29
```

#### --or--

Install the template from a nuget feed
```<language>
dotnet new -i BitXari.XafSyncTemplate::21.1.3.29 --nuget-source https://nuget.bitframeworks.com/nuget/public/v3/index.json
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
		<SyncFrameworkVersion>21.1.3.29</SyncFrameworkVersion>
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

**Binary data**
- Avoid storing binary data on your database if it's possible, binary data (like FileData from XAF) may use a lot of space in the database, which might result in larger delta transactions. Large deltas take a longer time to synchronize and can drain your mobile device battery and data plan

### Limitations

- **Auto generated integer primery keys**:Avoid using XPObjects that uses an Integer key, integer auto-increment keys depends on the database engine internal infrastructure and they are not possible to replicate. It?s out of the scope of the SyncFramework

https://docs.devexpress.com/XPO/3311/create-a-data-model/xpo-classes-comparison
### Theory
|   | Video |
| ------------- | ------------- |
| [SyncFramwork: Synchronization Theory](Docs/XpoOfflineDataSync.pptx)  | [![SyncFramwork: Synchronization Theory](https://img.youtube.com/vi/XVzyXJ43asQ/0.jpg)](https://www.youtube.com/watch?v=XVzyXJ43asQ)  |


### Tutorials

| Source  | Video |
| ------------- | ------------- |
| [01 Getting Started:Create a XAF Application and Access XAF Data in Xamarin Forms](Tutorials/01-GettingStarted/README.md)  | [![01 Getting Started](https://img.youtube.com/vi/_Af-iDnKVSU/0.jpg)](https://www.youtube.com/watch?v=_Af-iDnKVSU)  |
| [02 Adding a list and form page to a Xamarin Forms App and access XAF domain objects](Tutorials/02-Adding-a-list-and-form-page-for-a-new-DomainObject/README.md)  | [![02 Adding a list and form page for a new domain object](https://img.youtube.com/vi/OollP2p5eyM/0.jpg)](https://www.youtube.com/watch?v=OollP2p5eyM)  |
| [03 Using XAF validations in a Xamarin Forms App](Tutorials/03-Using-XAF-Validations-on-Xamarin/README.md)  | [![03 Using XAF Validations on Xamarin](https://img.youtube.com/vi/6XE3lC0qzLU/0.jpg)](https://www.youtube.com/watch?v=6XE3lC0qzLU)  |
| [04 Using XAF security system in a Xamarin Forms App](Tutorials/04-Using-XAF-SecuritySystem-in-Xamarin/README.md)  | [![04 Using XAF security system in Xamarin](https://img.youtube.com/vi/mtgj0rcIfEc/0.jpg)](https://www.youtube.com/watch?v=mtgj0rcIfEc)  |
| [05-Using existing XAF database in a Xamarin Forms App](Tutorials/05-Using-Existing-Database/README.md)  | [![05-Using existing database](https://img.youtube.com/vi/Cc6R4dMz0qk/0.jpg)](https://www.youtube.com/watch?v=Cc6R4dMz0qk)  |
  [06-Using XAF Custom LogonParameters and Authentication in Xamarin](Tutorials/06-Using-XAF-Custom-LogonParameters-and-Authentication-in-Xamarin/)  | [![06-Using XAF Custom LogonParameters and Authentication in Xamarin](https://img.youtube.com/vi/y8gq1zbA8b0/0.jpg)](https://www.youtube.com/watch?v=y8gq1zbA8b0)  |
### Integrations
| Source  | Video |
| ------------- | ------------- |
| [01 Synchronizing XAF Postgres database with Sqlite in Xamarin Forms](Integrations/01-XAF-Synchronizing-Postgres-with-Sqlite-in-XamarinForms/README.md)  | [![01 XAF Synchronizing Postgres with Sqlite in Xamarin Forms](https://img.youtube.com/vi/KJIQBramrGs/0.jpg)](https://www.youtube.com/watch?v=KJIQBramrGs)  |