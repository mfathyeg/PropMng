using System.ComponentModel.DataAnnotations;

namespace PropMng.Web.Data
{
    public class LoginDto
    {
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

    }
}
