using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class PersonaController : Controller
    {
        public ActionResult GetAll()
        {
            ML.Result result = BL.Persona.GetAll();            

            if (result.Correct)
            {
                ML.Persona persona = new ML.Persona();

                persona.Personas = result.Objects;
                
                return View(persona);
            }
            else
            {
                return View();
            }            
        }

        [HttpGet]
        public ActionResult Form(int? IdPersona)
        {
            ML.Result resultEstados = BL.Estado.GetAll();

            if (IdPersona == null)
            {
                ML.Persona persona = new ML.Persona();                

                persona.Estado = new ML.Estado();
                persona.Estado.Estados = resultEstados.Objects;

                return View(persona);
            }
            else
            {
                ML.Result resultById = BL.Persona.GetById(IdPersona.Value);

                if (resultById.Correct)
                {
                    ML.Persona persona = new ML.Persona();

                    persona = ((ML.Persona)resultById.Object);
                    persona.Estado.Estados = resultEstados.Objects;

                    return View(persona);
                }
                else
                {
                    ViewBag.Message = "No se pudo localizar a la persona. Razón: "+resultById.ErrorMessage;
                    return PartialView("Modal");
                }
                
            }
        }

        [HttpPost]
        public ActionResult Form(ML.Persona persona)
        {
            string curp;

            IFormFile file = Request.Form.Files["IFImagen"];

            if (file != null)
            {
                byte[] ImagenBytes = ConvertToBytes(file);
                persona.Imagen = Convert.ToBase64String(ImagenBytes);
            }

            if (persona.IdPersona == 0)
            {
                curp = GenerarCurp(persona);

                persona.CURP = curp;

                ML.Result resultAdd = BL.Persona.Add(persona);

                if (resultAdd.Correct)
                {
                    ViewBag.Message = "Persona registrada correctamente.";
                    return PartialView("Modal");
                }
                else
                {
                    ViewBag.Message = "No se pudo registrar la persona. Razón: "+resultAdd.ErrorMessage;
                    return PartialView("Modal");
                }

            }
            else
            {
                if (persona.CURP == null)
                {
                    curp = GenerarCurp(persona);

                    persona.CURP = curp;
                }

                ML.Result resultUpdate = BL.Persona.Update(persona);

                if (resultUpdate.Correct)
                {
                    ViewBag.Message = "Persona actualizada correctamente.";
                    return PartialView("Modal");
                }
                else
                {
                    ViewBag.Message = "No se pudo actualizar la persona. Razón: " + resultUpdate.ErrorMessage;
                    return PartialView("Modal");
                }
            }
        }
        
        public ActionResult Delete(int IdPersona)
        {
            ML.Result resultDelete = BL.Persona.Delete(IdPersona);

            if (resultDelete.Correct)
            {
                ViewBag.Message = "Persona eliminada correctamente.";
                return PartialView("Modal");
            }
            else
            {
                ViewBag.Message = "No se pudo eliminar la persona. Razón: " + resultDelete.ErrorMessage;
                return PartialView("Modal");
            }
        }

        public byte[] ConvertToBytes(IFormFile imagen)
        {
            using var fileStream = imagen.OpenReadStream();

            byte[] bytes = new byte[fileStream.Length];

            fileStream.Read(bytes, 0, bytes.Length);

            return bytes;
        }

        public string GenerarCurp(ML.Persona persona)
        {
            string curp = "";

            curp = BL.Persona.ObtenerPrimerLetraPaterno(persona);
            curp += BL.Persona.ObtenerPrimerVocalPaterno(persona);
            curp += BL.Persona.ObtenerPrimerLetraMaterno(persona);
            curp += BL.Persona.ObtenerPrimerLetraNombre(persona);
            curp += BL.Persona.AjustarFechaNacimiento(persona);
            curp += BL.Persona.ObtenerSexo(persona);
            curp += BL.Persona.ObtenerClaveEstados(persona);
            curp += BL.Persona.ObtenerPrimerConsonantePaterno(persona);
            curp += BL.Persona.ObtenerPrimerConsonanteMaterno(persona);
            curp += BL.Persona.ObtenerPrimerConsonanteNombre(persona);
            curp += BL.Persona.ObtenerUltimasDosClaves();

            return curp;
        }
        //public string GenerarCurp(ML.Persona persona)
        //{
        //    string curp = "";
        //    int contadorVocal = 0;
        //    int contadorConsonante = 0;

        //    /* PRIMER LETRA Apellido Paterno */

        //    char[] cadenaAPaterno = new char[persona.ApellidoPaterno.Length];
        //    cadenaAPaterno = persona.ApellidoPaterno.ToCharArray();

        //    curp = cadenaAPaterno[0].ToString();


        //    /* PRIMER VOCAL Apellido Paterno */
        //    for (int i = 1; i < persona.ApellidoPaterno.Length; i++)
        //    {
        //        if (cadenaAPaterno[i] == 'A' || cadenaAPaterno[i] == 'E' || cadenaAPaterno[i] == 'I' || cadenaAPaterno[i] == 'O' || cadenaAPaterno[i] == 'U')
        //        {
        //            curp = curp + cadenaAPaterno[i];
        //            contadorVocal++;
        //        }

        //        if (contadorVocal == 1)
        //        {
        //            break;
        //        }
        //    }

        //    /* PRIMER LETRA Apellido Materno */
        //    char[] cadenaAMaterno = new char[persona.ApellidoMaterno.Length];

        //    if (persona.ApellidoMaterno.Length != 0)
        //    {
        //        cadenaAMaterno = persona.ApellidoMaterno.ToCharArray();

        //        curp = curp + cadenaAMaterno[0];
        //    }
        //    else 
        //    {
        //        curp = curp + "X";
        //    }

        //    /* PRIMER LETRA Nombre */
        //    char[] cadenaNombre = new char[persona.Nombre.Length];
        //    cadenaNombre = persona.Nombre.ToCharArray();

        //    curp = curp + cadenaNombre[0];

        //    /* FECHA DE NACIMIENTO AAMMDD */
        //    char[] cadenaFecha = new char[10];
        //    cadenaFecha = persona.FechaNacimiento.ToCharArray();

        //    curp = curp + cadenaFecha[8] + cadenaFecha[9] + cadenaFecha[3] + cadenaFecha[4] + cadenaFecha[0] + cadenaFecha[1];

        //    /* SEXO */
        //    curp = curp + persona.Sexo;

        //    /* CLAVE DEL ESTADO */
        //    switch (persona.Estado.IdEstado.ToString())
        //    {
        //        case "1":
        //            curp = curp + "AS"; break;
        //        case "3":
        //            curp = curp + "BC"; break;
        //        case "21":
        //            curp = curp + "BS"; break;
        //        case "2":
        //            curp = curp + "CC"; break;
        //        case "22":
        //            curp = curp + "CS"; break;
        //        case "23":
        //            curp = curp + "CH"; break;
        //        case "4":
        //            curp = curp + "CL"; break;
        //        case "24":
        //            curp = curp + "CM"; break;
        //        case "25":
        //            curp = curp + "DF"; break;
        //        case "26":
        //            curp = curp + "DG"; break;
        //        case "6":
        //            curp = curp + "GT"; break;
        //        case "27":
        //            curp = curp + "GR"; break;
        //        case "28":
        //            curp = curp + "HG"; break;
        //        case "29":
        //            curp = curp + "JC"; break;
        //        case "10":
        //            curp = curp + "MC"; break;
        //        case "30":
        //            curp = curp + "MN"; break;
        //        case "31":
        //            curp = curp + "MS"; break;
        //        case "32":
        //            curp = curp + "NT"; break;
        //        case "8":
        //            curp = curp + "NL"; break;
        //        case "33":
        //            curp = curp + "OC"; break;
        //        case "34":
        //            curp = curp + "PL"; break;
        //        case "5":
        //            curp = curp + "QT"; break;
        //        case "35":
        //            curp = curp + "QR"; break;
        //        case "36":
        //            curp = curp + "SP"; break;
        //        case "37":
        //            curp = curp + "SL"; break;
        //        case "38":
        //            curp = curp + "SR"; break;
        //        case "39":
        //            curp = curp + "TC"; break;
        //        case "9":
        //            curp = curp + "TS"; break;
        //        case "40":
        //            curp = curp + "TL"; break;
        //        case "41":
        //            curp = curp + "VZ"; break;
        //        case "42":
        //            curp = curp + "YN"; break;
        //        case "7":
        //            curp = curp + "ZS"; break;
        //        default:
        //            curp = curp + "NE"; break;
        //    }

        //    /* PRIMER CONSONANTE Apellido Paterno */
        //    for (int i = 1; i < persona.ApellidoPaterno.Length; i++)
        //    {
        //        if (cadenaAPaterno[i] != 'A' && cadenaAPaterno[i] != 'E' && cadenaAPaterno[i] != 'I' && cadenaAPaterno[i] != 'O' && cadenaAPaterno[i] != 'U')
        //        {
        //            curp = curp + cadenaAPaterno[i];
        //            contadorConsonante++;
        //        }

        //        if (contadorConsonante == 1)
        //        {
        //            contadorConsonante = 0;
        //            break;
        //        }
        //    }

        //    /* PRIMER CONSONANTE Apellido Materno */
        //    for (int i = 1; i < persona.ApellidoMaterno.Length; i++)
        //    {
        //        if (cadenaAMaterno[i] != 'A' && cadenaAMaterno[i] != 'E' && cadenaAMaterno[i] != 'I' && cadenaAMaterno[i] != 'O' && cadenaAMaterno[i] != 'U')
        //        {
        //            curp = curp + cadenaAMaterno[i];
        //            contadorConsonante++;
        //        }

        //        if (contadorConsonante == 1)
        //        {
        //            contadorConsonante = 0;
        //            break;
        //        }
        //    }

        //    /* PRIMER CONSONANTE Nombre */
        //    for (int i = 1; i < persona.Nombre.Length; i++)
        //    {
        //        if (cadenaNombre[i] != 'A' && cadenaNombre[i] != 'E' && cadenaNombre[i] != 'I' && cadenaNombre[i] != 'O' && cadenaNombre[i] != 'U')
        //        {
        //            curp = curp + cadenaNombre[i];
        //            contadorConsonante++;
        //        }

        //        if (contadorConsonante == 1)
        //        {
        //            contadorConsonante = 0;
        //            break;
        //        }
        //    }

        //    /* ÚLTIMOS DOS DÍGITOS CLAVE */
        //    Random aleatorio = new Random();
        //    int clave = aleatorio.Next(01, 99);

        //    curp = curp + clave;

        //    curp = curp.ToUpper();

        //    return curp;
        //}
    }
}
