using System.ComponentModel.DataAnnotations;

namespace Comfy.ViewModels
{
    public class ProductViewModel
    {

        [Required(ErrorMessage = "Назва продукту обов'язкова")]
        [StringLength(100, ErrorMessage = "Назва не може бути довшою за 100 символів")]
        public string Name { get; set; }

        [Display(Name = "Зображення")]
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Опис обов'язковий")]
        [StringLength(500, ErrorMessage = "Опис не може бути довшим за 500 символів")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Ціна обов'язкова")]
        [Range(0.01, 100000, ErrorMessage = "Ціна має бути більшою за 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Кількість обов'язкова")]
        [Range(0, int.MaxValue, ErrorMessage = "Кількість не може бути від’ємною")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Категорія обов'язкова")]
        [Display(Name = "Категорія")]
        public int CategoryId { get; set; }

        public string? CategoryName { get; set; }
    }
}
