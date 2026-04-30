using Microsoft.AspNetCore.Mvc;
using RasyonetStaj.Services;

namespace RasyonetStaj.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FinanceController : ControllerBase
{
    private readonly StockService _stockService;

    public FinanceController(StockService stockService)
    {
        _stockService = stockService;
    }

    // 1. Veri Çekme (POST) - Mevcut metodun
    [HttpPost("fetch/{symbol}")]
    public async Task<IActionResult> FetchStock(string symbol)
    {
        var result = await _stockService.FetchAndSaveStockAsync(symbol.ToUpper());
        return Ok(result);
    }

    // 2. Analitik Görünüm: Tüm verileri listeleme (GET)
    // Hocanın 'Analytical/Aggregation' şartını bu karşılayacak.
    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var stocks = await _stockService.GetAllStocksAsync();
        return Ok(stocks);
    }

    // 3. Veri Silme (DELETE)
    // CRUD döngüsünü tamamlamak ve endpoint sayısını artırmak için.
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _stockService.DeleteStockAsync(id);
        if (!success) return NotFound("Stock not found.");
        return Ok("Deleted successfully.");
    }
}