using System;
using System.IO;
using System.Net;
using System.Web.Services;
using Newtonsoft.Json.Linq;

/// <summary>
/// StockPrice Web Service
/// </summary>
[WebService(Namespace = "http://grimlock257.github.io/Stocks")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class StockPrice : WebService
{
    /// <summary>
    /// Retrieve the share price information from the remote API and return
    /// as a object containing the relevant values
    /// </summary>
    /// <param name="symbol">The symbol for which to search</param>
    /// <returns>An object that contains the market price, update time and currency</returns>
    [WebMethod]
    public StockPriceResponse GetSharePrice(string symbol)
    {
        // If parameter is null, return null
        if (symbol == null) return null;

        // Request the stock price from the API and return
        return RequestSharePrice(symbol);
    }

    /// <summary>
    /// Request the share price for the supplied symbol from the yahoo API
    /// </summary>
    /// <param name="symbol">The symbol for which to search</param>
    /// <returns>An object that contains the market price, update time and currency</returns>
    private static StockPriceResponse RequestSharePrice(string symbol)
    {
        // Construct the URL
        var url = "https://query1.finance.yahoo.com/v8/finance/chart/" + symbol + "?interval=1d&range=1d";

        // Create the WebRequest object to send to the URL
        var req = WebRequest.Create(url);
        req.ContentType = "application/json; charset=utf-8";
        req.Method = "GET";

        // Retrieve the response from the request, if null, return null
        var responseSteam = req.GetResponse().GetResponseStream();

        if (responseSteam == null) return null;

        // Read the response stream and store a trimmed version
        var sr = new StreamReader(responseSteam);
        var response = sr.ReadToEnd().Trim();

        // Access the relevant fields from the JSON response (the current market price, currency and updated time)
        var rootObject = JObject.Parse(response);
        var metaObject = rootObject["chart"]?["result"]?[0]?["meta"];
        var regularMarketPriceStr = metaObject?["regularMarketPrice"]?.ToString();
        var regularMarketTimeStr = metaObject?["regularMarketTime"]?.ToString();
        var stockCurrency = metaObject?["currency"]?.ToString();

        var regularMarketPrice = double.Parse(regularMarketPriceStr ?? string.Empty);
        var regularMarketTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(regularMarketTimeStr)).DateTime;

        // Construct the response object and return
        var stockPriceResponse = new StockPriceResponse
        {
            StockPrice = regularMarketPrice,
            StockCurrency = stockCurrency,
            StockPriceTime = regularMarketTime,
        };

        return stockPriceResponse;
    }
}
