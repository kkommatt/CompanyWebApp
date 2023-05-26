using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace CompanyWebApp.Models;

public partial class Citizen
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Номер паспорту")]
    public int Passport { get; set; }

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Ідентифікаційний код")]
    public int IdentifyCode { get; set; }

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Прізвище та ім'я")]
    public string FullName { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Вік")]
    public int Age { get; set; }

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Населений пункт")]
    public string City { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Вулиця")]
    public string Street { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Номер будинку")]
    public int NoHouse { get; set; }

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Номер квартири")]
    public int? NoFlat { get; set; }

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Освіта")]
    public string? Education { get; set; }

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Країна")]
    public int CountryId { get; set; }

    [Display(Name = "Країна")]
    public virtual Country Country { get; set; } = null!;

    public virtual ICollection<Programmer> Programmers { get; set; } = new List<Programmer>();
}
