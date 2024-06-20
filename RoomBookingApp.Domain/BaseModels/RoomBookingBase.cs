using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
namespace RoomBookingApp.Domain.BaseModels;

public abstract class RoomBookingBase : IValidatableObject
{
    [Required]
    [StringLength(80)]
    public string FullName { get; set; }

    [Required]
    [StringLength(80)]
    [EmailAddress]
    public string Email { get; set; }

    [DataType(DataType.Date)]
    public DateTime Date { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Date < DateTime.Now.Date)
        {
            yield return new ValidationResult("Data Must be In The Future", [nameof(Date)]);
        }
    }
}