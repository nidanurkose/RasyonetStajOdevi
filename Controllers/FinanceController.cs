using Microsoft.AspNetCore.Mvc;
using RasyonetStaj.Services;
using RasyonetStaj.Models;

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

    // Veri Çekme (POST)
    [HttpPost("fetch/{symbol}")]
    public async Task<IActionResult> FetchStock(string symbol)
    {
        try 
        {
            var result = await _stockService.FetchAndSaveStockAsync(symbol.ToUpper());
            return Ok(result);
        }
        catch (Exception)
        {
            return Problem("An error occurred while fetching data. Please try again later.");
        }
    }

    // Tüm verileri listeleme (GET)
    [HttpGet("all")] // BURAYI TEKE DÜŞÜRDÜK
    public async Task<ActionResult<List<StockDto>>> GetAll()
    {
        var stocks = await _stockService.GetAllStocksAsync();
        return Ok(stocks);
    }

    // Veri Silme (DELETE)
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _stockService.DeleteStockAsync(id);
        if (!success) return NotFound("Stock not found.");
        return Ok("Deleted successfully.");
    }
}