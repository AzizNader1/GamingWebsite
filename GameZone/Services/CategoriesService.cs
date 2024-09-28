namespace GameZone.Services;

public class CategoriesService : ICategoriesService
{
    private readonly ApplicationDbContext _context;

    public CategoriesService(ApplicationDbContext context)
    {
        _context = context;
    }
    /// <summary>
    /// this method is related to select the values of the existing Categories and return them in the shape of selectlistitem which will make the operations on them in the view more easy
    /// and also this will make us able to put the Categories in the ViewBag and send it to the related view and make select one category from these Categories
    /// </summary>
    /// <returns></returns>
    public IEnumerable<SelectListItem> GetSelectList()
    {
        return _context.Categories
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .OrderBy(c => c.Text)
                .AsNoTracking()
                .ToList();
    }
}