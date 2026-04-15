using System.ComponentModel.DataAnnotations;

namespace Playon24.PresentationLayer.Modules.Customers.ViewModels
{
    public class CustomerCreateViewModel
    {
        [Required, StringLength(100)]
        [Display(Name = "First name")]
        public string FirstName { get; set; } = string.Empty;

        [Required, StringLength(100)]
        [Display(Name = "Last name")]
        public string LastName { get; set; } = string.Empty;

        [Required, StringLength(200), EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(20)]
        [Display(Name = "Phone")]
        public string Phone { get; set; } = string.Empty;

        [Required, StringLength(300)]
        public string Address { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string City { get; set; } = string.Empty;
    }
}
