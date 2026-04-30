using Microsoft.EntityFrameworkCore;
using RasyonetStaj.Data;
using RasyonetStaj.Models;
using System.Text.Json;

namespace RasyonetStaj.Services;

// DESIGN PATTERN: Service Pattern & Dependency Injection
// Why: Business logic is separated from the controller to ensure 'Separation of Concerns'.
public class StockService
{
    private readonly AppDbContext _context;
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public StockService(AppDbContext context, HttpClient httpClient, IConfiguration config)
    {
        _context = context;
        _httpClient = httpClient;
        _apiKey = config["ApiKey"] ?? "demo"; // appsettings.json'dan API anahtarını alır
    }

    // 1. Dışarıdan Veri Çekme ve Kaydetme
    public async Task<string> FetchAndSaveStockAsync(string symbol)
    {
        var url = $"https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol={symbol}&apikey={_apiKey}";
        var response = await _httpClient.GetStringAsync(url);
        using var doc = JsonDocument.Parse(response);
        
        if (doc.RootElement.TryGetProperty("Global Quote", out var quote))
        {
            var priceString = quote.GetProperty("05. price").GetString();
            if (decimal.TryParse(priceString, out decimal price))
            {
                var stock = new Stock { Symbol = symbol, Price = price, LastUpdated = DateTime.Now };
                _context.Stocks.Add(stock);
                await _context.SaveChangesAsync();
                return $"Success: {symbol} saved at price {price}.";
            }
        }
        return "Limit reached or invalid symbol.";
    }

    // Analitik Görünüm: Tüm hisseleri fiyata göre listeleme
    public async Task<List<Stock>> GetAllStocksAsync()
    {
        // SQLite decimal sıralamayı desteklemediği için önce veriyi çekip 
        // sıralamayı uygulama tarafında (C# tarafında) yapıyoruz.
        var stocks = await _context.Stocks.ToListAsync();
        return stocks.OrderByDescending(s => s.Price).ToList();
    }
    // 3. Veri Silme
    public async Task<bool> DeleteStockAsync(int id)
    {
        var stock = await _context.Stocks.FindAsync(id);
        if (stock == null) return false;
        _context.Stocks.Remove(stock);
        await _context.SaveChangesAsync();
        return true;
    }
}