using Microsoft.AspNetCore.Mvc;

namespace CourtReservation_Api.Controllers
{
    public class UsuariosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
