using System;
using System.Web.Services;

/// <summary>
/// StockPrice Web Service
/// </summary>
[WebService(Namespace = "http://grimlock257.github.io/Stocks")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class StockPrice : WebService
{
    [WebMethod]
    public string HelloWorld(String name)
    {
        return "Hello " + name;
    }
}
