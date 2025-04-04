using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace RealEstates_database
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var serverKapscolat = new MySqlConnectionStringBuilder { Server = "127.0.0.1", Database = "ingatlan2", UserID = "root", Password = "" };
            MySqlConnection kapcsolat = new MySqlConnection(serverKapscolat.ConnectionString);
            kapcsolat.Open();
            var lekerdezes = kapcsolat.CreateCommand();
            Console.Write("Kérem az emeletet: ");
            string emelet = Console.ReadLine();
            lekerdezes.CommandText = $"Select avg(area) from realestates where floors = {emelet}";
            var olvaso = lekerdezes.ExecuteReader();
            while (olvaso.Read())
            {
                Console.WriteLine($"A {emelet} emeleti ingatlanok átlagterülete: {olvaso.GetDouble(0):0.00} m2");

            }
            olvaso.Close();

            lekerdezes.CommandText = "$SELECT name FROM sellers WHERE id in (SELECT DISTINCT sellerid FROM realestates) ORDER BY name DESC;";
            olvaso = lekerdezes.ExecuteReader();
            while (olvaso.Read())
            {
                Console.WriteLine($"{olvaso.GetString(0)}");

            }
            olvaso.Close();

            kapcsolat.Close();



            Console.ReadKey();
        }
    }
}
