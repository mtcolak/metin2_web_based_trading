using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    public int id { get; set; }
    public string login { get; set; }
    public string password { get; set; }
    public string email { get; set; }

}
