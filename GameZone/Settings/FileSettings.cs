namespace GameZone.Settings;
/// <summary>
/// the object of this class is to put all the settings of the uploaded file in one place
/// that will make you avoid the repeat of the same lines in many places while you will need them
/// and also will give you a golbal access to the properites inside it in many places like the cotrollers, models and also in the views.
/// the valus of the properites in this calss are constand which mean this is the only place where you can change the value of them
/// that will help you to avoid the mistakes of change the value over time in many places if they are not in one place like this class
/// </summary>
public static class FileSettings
{
    public const string ImagesPath = "/assets/images/games";
    public const string AllowedExtensions = ".jpg,.jpeg,.png";
    public const int MaxFileSizeInMB = 1;
    public const int MaxFileSizeInBytes = MaxFileSizeInMB * 1024 * 1024;
}