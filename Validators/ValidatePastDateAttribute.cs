using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;

namespace LearnByDoing.Validators;

public class ValidatePastDateAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is string dateTime)
        {
            var convertedValue = DateTime.ParseExact(dateTime, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            return convertedValue <= DateTime.Now;
        }
    
        return false;
    }
}