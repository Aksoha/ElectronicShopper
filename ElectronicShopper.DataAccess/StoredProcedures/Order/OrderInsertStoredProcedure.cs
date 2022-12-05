﻿namespace ElectronicShopper.DataAccess.StoredProcedures.Order;

internal class OrderInsertStoredProcedure : IStoredProcedure
{
    public int UserId { get; set; }

    public string ProcedureName()
    {
        return "spOrder_Insert";
    }
}