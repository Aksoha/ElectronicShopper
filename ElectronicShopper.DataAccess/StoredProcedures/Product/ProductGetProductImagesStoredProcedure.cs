﻿namespace ElectronicShopper.DataAccess.StoredProcedures.Product;

internal class ProductGetProductImagesStoredProcedure : IStoredProcedure
{
    public int ProductId { get; set; }
    public string ProcedureName()
    {
        return "spProduct_GetProductImages";
    }
}