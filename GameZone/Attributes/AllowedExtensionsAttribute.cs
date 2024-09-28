namespace GameZone.Attributes;
/// <summary>
/// this class has one goal is to check if the extention of the uploaded file is accepted or not 
/// and will do this by making an inherit from the ValidationAttribute class and override the IsValid method which exist in that class
/// and will reutrn success if the extention is accepted and error message if it is not accepted
/// </summary>
public class AllowedExtensionsAttribute : ValidationAttribute
{
    private readonly string _allowedExtensions;

    public AllowedExtensionsAttribute(string allowedExtensions)
    {
        _allowedExtensions = allowedExtensions;
    }

    protected override ValidationResult? IsValid
        (object? value, ValidationContext validationContext)
    {
        var file = value as IFormFile;
        
        if(file is not null)
        {
            var extension = Path.GetExtension(file.FileName);

            var isAllowed = _allowedExtensions.Split(',').Contains(extension, StringComparer.OrdinalIgnoreCase);

            if (!isAllowed)
            {
                return new ValidationResult($"Only {_allowedExtensions} are allowed!");
            }
        }

        return ValidationResult.Success;
    }
}