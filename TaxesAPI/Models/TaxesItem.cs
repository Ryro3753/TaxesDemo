using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace TaxesAPI.Models
{
    public class TaxesItemDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "You have to specify the Municipality")]

        public string Municipality { get; set; }
        public DateTime Date { get; set; }
        public double TaxesRatio { get; set; } = 0;
        [Required(ErrorMessage = "You have to specify a schedule")]
        [StringRange(AllowableValues = new[] { "daily", "weekly","monthly","yearly" }, ErrorMessage = "Taxes Schedule must be one of 'daily' or 'weekly' or 'monthly' or 'yearly'")]
        public string TaxesSchedule { get; set; }


    }

    public class TaxesItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }

        public string Municipality { get; set; }
        public DateTime Date { get; set; }
        public double TaxesRatio { get; set; } = 0;
        public string TaxesSchedule { get; set; }


    }
    public class StringRangeAttribute : ValidationAttribute
    {
        public string[] AllowableValues { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (AllowableValues?.Contains(value?.ToString()) == true)
            {
                return ValidationResult.Success;
            }

            var msg = $"Please enter one of the allowable values: {string.Join(", ", (AllowableValues ?? new string[] { "No allowable values found" }))}.";
            return new ValidationResult(msg);
        }
    }
}
