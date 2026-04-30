using RasyonetStaj.Data;
using RasyonetStaj.Models;
using System.Text.Json;

namespace RasyonetStaj.Services;

public class StockService
{
    private readonly AppDbContext _context;
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public StockService(AppDbContext context, HttpClient httpClient, IConfiguration config)
    {
        _context = context;
        _httpClient = httpClient;
        _apiKey = config["ApiKey"] ?? "demo";
    }

    public async Task<string> FetchAndSaveStockAsync(string symbol)
    {
        // Hocanın istediği Alpha Vantage API adresi
        var url = $"https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol={symbol}&apikey={_apiKey}";
        
        var response = await _httpClient.GetStringAsync(url);
        using var doc = JsonDocument.Parse(response);
        
        if (doc.RootElement.TryGetProperty("Global Quote", out var quote))
        {
            var priceString = quote.GetProperty("05. price").GetString();
            if (decimal.TryParse(priceString, out decimal price))
            {
                var stock = new Stock
                {
                    Symbol = symbol,
                    Price = price,
                    LastUpdated = DateTime.Now
                };

                _context.Stocks.Add(stock);
                await _context.SaveChangesAsync();
                return $"Başarıyla kaydedildi: {symbol} - {price}";
            }
        }
        return "Veri çekilemedi. API günlük limitine takılmış olabilir.";
    }
}