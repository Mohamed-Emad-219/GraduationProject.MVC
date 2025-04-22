using System.ComponentModel.DataAnnotations;

namespace GraduationProject.ViewModels
{
    public class ResetPasswordVM
    {
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
