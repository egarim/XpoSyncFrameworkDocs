# Using an existing database

Sometimes your mobile client should use data that already exist and that it was not created
with the SyncFramework components.

That is a problem because, if the data is already created that means that there are no deltas
containing that information.

The solution is to include in the clients a database with the same data that exist in the master,
that means that all databases (the master and clients) are in the same state

Once you have distributed a copy of the database with the same data as the master you can turn on delta tracking
in your connection string and start synchronizing data

## The tools for this example

In this case I'm using MS SQL Server for the master database and SQLite in the Xamarin client 

There are many tools to migrate databases from X engine to Y engine, you can use the tools that best fit your needs

For this example, I'm going to use SQLite & SQL Server Compact Toolbox by Erik Ejlskov Jensen

Visual Studio Extension

https://marketplace.visualstudio.com/items?itemName=ErikEJ.SQLServerCompactSQLiteToolbox

Github Repo

https://github.com/ErikEJ/SqlCeToolbox

***Note:*** You need to make sure that your migration generates a valid database and that all the relations in the data
are correct.


## The flow 

- Turn off DeltaTracking in your master database and generate some data, see the connections string example below, notice
the parameter ***EnableDeltaTracking='false'***
```<language>
XpoProvider=SyncDataStore;DataConnectionString='Integrated Security=SSPI;Pooling=false;Data Source=(localdb)\mssqllocaldb;Initial Catalog=UsingExistingDb';DeltaConnectionString='Integrated Security=SSPI;Pooling=false;Data Source=(localdb)\mssqllocaldb;Initial Catalog=UsingExistingDbDeltas';Identity=MASTER;EnableDeltaTracking='false';ExcludedEntities=''
```
- Use the tools of your choice to generate a valid copy of your master database for SQLite database engine
- Include a copy of SQLite database with your mobile application 
- Save your SQLite database in the device

## The code

as you can see if you check the [UsingExistingDb.Mobile.csproj](UsingExistingDb.Mobile/UsingExistingDb.Mobile.csproj) file
you will notice that I include the database (UsingExistingDb.db) with existing data  as embedded resource

```xml
	<ItemGroup>
	  <None Remove="UsingExistingDb.db" />
	</ItemGroup>
	
	<ItemGroup>
	  <EmbeddedResource Include="UsingExistingDb.db" />
	</ItemGroup>

```

And this is the code I added in [SyncSettingsPageViewModel](UsingExistingDb.Mobile/ViewModels/SyncSettingsPageViewModel.cs) 
ExecuteSaveSettingsCommand method, to get the database from the embedded resource and copy it to the local file system

```csharp

            //Get the path for your local "Data" database
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var DbPath=Path.Combine(folderPath,"Data.db");

            //if the database does not exist get the database with existing data from the resources and save it in the
            //in the file system
            if(!File.Exists(DbPath))
            {
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = "UsingExistingDb.Mobile.UsingExistingDb.db";
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    MemoryStream memoryStream = new MemoryStream();
                    stream.CopyTo(memoryStream);
                    File.WriteAllBytes(DbPath, memoryStream.ToArray());
                }
            }
             
            //Just continue the normal flow  

```

**Safe guard**
To make sure that the database that we are shipping with the app is readable by XPO/XAF, I added the code below to the [LoginPageViewModel](UsingExistingDb.Mobile/ViewModels/LoginPageViewModel.cs) 


```csharp
                try
                {
                    var Users = SyncHelper.CreateNonSecuredObjectSpace().CreateCollection(typeof(ApplicationUser)).Cast<ApplicationUser>().ToList();
                    var Admin = Users.FirstOrDefault(u => u.UserName == "Admin");
                    var RolesCount = Admin.Roles.Count;
                    foreach (DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyRole permissionPolicyRole in Admin.Roles)
                    {
                        Debug.WriteLine(permissionPolicyRole.Name);
                        Debug.WriteLine(permissionPolicyRole.IsAdministrative);
                    }
                    var LoginValid = Admin.ComparePassword(this.Password);
                }
                catch (Exception ex)
                {

                    var message = $"There is a problem with the initial data{ex.Message}";
                }
```









