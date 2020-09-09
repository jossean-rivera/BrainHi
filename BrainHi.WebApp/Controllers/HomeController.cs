using Microsoft.AspNetCore.Mvc;

namespace BrainHi.WebApp.Controllers
{
    /// <summary>
    /// Controller for the home page
    /// </summary>
    [Route("/")]
    public class HomeController : Controller
    {
        public ViewResult Index() => View();
    }
}
