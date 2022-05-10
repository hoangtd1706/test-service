namespace Ecoba.IdentityService.Data.Model;

using System;
using System.ComponentModel.DataAnnotations;

public class User
{
    [Key]
    public string Username { get; set; }
    public string EmployeeId { get; set; }
    public string DisplayName { get; set; }
    public string? GivenName { get; set; }
    public string? Surname { get; set; }
    public string? JobTitle { get; set; }
    public string Mail { get; set; }
    public string? MobilePhone { get; set; }
    public string? OfficeLocation { get; set; }
    public string? PreferredLanguage { get; set; }
    public string? UserPrincipalName { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public bool Editable { get; set; }
    public bool IsActive { get; set; }

    public bool IsAdmin()
    {
        return (this.EmployeeId == "0000");
    }
}