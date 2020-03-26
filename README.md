# Mangary

![Mangary](/README_media/Mangary.gif)

## Dependencies
- **Bootstrap@4.3.1**
- **Entity Framework Core@3.0.0**
- **Font Awesome@4.7.0**
- **JQuery Validation Unobtrusive@3.2.11**
- **Jquery Validation@1.17.0**
- **JQuery@3.3.1**
- **LibMan@2.0.96+g65c9be8001**
- **Npgsql@3.0.0**

## How to install

**Step 1: clone this repo**
```bash
$ git clone https://github.com/alochaus/Mangary.git
$ cd Mangary
$ mkdir wwwroot/UploadedPhotos
```

**Step 2: install dependencies**
```bash
$ dotnet tool install -g dotnet-ef
$ dotnet tool install -g Microsoft.Web.LibraryManager.Cli
$ dotnet restore
$ libman restore
```
**Step 3: connection string**

**Linux and MacOS:**

3.1: Open ~/.bash_profile with a text editor.

3.2: Add the following line and replace **YourConnectionString** by your connection string:
```bash
export MangaryConnectionString="YourConnectionString"
```
*Note: keep the double quotes.*
```bash
Host=localhost;Database=mangary;Username=aloc;Password=mysupersecretpassword
```
If you want to enable other features (e.g., pooling) you can check [this page](https://www.npgsql.org/doc/connection-string-parameters.html).

3.3: Run the script.
```bash
$ source ~/.bash_profile
```

**Windows:**

I don't know how to set environment variables on Windows but [this page](https://www.computerhope.com/issues/ch000549.htm) might help you. Make sure that the key is MangaryConnectionString and the value is the connection string.

**Step 4: create database**
```bash
$ dotnet ef database update
```

**Step 5: run it**
```bash
$ dotnet run
```

## Features

- Responsive layout
- Accounts
- Roles (admin and manager)
- Create, edit and delete products (only available for users with manager role)
- Role manager (only available for users with admin role)
- Shopping cart
- Search by name or category
- Tables designed for the app to be scalable to as many categories as you want
