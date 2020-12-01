using Newtonsoft.Json;
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
    [WebMethod]
    public string GetStockPrice(string symbol)
    {
        // If parameter is null, return an empty string
        if (symbol == null) return "";

        // Request the stock price from the API
        var sharePrice = RequestSharePrice(symbol);

        // Return the response string
        return sharePrice;
    }

    /// <summary>
    /// Request the share price for the supplied symbol from the yahoo API
    /// </summary>
    /// <param name="symbol">The symbol for which to search</param>
    /// <returns>The current share price for the supplied symbol</returns>
    public static string RequestSharePrice(string symbol)
    {
        // Construct the URL
        var url = "https://query1.finance.yahoo.com/v8/finance/chart/" + symbol + "?interval=1d&range=1d";

        // Create the WebRequest object to send to the URL
        var req = WebRequest.Create(url);
        req.ContentType = "application/json; charset=utf-8";
        req.Method = "GET";

        // Retrieve the response from the request, if null, return empty string
        var responseSteam = req.GetResponse().GetResponseStream();

        if (responseSteam == null) return "";

        // Read the response stream and store a trimmed version
        var sr = new StreamReader(responseSteam);
        var response = sr.ReadToEnd().Trim();

        // Access the regularMarketPrice field
        var rootObject = JObject.Parse(response);
        var metaObject = rootObject["chart"]?["result"]?[0]?["meta"];
        var regularMarketPrice = metaObject?["regularMarketPrice"]?.ToString();
        var regularMarketTime = metaObject?["regularMarketTime"]?.ToString();

        return regularMarketPrice + "|" + regularMarketTime;
    }
}
