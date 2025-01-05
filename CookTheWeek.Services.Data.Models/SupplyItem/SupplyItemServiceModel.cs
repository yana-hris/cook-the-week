namespace CookTheWeek.Services.Data.Models.SupplyItem
{
    
    public class SupplyItemServiceModel
    {
        public string Name { get; set; } = null!;
        public decimal Qty { get; set; }
        public int MeasureId { get; set; }
        public int CategoryId { get; set; }
        public string? Note { get; set; }
    }
}
