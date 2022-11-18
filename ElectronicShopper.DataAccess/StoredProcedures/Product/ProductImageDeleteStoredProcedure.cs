﻿namespace ElectronicShopper.DataAccess.StoredProcedures.Product;

internal class ProductImageDeleteStoredProcedure : IStoredProcedure
{
    public int Id { get; set; }

    public string ProcedureName()
    {
        return "spProductImage_Delete";
    }
}