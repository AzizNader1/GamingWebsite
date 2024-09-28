namespace GameZone.Services;

public class DevicesService : IDevicesService
{
    private readonly ApplicationDbContext _context;

    public DevicesService(ApplicationDbContext context)
    {
        _context = context;
    }
    /// <summary>
    /// this method is related to select the values of the existing Devices and return them in the shape of selectlistitem which will make the operations on them in the view more easy
    /// and also this will make us able to put the Devices in the ViewBag and send it to the related view and make select one device from these Devices
    /// </summary>
    /// <returns></returns
    public IEnumerable<SelectListItem> GetSelectList()
    {
        return _context.Devices
                .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Name })
                .OrderBy(d => d.Text)
                .AsNoTracking()
                .ToList();
    }
}