namespace GameZone.Attributes;

/// <summary>
/// this class will take the file that i need to check about it's lenght 
/// and will return success if the lenght of the sended file is accepted
/// and will return an error message if the length more that the accepted lenght 
/// and this class will do this by inheritance from the ValidationAttribute class and override the IsValid method and apply our own concept in this functoin
/// </summary>
public class MaxFileSizeAttribute : ValidationAttribute
{
    private readonly int _maxFileSize;

    public MaxFileSizeAttribute(int maxFileSize)
    {
        _maxFileSize = maxFileSize;
    }

    protected override ValidationResult? IsValid
        (object? value, ValidationContext validationContext)
    {
        var file = value as IFormFile;

        if (file is not null)
        {
            if (file.Length > _maxFileSize)
            {
                return new ValidationResult($"Maximum allowed size is {_maxFileSize} bytes");
            }
        }

        return ValidationResult.Success;
    }
}