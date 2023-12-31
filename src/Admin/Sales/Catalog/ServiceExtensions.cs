using Blazored.LocalStorage;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

using MudBlazor;

using YourBrand.Admin.AppBar;
using YourBrand.Admin.NavMenu;
using YourBrand.Admin.Services;
using YourBrand.Catalog;

using Store = YourBrand.Admin.Services.Store;

namespace YourBrand.Admin.Sales.Catalog;

public static class ServiceExtensions
{
    public static IServiceProvider UseCatalog(this IServiceProvider services)
    {
        services.InitNavBar();
        services.InitAppBarTray();

        return services;
    }

    private static void InitNavBar(this IServiceProvider services)
    {
        var navManager = services
           .GetRequiredService<NavManager>();

        var t = services.GetRequiredService<IStringLocalizer<YourBrand.Admin.Sales.Catalog.Resources>>();

        var group = navManager.GetGroup("sales") ?? navManager.CreateGroup("sales", () => t["Sales"]);
        group.RequiresAuthorization = true;

        var catalogItem = group.CreateGroup("catalog", options =>
        {
            options.NameFunc = () => t["Catalog"];
            options.Icon = MudBlazor.Icons.Material.Filled.Book;
        });

        catalogItem.CreateItem("products", () => t["Products"], MudBlazor.Icons.Material.Filled.FormatListBulleted, "/products");

        catalogItem.CreateItem("categories", () => t["Categories"], MudBlazor.Icons.Material.Filled.Collections, "/products/categories");

        catalogItem.CreateItem("attributes", () => t["Attributes"], MudBlazor.Icons.Material.Filled.List, "/products/attributes");

        catalogItem.CreateItem("brands", () => t["Brands"], MudBlazor.Icons.Material.Filled.List, "/brands");

        catalogItem.CreateItem("stores", () => t["Stores"], MudBlazor.Icons.Material.Filled.Store, "/stores");
    }

    private static void InitAppBarTray(this IServiceProvider services)
    {
        var appBarTray = services
            .GetRequiredService<IAppBarTrayService>();

        var snackbar = services
            .GetRequiredService<ISnackbar>();

        var t = services.GetRequiredService<IStringLocalizer<YourBrand.Admin.Sales.Resources>>();

        appBarTray.AddItem(new AppBarTrayItem("show", () => t["Store"], typeof(StoreSelector)));
    }
}