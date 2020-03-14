# Mangary

![Mangary](/README_media/Mangary.gif)

## Dependencies

- **bootstrap**
- **font-awesome-4.7.0**
- **jquery**
- **jquery-validation**
- **jquery-validation-unobtrusive**
- **efcore**
- **libman**

*Make sure that the paths are pointing to existing files in Views/Shared/_Layout.cs.*

## How to install

**Step 1: clone this repo**
```bash
$ git clone https://github.com/alochaus/Mangary.git
$ cd Mangary
```

**Step 2: install dependencies**
```bash
$ dotnet tool install -g dotnet-ef
$ dotnet tool install -g Microsoft.Web.LibraryManager.Cli
$ dotnet restore
$ libman restore
```

**Step 4: create database**
```bash
$ dotnet ef database update
```

**Step 3: run it**
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
