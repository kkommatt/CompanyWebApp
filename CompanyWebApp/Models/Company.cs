using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CompanyWebApp.Models;

public partial class Company
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Назва")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Місто")]
    public string City { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Вулиця")]
    public string Street { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Керівник")]
    public string Header { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "К-ть працівників")]
    public int StaffCount { get; set; }

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Країна")]
    public int CountryId { get; set; }

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Веб-сайт")]
    public string Website { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Електронна адреса")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "ЄДРОПОУ")]
    public int Edrpou { get; set; }

    public virtual Country Country { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<Programmer> Programmers { get; set; } = new List<Programmer>();
}
