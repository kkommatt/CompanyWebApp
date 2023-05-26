using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CompanyWebApp.Models;

public partial class Country
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Назва")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Континент")]
    public string Continent { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Форма правління")]
    public string Type { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "ВВП в трлн доларах")]
    public int Gdp { get; set; }

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Очільник")]
    public string Header { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Столиця")]
    public string Capital { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Населення в млн")]
    public int Population { get; set; }

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Валюта")]
    public string Currency { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Площа в км2")]
    public int Area { get; set; }

    public virtual ICollection<Citizen> Citizens { get; set; } = new List<Citizen>();

    public virtual ICollection<Company> Companies { get; set; } = new List<Company>();
}
