namespace CountryAPP.Core.Model;

public class UserInfoModel
{
    public string Id { get; set; } 

    public string Password { get; set; } 

    public string Name { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public int IsActive { get; set; }

    public string refresh { get; set; }
    public string access { get; set; }
    public int expiresIn { get; set; }
}