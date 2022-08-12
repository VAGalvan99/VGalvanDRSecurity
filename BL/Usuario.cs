using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Usuario
    {
        public static ML.Result GetByUsuario(string user)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.VGalvanDRSecurityContext context = new DL.VGalvanDRSecurityContext())
                {
                    var query = context.Usuarios.FromSqlRaw($" UsuarioGetByUsuario '{user}'").AsEnumerable().FirstOrDefault();

                    if (query != null)
                    {
                        ML.Usuario usuarioGet = new ML.Usuario();

                        usuarioGet.IdUsuario = query.IdUsuario;
                        usuarioGet.Usuario1 = query.Usuario1;
                        usuarioGet.Password = query.Password;

                        result.Object = usuarioGet;

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
    }
}
