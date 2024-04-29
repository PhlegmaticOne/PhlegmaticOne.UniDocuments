using Microsoft.AspNetCore.Mvc;
using UniDocuments.App.Client.Web.ViewModels;

namespace UniDocuments.App.Client.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Error(string errorMessage)
    {
        return View(new ErrorViewModel { ErrorMessage = errorMessage });
    }
}