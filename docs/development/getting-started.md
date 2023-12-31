# Getting started

The entire project can be run on your local computer.

Everything has been pre-configured to work with SQL databases, as well as other dependencies, running in Docker.

## Prerequisites

The following software has to be present on your development machine in order to build and run this project:

* .NET SDK 8
* Docker desktop
* VS Code (or Visual Studio, or Rider)

## VS Code

To ensure code style, "Auto-format on save" has been enabled by default for this repo.

In order to run in VS code you need the following extensions:

* C# language kit
* C# Dev Kit - for the best dev experience (Solution Explorer, Test Explorer, debugging)
* C# IntelliCode

Install the extensions manually, or by running ``install-vscode-extensions.sh``.

This is a "nice to have" that will be installed by running the script:

**Restore Terminals** will make it easier for you to start up all services locally by automatically creating a terminal window for each of them.

## DevContainer

If you don't want to, or can't, install any SDKs on your machine.

As an alternative to doing development in a local VS Code instance, you can run this container environment on your machine, or in a DevContainer in the cloud, using a service provider such as GitHub Codespaces.

Read more [here](/docs/development/devcontainer.md).

## Run dependencies in Docker

To start the database and other [dependencies](/docs/services.md), run this:

```
docker compose -f docker-compose.deps.yml up -d
```

## Set up Azurite

To upload and serve blobs (product images) you need to configure Azurite.

More on that [here](setting-up-azurite.md).

## Run individual services from CLI

You can start individual services using these commands:

```sh
dotnet run --project src/Store/Web/Server
dotnet run --project src/Admin/Web/Server
dotnet run --project src/Catalog/Catalog.API
dotnet run --project src/Carts/Carts.API
```

### Restore Terminal in VS Code

You can use the [Restore Terminal](https://marketplace.visualstudio.com/items?itemName=EthanSK.restore-terminals) VS Code extension to restore Terminal windows with these commands.

The terminals are configured in the ``.vscode/settings.json`` file.

## Service hosts

The service will run at these endpoints:

* Store Web: https://localhost:7188/
* Admin Web: https://localhost:5001/
* Catalog https://localhost:7134/
* Carts: https://localhost:7154/

Each service has a Swagger UI endpoint at ``/swagger``.

## Seeding data

### Databases

```sh
dotnet run --project src/Carts/Carts.API --seed
dotnet run --project src/Catalog/Catalog.API --seed
```

### Blob Storage

To seed product images, follow [this guide](/docs/seed/blobs/README.md).
