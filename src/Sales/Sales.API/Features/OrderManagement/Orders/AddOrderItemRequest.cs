namespace YourBrand.Sales.API.Features.OrderManagement.Orders;

public sealed record AddOrderItemRequest(string Description, string? ItemId, string? Unit, decimal UnitPrice, double Quantity, double VatRate, string? Notes);

public record UpdateOrderItemRequest(string Description, string? ItemId, string? Unit, decimal UnitPrice, double Quantity, double VatRate, string? Notes);