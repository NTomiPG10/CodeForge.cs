using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace cukraszda
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var serverKapscolat = new MySqlConnectionStringBuilder { Server = "127.0.0.1", Database = "cukraszda", UserID = "root", Password = "" };
            MySqlConnection kapcsolat = new MySqlConnection(serverKapscolat.ConnectionString);
            kapcsolat.Open();
            var lekerdezes = kapcsolat.CreateCommand();
            lekerdezes.CommandText = $"SELECT COUNT(*) AS \"Hiányzó kalória érték\" FROM termek WHERE termek.kaloria IS NULL;";
            var olvaso = lekerdezes.ExecuteReader();
            while(olvaso.Read())
            {
                Console.WriteLine($"A hiányzó kalória érték: {olvaso.GetUInt16(0)}");

            }
            olvaso.Close();
            Console.WriteLine("\n");
            lekerdezes.CommandText = "SELECT termek.nev, kiszereles.mennyiseg\r\nFROM termek INNER JOIN kiszereles ON termek.kiszerelesId = kiszereles.id\r\nWHERE kiszereles.mennyiseg LIKE \"%g\";";
            olvaso = lekerdezes.ExecuteReader();
            while (olvaso.Read())
            {
                Console.WriteLine($"{olvaso.GetString(0)}\t{olvaso.GetString(1)}");

            }
            olvaso.Close();

            Console.WriteLine("\n");
            lekerdezes.CommandText = "SELECT allergen.nev, COUNT(*) AS \"termék szám\"\r\nFROM termek INNER JOIN allergeninfo ON termek.id = allergeninfo.termekId INNER JOIN allergen ON allergeninfo.allergenId = allergen.id\r\nGROUP BY allergen.nev DESC\r\nORDER BY COUNT(*) DESC LIMIT 3;";
            olvaso = lekerdezes.ExecuteReader();
            
            while (olvaso.Read())
            {
                Console.WriteLine($"{olvaso.GetString(0)}\t{olvaso.GetInt32(1)}");

            }
            olvaso.Close();

            Console.WriteLine("\n");
            lekerdezes.CommandText = "SELECT termek.nev, termek.ar FROM termek WHERE termek.laktozmentes = 1 and termek.tejmentes = 1 AND termek.tojasmentes = 1 and termek.id not in (select allergeninfo.termekId from allergeninfo);";
            olvaso = lekerdezes.ExecuteReader();

            while (olvaso.Read())
            {
                Console.WriteLine($"{olvaso.GetString(0)}\t{olvaso.GetInt32(1)}");

            }
            olvaso.Close();

            lekerdezes.CommandText = "SELECT termek.nev, (termek.ar-100)*12\r\nfrom termek\r\nWHERE termek.nev like \"Paleo%\";";
            olvaso = lekerdezes.ExecuteReader();

            while (olvaso.Read())
            {
                Console.WriteLine($"{olvaso.GetString(0)}\t{olvaso.GetInt32(1)}");

            }
            olvaso.Close();

            kapcsolat.Close();
            Console.ReadKey();
        }
    }
}
