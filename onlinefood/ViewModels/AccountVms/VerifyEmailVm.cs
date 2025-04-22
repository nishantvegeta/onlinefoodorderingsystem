// ViewModels/VerifyEmailVm.cs
using System.ComponentModel.DataAnnotations;

namespace onlinefood.ViewModels
{
    public class VerifyEmailVm
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Verification Code")]
        public string Code { get; set; }
    }
}
