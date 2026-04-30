using Microsoft.EntityFrameworkCore;
using RasyonetStaj.Data;
using RasyonetStaj.Models;
using System.Text.Json;
using System.Linq; 
using System.Globalization;

namespace RasyonetStaj.Services;

// DESIGN PATTERN: Service Pattern & Dependency Injection
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

    // Dışarıdan Veri Çekme ve Kaydetme
    public async Task<string> FetchAndSaveStockAsync(string symbol)
    {
        var url = $"https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol={symbol}&apikey={_apiKey}";
        var response = await _httpClient.GetStringAsync(url);
        using var doc = JsonDocument.Parse(response);
        
        if (doc.RootElement.TryGetProperty("Global Quote", out var quote))
        {
            if (quote.TryGetProperty("05. price", out var priceElement))
            {
                var priceString = priceElement.GetString();
                // NumberStyles ve InvariantCulture kullanarak '.' ayracını her dilde doğru okuyoruz.
                if (decimal.TryParse(priceString, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal price))
                {
                    var stock = new Stock { Symbol = symbol, Price = price, LastUpdated = DateTime.Now };
                    _context.Stocks.Add(stock);
                    await _context.SaveChangesAsync();
                    return $"Success: {symbol} saved at price {price}.";
                }
            }
        }
        return "Limit reached or invalid symbol.";
    }

    // Verileri çekip DTO'ya dönüştürerek fiyata göre sıralar
    public async Task<List<StockDto>> GetAllStocksAsync()
    {
        // SQLite decimal sıralamayı doğrudan desteklemediği için önce listeyi alıyoruz.
        var stocks = await _context.Stocks.ToListAsync();
        
        return stocks
            .OrderByDescending(s => s.Price)
            .Select(s => new StockDto 
            {
                Symbol = s.Symbol,
                Price = s.Price,
                LastUpdated = s.LastUpdated
            })
            .ToList();
    }

    // Veri Silme
    public async Task<bool> DeleteStockAsync(int id)
    {
        var stock = await _context.Stocks.FindAsync(id);
        if (stock == null) return false;
        _context.Stocks.Remove(stock);
        await _context.SaveChangesAsync();
        return true;
    }
}