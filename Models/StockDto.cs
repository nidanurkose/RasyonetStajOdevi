namespace RasyonetStaj.Models;

public class StockDto
{
    public string Symbol { get; set; }
    public decimal Price { get; set; }
    public DateTime LastUpdated { get; set; }
}