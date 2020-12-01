using System;

/// <summary>
/// Represent a return type for this web service
/// </summary>
public class StockPriceResponse
{
    public double StockPrice { get; set; }
    public string StockCurrency { get; set; }
    public DateTime StockPriceTime { get; set; }
}