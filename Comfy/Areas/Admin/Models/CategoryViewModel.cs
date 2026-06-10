using System.ComponentModel.DataAnnotations;

namespace Comfy.Areas.Admin.Models
{
    public class CategoryViewModel
    {
        [Required(ErrorMessage = "Назва категорії є обов'язковою")]
        [StringLength(100, ErrorMessage = "Назва категорії не може бути довшою за 100 символів")]
        public string Name { get; set; }
    }
}
