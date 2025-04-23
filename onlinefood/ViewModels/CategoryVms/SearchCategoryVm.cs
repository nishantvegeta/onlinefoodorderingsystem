using System;

namespace onlinefood.ViewModels.CategoryVms;

public class SearchCategoryVm
{
    public int CategoryId { get; set; }

    public string Name { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; }
}
