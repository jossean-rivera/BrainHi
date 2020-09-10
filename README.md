# Overview
This project consists of a web application built on ASP.NET Core MVC where users can find a health provider and submit an appointment to the office. Health providers can also be added to the system. The project was created following the instructions of [BrainHi Software Engineer Interview Project](https://www.notion.so/BrainHi-Software-Engineer-Interview-Project-c973a3794852449a818c82b4b6c9e714). 

# Getting Started
### Software Dependencies
Make sure you have installed the following:

 - [Git](https://git-scm.com/downloads)
 - [.NET Core SDK](https://dotnet.microsoft.com/download) (version >= 3.1)
 - [Microsoft SQL Server - Developer Edition](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Optional, Windows recommended)

Make sure to install the .NET Core SDK and not just the .NET Core Runtime, you will need the SDK to build and run the application. The SDK package also includes the runtime. After the installation, you can run the command `dotnet --version` to see if it was successfully installed.

```cmd
C:\>dotnet --version
3.1.401
```

The application by default will use the memory of the host computer to save object like if it was saving objects to a database. However, the application is ready to use a Microsoft SQL database as back-end storage. If you use Windows, installing MS SQL Server is very simple (specially for the developer edition). If you use MacOS or Linux, then it's a little harder to install it but it can be done. Also, I use Entity Framework Core (EF Core) for Object-Relational Mapping (ORM) and we don't need to stick with MS SQL Server. He just have to change the database provider for EF Core and we can use other types of databases such as Oracle, MySql, etc. [Here's a full list of other supported databases that you could use](https://docs.microsoft.com/en-us/ef/core/providers/?tabs=dotnet-core-cli).

### Build & Run
Follow the next steps to run the web application localy.

 1. Clone or download the Git repository in a convenient location. 
```
C:\>git clone https://github.com/jossean-rivera/BrainHi.git
```
2. Move to the root directory (the directory that contains the `BrainHi.sln` file) 
```
C:\>cd .\BrainHi
C:\BrainHi>
```
3. Use the DotNet SDK to build and run the web application project (you might need to use the sudo command on MacOS)
```
C:\BrainHi\>dotnet run -p .\BrainHi.WebApp\BrainHi.WebApp.csproj
```
4. While the application is running open your browser on `https://localhost:5001/` or `http://localhost:5000` if you don't want to use HTTPS. If you use HTTPS, you must trust the SSL certficate at the "Insecure" warning message that you'll see in your browser. 
5. Use `Ctrl + C` to stop the application when you're done with testing. 

# Switch to SQL Server as Backend
Currently, the application is using the [`MemoryApplicationRepository`](https://github.com/jossean-rivera/BrainHi/blob/master/BrainHi.Data/MemoryApplicationRepository.cs) to save objects in memory and mock the repository as if it was the database. I developed this class in case you don't have a MS SQL Server that you can use for testing. However, a real application should use a database as back-end storage.

If you have a MS SQL Server for testing, then **create a database in the server and get the connection string** to that database. You can even use Microsfot Azure or AWS to get a MS SQL database. Then, you must **update the connection string in the [`appsettings.json`](https://github.com/jossean-rivera/BrainHi/blob/master/BrainHi.WebApp/appsettings.json) file with your connection string**.

```json
"ConnectionStrings": {
	"DefaultConnection": "Server=localhost;Database=BrainHi;Trusted_Connection=true;"
}
``` 
Then, you need to **install the EF Core tools for the DotNet SDK** so that we can update the schema of your empty database. Run the following command to install the [`dotnet-ef`](https://www.nuget.org/packages/dotnet-ef/3.1.8) global tool.
```
C:\BrainHi> dotnet tool install --global dotnet-ef --version 3.1.8
```
Now, we need to switch the [`MemoryApplicationRepository`](https://github.com/jossean-rivera/BrainHi/blob/master/BrainHi.Data/MemoryApplicationRepository.cs) with the [`ApplicationRepository`](https://github.com/jossean-rivera/BrainHi/blob/master/BrainHi.Data/ApplicationRepository.cs) as a service that implements the [`IApplicationRepository`](https://github.com/jossean-rivera/BrainHi/blob/master/BrainHi.Data/IApplicationRepository.cs) interface. **Go to the [`Startup.cs`](https://github.com/jossean-rivera/BrainHi/blob/master/BrainHi.WebApp/Startup.cs#L22) file and update the following code:**

```c#
public void ConfigureServices(IServiceCollection services)
{
	/*
	 * IF YOU HAVE A SQL DATABASE FOR TESTING THEN
	 *  1. UNCOMMENT THE NEXT TWO LINE, 
	 *  2. MAKE SURE THAT THE 'appsetting.json' FILE CONTAINS THE CORRECT CONNECTION STRING TO THE DATABASE 
	 *  3. AND COMMENT THE FOLLOWING LINE: services.AddSingleton<IApplicationRepository, MemoryApplicationRepository>();
	 */
	//  Add DB Context, and Add Db context as inteface
	services.AddDbContext<BrainHiContext>(options => options.UseSqlServer(Config.GetConnectionString("DefaultConnection")));
	services.AddScoped<IApplicationRepository, ApplicationRepository>();

	//  IF YOU DON'T HAVE A SQL DATABASE, then use the Memory repository that will keep the objects in memory for testing (not suitable for production)
	//services.AddSingleton<IApplicationRepository, MemoryApplicationRepository>();

	//  Add MVC services
	services.AddControllersWithViews()
		.AddRazorRuntimeCompilation();
}
```


Now, we need to update the schema of our empty database. **Run the next command to update the schema of the empty database by applying the migrations in the Data project:**
```
C:\BrainHi>dotnet ef database update -p .\BrainHi.Data\BrainHi.Data.csproj -s .\BrainHi.WebApp\BrainHi.WebApp.csproj
Build started...
Build succeeded.
Done.
```

Done. Now you can run the web application and use your SQL database as back-end storage.
```
C:\BrainHi\>dotnet run -p .\BrainHi.WebApp\BrainHi.WebApp.csproj
```
