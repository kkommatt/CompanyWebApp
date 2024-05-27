using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CompanyWebApp.Models;

public partial class Programmer
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Посада")]
    public string Specialization { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Громадянин")]
    public int CitizenId { get; set; }

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Компанія")]
    public int CompanyId { get; set; }

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Досвід")]
    public int YearsExperience { get; set; }

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Ранг")]
    public string Range { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Мова програмування")]
    public string Language { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "ЗП")]
    public int Salary { get; set; }

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Part/Full -time")]
    public string Time { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Вдома/Офіс")]
    public string Place { get; set; } = null!;

    public virtual Citizen Citizen { get; set; } = null!;

    public virtual Company Company { get; set; } = null!;
    public List<ProgrammerProduct> ProgrammerProducts { get; set; }
    public IEnumerable<Product> Products => ProgrammerProducts?.Select(pp => pp.Product).ToList();
}
