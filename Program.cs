using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace sqlconnect
{
    internal class Program
    {
        static void AddDiak(MySqlConnection cutasi, string nev, int szulev)
        {
            string insertString = "INSERT INTO diak(nev, szulev) VALUES(@nev, @szulev)";
            MySqlCommand command = new MySqlCommand(insertString, cutasi);
            command.Parameters.AddWithValue("@nev", nev);
            command.Parameters.AddWithValue("@szulev", szulev);
            int sorok = command.ExecuteNonQuery();
            Console.WriteLine($"{sorok} sor érintett!");
        }

        static List<(string, int)> fetchDiakok(MySqlConnection cutasi)
        {
            string fetchString = "SELECT * FROM diak";
            MySqlCommand command = new MySqlCommand(fetchString, cutasi);
            MySqlDataReader reader = command.ExecuteReader();
            List<(string, int)> diakok = new List<(string, int)>();
            while (reader.Read())
            {
                string nev = reader.GetString("nev");
                int szulev = reader.GetInt32("szulev");
                diakok.Add((nev, szulev));
            }
            return diakok;
        }

        static void Main(string[] args)
        {
            string connectionString = "server=localhost;database=teszt;username=root;password=mysql";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    Console.WriteLine("Sikeres kapcsolódás!");
                    AddDiak(conn, "Utasi Zalán Zoltán", 2008);
                    AddDiak(conn, "Tóth Kornél", 2007);
                    AddDiak(conn, "PZA-GND", 1812);

                    List<(string Nev, int Szulev)> diakok = fetchDiakok(conn);
                    foreach (var d in diakok)
                    {
                        Console.WriteLine($"Diák neve: {d.Nev}");
                        Console.WriteLine($"Diák születési éve: {d.Szulev}");
                    }
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }
            }

            Console.ReadKey();
        }
    }
}
