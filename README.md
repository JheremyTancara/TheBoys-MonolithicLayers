# Streaming Project

## Description
This project is an API for a streaming system that uses Entity Framework Core to interact with a MySQL database. It allows for the management of users and movies, as well as functionalities related to managing watchlists.

## Prerequisites
Make sure you have the following components installed on your machine:

- [.NET SDK](https://dotnet.microsoft.com/download) (version 6.0 or higher)
- [MySQL Server](https://dev.mysql.com/downloads/mysql/)
- [MySQL Workbench](https://dev.mysql.com/downloads/workbench/) (Optional)

## Project Setup

### 1. Clone the repository
  ```bash
  git clone https://github.com/JheremyTancara/TheBoys-MonolithicLayers/settings/access?guidance_task=
  cd TheBoys-MonolithicLayers
  ```

### 2. Configure the credentials in `appsettings.json`

  Open the `appsettings.json` file and add the connection string with your MySQL credentials:

  ```json
  {
    "ConnectionStrings": {
      "MySQLConnection": "Server=localhost;Port=NUMBER_PORT;Database=STREAMING_PROJECT;User Id=NAME_USER;Password=PASSWORD"
    },
  }
  ```

Replace the following values:
- `NUMBER_PORT`: The port of your MySQL server (default is 3306).
- `NAME_USER`: Your MySQL username.
- `PASSWORD`: Your MySQL password.

### 3. Update the database

Run the following command to apply migrations and update the database:

```bash
dotnet ef database update
```

### 4. Run the application

To start the application, use the following command:

```bash
dotnet run
```

The API will be available at `https://localhost:5270/swagger/index.html` 