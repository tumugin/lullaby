namespace Lullaby.Admin.ViewComponents;

using Microsoft.AspNetCore.Mvc;
using ViewModels;

public class BootstrapPaginator : ViewComponent
{
    public Task<IViewComponentResult> InvokeAsync(int currentPage, int totalPages)
        => Task.FromResult<IViewComponentResult>(
            this.View(new BootstrapPaginatorViewModel { CurrentPage = currentPage, TotalPages = totalPages })
        );
}
