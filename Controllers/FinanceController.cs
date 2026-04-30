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

    [HttpPost("fetch/{symbol}")]
    public async Task<IActionResult> FetchStock(string symbol)
    {
        var result = await _stockService.FetchAndSaveStockAsync(symbol);
        return Ok(result);
    }
}