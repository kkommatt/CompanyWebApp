using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CompanyWebApp.Models;

public partial class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Назва")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Версія")]
    public string Version { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Ціль")]
    public string Appointment { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Дистрибутив")]
    public string Distribution { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Реліз")]
    public string ReleaseDate { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Ціна")]
    public int Price { get; set; }

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Мова програмування")]
    public string Language { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Фічи")]
    public string Features { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Примітки")]
    public string Info { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Компанія")]
    public int CompanyId { get; set; }

    public virtual Company Company { get; set; } = null!;
}
