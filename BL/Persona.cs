using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Persona
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.VGalvanDRSecurityContext context = new DL.VGalvanDRSecurityContext())
                {
                    var query = context.Personas.FromSqlRaw($" PersonaGetAll ").ToList();

                    result.Objects = new List<object>();

                    if (query != null)
                    {
                        foreach (var obj in query)
                        {
                            ML.Persona persona = new ML.Persona();
                            persona.IdPersona = obj.IdPersona;
                            persona.Nombre = obj.Nombre;
                            persona.ApellidoPaterno = obj.ApellidoPaterno;
                            persona.ApellidoMaterno = obj.ApellidoMaterno;
                            persona.FechaNacimiento = obj.FechaNacimiento.Value.ToString("dd-MM-yyyy");
                            persona.Sexo = obj.Sexo;

                            persona.Estado = new ML.Estado();
                            persona.Estado.IdEstado = obj.IdEstado.Value;
                            persona.Estado.Nombre = obj.EstadoNombre;

                            persona.CURP = obj.Curp;
                            persona.Imagen = obj.Imagen;

                            result.Objects.Add(persona);
                        }

                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;
        }

        public static ML.Result Add(ML.Persona persona)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.VGalvanDRSecurityContext context = new DL.VGalvanDRSecurityContext())
                {
                    var query = context.Database.ExecuteSqlRaw($" PersonaAdd '{persona.Nombre}', '{persona.ApellidoPaterno}', '{persona.ApellidoMaterno}', '{persona.FechaNacimiento}', '{persona.Sexo}', {persona.Estado.IdEstado}, '{persona.CURP}', '{persona.Imagen}'");

                    if (query > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;
        }

        public static ML.Result GetById(int IdPersona)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.VGalvanDRSecurityContext context = new DL.VGalvanDRSecurityContext())
                {
                    var query = context.Personas.FromSqlRaw($" PersonaGetById {IdPersona} ").AsEnumerable().FirstOrDefault();

                    if (query != null)
                    {
                        ML.Persona persona = new ML.Persona();
                        persona.IdPersona = query.IdPersona;
                        persona.Nombre = query.Nombre;
                        persona.ApellidoPaterno = query.ApellidoPaterno;
                        persona.ApellidoMaterno = query.ApellidoMaterno;
                        persona.FechaNacimiento = query.FechaNacimiento.Value.ToString("dd-MM-yyyy");
                        persona.Sexo = query.Sexo;

                        persona.Estado = new ML.Estado();
                        persona.Estado.IdEstado = query.IdEstado.Value;
                        persona.Estado.Nombre = query.EstadoNombre;

                        persona.CURP = query.Curp;
                        persona.Imagen = query.Imagen;

                        result.Object = persona;
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;
        }

        public static ML.Result Update(ML.Persona persona)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.VGalvanDRSecurityContext context = new DL.VGalvanDRSecurityContext())
                {
                    var query = context.Database.ExecuteSqlRaw($" PersonaUpdate {persona.IdPersona}, '{persona.Nombre}', '{persona.ApellidoPaterno}', '{persona.ApellidoMaterno}', '{persona.FechaNacimiento}', '{persona.Sexo}', {persona.Estado.IdEstado}, '{persona.CURP}', '{persona.Imagen}'");

                    if (query > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;            
        }

        public static ML.Result Delete(int IdPersona)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.VGalvanDRSecurityContext context = new DL.VGalvanDRSecurityContext())
                {
                    var query = context.Database.ExecuteSqlRaw($" PersonaDelete {IdPersona}");

                    if (query > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;            
        }

        public static string ObtenerPrimerLetraPaterno(ML.Persona persona)
        {
            string primerLetra;

            char[] cadenaAPaterno = new char[persona.ApellidoPaterno.Length];
            cadenaAPaterno = persona.ApellidoPaterno.ToCharArray();

            primerLetra = cadenaAPaterno[0].ToString();

            return primerLetra;
        }

        public static string ObtenerPrimerVocalPaterno(ML.Persona persona)
        {
            string primerVocal = "";

            int contadorVocal = 0;

            char[] cadenaAPaterno = new char[persona.ApellidoPaterno.Length];
            cadenaAPaterno = persona.ApellidoPaterno.ToCharArray();

            for (int i = 1; i < persona.ApellidoPaterno.Length; i++)
            {
                if (cadenaAPaterno[i] == 'A' || cadenaAPaterno[i] == 'E' || cadenaAPaterno[i] == 'I' || cadenaAPaterno[i] == 'O' || cadenaAPaterno[i] == 'U')
                {
                    primerVocal = primerVocal + cadenaAPaterno[i];
                    contadorVocal++;
                }

                if (contadorVocal == 1)
                {
                    break;
                }
            }

            return primerVocal;
        }

        public static string ObtenerPrimerLetraMaterno(ML.Persona persona)
        {
            string primerLetra = "";

            char[] cadenaAMaterno = new char[persona.ApellidoMaterno.Length];
            
            if (persona.ApellidoMaterno.Length != 0)
            {
                cadenaAMaterno = persona.ApellidoMaterno.ToCharArray();

                primerLetra += cadenaAMaterno[0];
            }
            else
            {
                primerLetra = primerLetra + "X";
            }

            return primerLetra;
        }

        public static string ObtenerPrimerLetraNombre(ML.Persona persona)
        {
            string primerLetra = "";

            char[] cadenaNombre = new char[persona.Nombre.Length];
            cadenaNombre = persona.Nombre.ToCharArray();

            primerLetra = primerLetra + cadenaNombre[0];

            return primerLetra;
        }

        public static string AjustarFechaNacimiento(ML.Persona persona)
        {
            string numerosFecha = "";
            char[] cadenaFecha = new char[10];
            cadenaFecha = persona.FechaNacimiento.ToCharArray();

            numerosFecha = numerosFecha + cadenaFecha[8] + cadenaFecha[9] + cadenaFecha[3] + cadenaFecha[4] + cadenaFecha[0] + cadenaFecha[1];

            return numerosFecha;
        }

        public static string ObtenerSexo(ML.Persona persona)
        {
            string sexo = "";

            sexo = sexo + persona.Sexo;

            sexo.TrimEnd();

            return sexo;
        }

        public static string ObtenerClaveEstados(ML.Persona persona)
        {
            string claveEstado = "";

            switch (persona.Estado.IdEstado.ToString())
            {
                case "1":
                    claveEstado = claveEstado + "AS"; break;
                case "3":
                    claveEstado = claveEstado + "BC"; break;
                case "21":
                    claveEstado = claveEstado + "BS"; break;
                case "2":
                    claveEstado = claveEstado + "CC"; break;
                case "22":
                    claveEstado = claveEstado + "CS"; break;
                case "23":
                    claveEstado = claveEstado + "CH"; break;
                case "4":
                    claveEstado = claveEstado + "CL"; break;
                case "24":
                    claveEstado = claveEstado + "CM"; break;
                case "25":
                    claveEstado = claveEstado + "DF"; break;
                case "26":
                    claveEstado = claveEstado + "DG"; break;
                case "6":
                    claveEstado = claveEstado + "GT"; break;
                case "27":
                    claveEstado = claveEstado + "GR"; break;
                case "28":
                    claveEstado = claveEstado + "HG"; break;
                case "29":
                    claveEstado = claveEstado + "JC"; break;
                case "10":
                    claveEstado = claveEstado + "MC"; break;
                case "30":
                    claveEstado = claveEstado + "MN"; break;
                case "31":
                    claveEstado = claveEstado + "MS"; break;
                case "32":
                    claveEstado = claveEstado + "NT"; break;
                case "8":
                    claveEstado = claveEstado + "NL"; break;
                case "33":
                    claveEstado = claveEstado + "OC"; break;
                case "34":
                    claveEstado = claveEstado + "PL"; break;
                case "5":
                    claveEstado = claveEstado + "QT"; break;
                case "35":
                    claveEstado = claveEstado + "QR"; break;
                case "36":
                    claveEstado = claveEstado + "SP"; break;
                case "37":
                    claveEstado = claveEstado + "SL"; break;
                case "38":
                    claveEstado = claveEstado + "SR"; break;
                case "39":
                    claveEstado = claveEstado + "TC"; break;
                case "9":
                    claveEstado = claveEstado + "TS"; break;
                case "40":
                    claveEstado = claveEstado + "TL"; break;
                case "41":
                    claveEstado = claveEstado + "VZ"; break;
                case "42":
                    claveEstado = claveEstado + "YN"; break;
                case "7":
                    claveEstado = claveEstado + "ZS"; break;
                default:
                    claveEstado = claveEstado + "NE"; break;
            }

            return claveEstado;
        }

        public static string ObtenerPrimerConsonantePaterno(ML.Persona persona)
        {
            string primerConsonante = "";
            int contadorConsonante = 0;

            char[] cadenaAPaterno = new char[persona.ApellidoPaterno.Length];
            cadenaAPaterno = persona.ApellidoPaterno.ToCharArray();
            
            for (int i = 1; i < persona.ApellidoPaterno.Length; i++)
            {
                if (cadenaAPaterno[i] != 'A' && cadenaAPaterno[i] != 'E' && cadenaAPaterno[i] != 'I' && cadenaAPaterno[i] != 'O' && cadenaAPaterno[i] != 'U')
                {
                    primerConsonante = primerConsonante + cadenaAPaterno[i];
                    contadorConsonante++;
                }

                if (contadorConsonante == 1)
                {
                    contadorConsonante = 0;
                    break;
                }
            }

            return primerConsonante;
        }

        public static string ObtenerPrimerConsonanteMaterno(ML.Persona persona)
        {
            string primerConsonante = "";
            int contadorConsonante = 0;

            char[] cadenaAMaterno = new char[persona.ApellidoMaterno.Length];
            cadenaAMaterno = persona.ApellidoMaterno.ToCharArray();

            for (int i = 1; i < persona.ApellidoMaterno.Length; i++)
            {
                if (cadenaAMaterno[i] != 'A' && cadenaAMaterno[i] != 'E' && cadenaAMaterno[i] != 'I' && cadenaAMaterno[i] != 'O' && cadenaAMaterno[i] != 'U')
                {
                    primerConsonante = primerConsonante + cadenaAMaterno[i];
                    contadorConsonante++;
                }

                if (contadorConsonante == 1)
                {
                    contadorConsonante = 0;
                    break;
                }
            }

            return primerConsonante;
        }

        public static string ObtenerPrimerConsonanteNombre(ML.Persona persona)
        {
            string primerConsonante = "";
            int contadorConsonante = 0;

            char[] cadenaNombre = new char[persona.Nombre.Length];
            cadenaNombre = persona.Nombre.ToCharArray();

            for (int i = 1; i < persona.Nombre.Length; i++)
            {
                if (cadenaNombre[i] != 'A' && cadenaNombre[i] != 'E' && cadenaNombre[i] != 'I' && cadenaNombre[i] != 'O' && cadenaNombre[i] != 'U')
                {
                    primerConsonante = primerConsonante + cadenaNombre[i];
                    contadorConsonante++;
                }

                if (contadorConsonante == 1)
                {
                    contadorConsonante = 0;
                    break;
                }
            }

            return primerConsonante;
        }

        public static string ObtenerUltimasDosClaves()
        {
            string claves = "";

            Random aleatorio = new Random();
            int clave = aleatorio.Next(10, 99);

            claves = claves + clave;

            return claves;
        }
    }
}
