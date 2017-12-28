using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MVCAgeVerificationDemo.Models
{
    public class AgeVerificationViewModel : IValidatableObject
    {
        [Required(ErrorMessage = "Month is required.")]
        [Range(1, 12)]
        public int? Month { get; set; }

        [Required(ErrorMessage = "Day is required.")]
        [Range(1, 31, ErrorMessage = "Day should be a number between 1 and 31.")]
        public int? Day { get; set; }

        [Required(ErrorMessage = "Year is required.")]
        public int? Year { get; set; }

        public string RedirectUrl { get; set; }

        public SelectList AvailableMonths { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!DateTime.TryParse($"{Month}/{Day}/{Year}", out DateTime birthdate))
                yield return new ValidationResult("The birth date entered was invalid.", new[] { "InvalidBirthDate" });
            else if (birthdate > DateTime.Now)
                yield return new ValidationResult("The birth date must be a date in the past.", new[] { "FutureBirthDate" });
        }
    }
}