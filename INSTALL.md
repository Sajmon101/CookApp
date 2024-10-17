# CookApp Installation Guide

The following instructions will help you set up and run the CookApp application on a local server.

## Prerequisites

- **.NET SDK 7.0 or later**: Ensure you have the .NET SDK installed to run the WPF and ASP.NET Core Web API applications.
- **SQL Server**: The application uses an SQL Server database. You can use the free SQL Server Express version for application data storage.
- **SQL Server Management Studio (SSMS)**: Recommended for managing SQL Server, SSMS facilitates database setup and maintenance.

## Step 1: Clone the Repository

First, clone the CookApp repository to your local machine:

```bash
git clone https://github.com/Sajmon101/CookApp.git
```

## Step 2: Restore Database from .bak File

1. **Launch SSMS** and connect to your local SQL Server instance.
2. In **Object Explorer**, right-click on `Databases` and select **Restore Database...**.
3. In the **Source** section, select **Device** and click the ellipsis button (`...`).
4. In the **Select backup devices** window, click **Add** and locate the `CookApp.bak` file provided in the project files.
5. Select the .bak file and click **OK**, then click **OK** again in the **Select backup devices** window.
6. In the **Destination** section, choose a database name (e.g., `CookApp`) and click **OK** to restore the database.

Your database should now be ready for use.

## Step 3: Configure appsettings.json Files

### Creating appsettings.json Files

In the `CookApp` and `CookAppAPI` folders, create `appsettings.json` with content as below:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=YourServerName;Database=CookApp;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "ApiSettings": {
    "BaseUrl": "http://localhost:YourPort/"
  }
}
```

**Note**: Customize `Server`, `Database`, and `BaseUrl` according to your local setup.

## Step 4: Launch the Application

### Running the WebAPI

1. Navigate to the `CookAppAPI` folder:

    ```bash
    cd CookAppAPI
    ```

2. Restore project dependencies:

    ```bash
    dotnet restore
    ```

3. Run the WebAPI application:

    ```bash
    dotnet run
    ```

The WebAPI should now be accessible at `http://localhost:36032/`.

### Running the WPF Application

1. Navigate to the `CookApp` folder:

    ```bash
    cd CookApp
    ```

2. Restore project dependencies:

    ```bash
    dotnet restore
    ```

3. Open the CookApp solution in Visual Studio or another compatible editor and run the WPF application.

## Optional: Running Both Projects Simultaneously

If you wish to run both projects simultaneously in Visual Studio, follow these steps:

1. Open the CookApp solution (`CookApp.sln`).
2. In **Solution Explorer**, right-click on the solution name and select **Properties**.
3. In the **Startup Project** section, choose **Multiple startup projects**.
4. Set the **Action** to **Start** for both `CookApp` and `CookAppAPI`.
5. Click **OK** to save the changes.

## Step 5: Testing the Application

The application is now ready to use. Log in as either a waiter or a chef and test the order placement and fulfillment process.

## Troubleshooting

If you encounter issues:
- Ensure that the SQL Server is running and that the database is correctly restored.
- Check the `appsettings.json` configuration in both projects and verify that connection settings are accurate.
