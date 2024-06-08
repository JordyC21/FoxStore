using System;
using System.IO;

namespace Foxstore.Logica
{
    public class utilidades
    {
        public static string convertirBase64(string ruta)
        {
            try
            {
                byte[] bytes = File.ReadAllBytes(ruta);
                string file = Convert.ToBase64String(bytes);
                return file;
            }
            catch (FileNotFoundException ex)
            {
                // El archivo no fue encontrado
                Console.WriteLine($"Error: Archivo no encontrado ({ex.Message})");
                return null; // Otra opción es lanzar la excepción hacia arriba
            }
            catch (UnauthorizedAccessException ex)
            {
                // No se tienen los permisos necesarios para acceder al archivo
                Console.WriteLine($"Error: No se tienen los permisos necesarios para acceder al archivo ({ex.Message})");
                return null; // Otra opción es lanzar la excepción hacia arriba
            }
            catch (IOException ex)
            {
                // Error de E/S al leer el archivo
                Console.WriteLine($"Error de E/S al leer el archivo ({ex.Message})");
                return null; // Otra opción es lanzar la excepción hacia arriba
            }
            catch (Exception ex)
            {
                // Otros tipos de errores
                Console.WriteLine($"Error: {ex.Message}");
                return null; // Otra opción es lanzar la excepción hacia arriba
            }
        }
    }
}