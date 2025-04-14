using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountryAPP.Core.Model;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("users")]
public class UserModel
{
    [Key]
    public string Email { get; set; } // Primary key is Email

    public string? Id { get; set; } 

    public string FullName { get; set; }

    public string Password { get; set; }

    [NotMapped] // Not mapped to DB
    public string ConfirmPassword { get; set; }

    public string PhoneNumber { get; set; }

    public int IsActive { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpires { get; set; }
}

