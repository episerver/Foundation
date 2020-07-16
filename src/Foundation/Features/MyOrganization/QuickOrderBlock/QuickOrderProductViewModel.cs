using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.MyOrganization.QuickOrderBlock
{
    public class QuickOrderProductViewModel
    {
        [Required(ErrorMessage = "Product name is required")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Sku is required")]
        public string Sku { get; set; }

        [Required(ErrorMessage = "Unit price is required")]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Total price is required")]
        public decimal TotalPrice { get; set; }
    }
}