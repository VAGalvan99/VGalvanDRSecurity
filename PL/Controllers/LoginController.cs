using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(ML.Usuario usuarioForm)
        {
            string txtUsuario = usuarioForm.Usuario1;
            string txtPassword = usuarioForm.Password;
            
            ML.Result resultGetUsuario = BL.Usuario.GetByUsuario(txtUsuario);

            if (resultGetUsuario.Correct)
            {
                ML.Usuario usuario = new ML.Usuario();

                usuario = ((ML.Usuario)resultGetUsuario.Object);

                if (txtPassword == usuario.Password)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Message = "Contraseña incorrecta. Por favor, verifíquela.";
                    return PartialView("Modal");
                }
            } 
            else
            {
                ViewBag.Message = "El usuario " + txtUsuario + " no existe. Por favor, verifíquelo.";
                return PartialView("Modal");
            }
        }
    }
}
