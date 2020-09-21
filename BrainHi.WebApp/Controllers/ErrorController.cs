using Microsoft.AspNetCore.Mvc;

namespace BrainHi.WebApp.Controllers
{
    public class ErrorController : Controller
    {
        /// <summary>
        /// Returns <see cref="ViewResult"/> to display message for a not found page
        /// </summary>
        /// <returns>ViewResult for a not found page</returns>
        [Route("not-found")]
        public new ViewResult NotFound() => View();
    }
}
