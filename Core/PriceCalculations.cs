﻿namespace Core;

public static class PriceCalculations
{
    public static decimal AddVat(this decimal subTotal, double vatRate)
    {
        return subTotal * (decimal)(1 + vatRate);
    }

    public static decimal GetVatFromSubTotal(this decimal subTotal, double vatRate)
    {
        return subTotal * (decimal)vatRate;
    }

    public static decimal GetVatFromTotal(this decimal total, double vatRate)
    {
        return total - total.GetSubTotal(vatRate);
    }

    public static decimal GetSubTotal(this decimal total, double vatRate)
    {
        if (vatRate == 0)
        {
            return total;
        }

        if (vatRate == 0.25)
        {
            return total * 0.8m;
        }
        else if (vatRate == 0.12)
        {
            return total * 1 / 1.12m;
        }
        else if (vatRate == 0.06)
        {
            return total * 1 / 1.06m;
        }

        return total / (1m + (decimal)vatRate);
    }

    public static decimal ApplyRot(this decimal total)
    {
        return total * (1 - 0.3m);
    }

    public static decimal ApplyRut(this decimal total)
    {
        return total * 0.5m;
    }

    public static decimal GetRot(this decimal total)
    {
        return total * 0.3m;
    }

    public static decimal GetRut(this decimal total)
    {
        return total * 0.5m;
    }

    public static double CalculateDiscountPercent(decimal discountPrice, decimal regularPrice)
    {
        return Math.Round((double)((regularPrice - discountPrice) / regularPrice * 100), 0, MidpointRounding.AwayFromZero);
    }

    public static double CalculateDiscountRate(decimal discountPrice, decimal regularPrice)
    {
        return CalculateDiscountPercent(discountPrice, regularPrice) / 100;
    }
}