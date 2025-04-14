using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace mozaik
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var serverKapscolat = new MySqlConnectionStringBuilder { Server = "127.0.0.1", Database = "", UserID = "root", Password = "" };
            MySqlConnection kapcsolat = new MySqlConnection(serverKapscolat.ConnectionString);
            kapcsolat.Open();
            var lekerdezes = kapcsolat.CreateCommand();
            lekerdezes.CommandText = $"Drop database if exists mozaik";
            var olvaso = lekerdezes.ExecuteReader();
            olvaso.Close();

            lekerdezes.CommandText = $"CREATE DATABASE if not exists mozaik CHARACTER SET utf8 COLLATE utf8_hungarian_ci;";
            olvaso = lekerdezes.ExecuteReader();
            olvaso.Close();

            kapcsolat.ChangeDatabase("mozaik");

            List<string> commands = new List<string>();

            try
            {
                using (StreamReader reader = new StreamReader("adatbazis.sql", Encoding.UTF8))
                {
                    commands.Add(reader.ReadToEnd());
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }

            for (int i = 0; i < commands.Count; i++)
            {
                lekerdezes.CommandText = $"{commands[i]}";
            }

            olvaso = lekerdezes.ExecuteReader();
            olvaso.Close();

            Console.Write("3.feladat: ");
            lekerdezes.CommandText = "SELECT COUNT(*)\r\nFROM rendezveny\r\nWHERE rendezveny.letszam >= 100;";
            olvaso = lekerdezes.ExecuteReader();
            while (olvaso.Read())
            {
                Console.WriteLine($"{olvaso.GetInt32(0)} rendezvényt szerveztek legalább 100 fő létszámmal!");
            }
            olvaso.Close();

            Console.Write("4.feladat: ");
            lekerdezes.CommandText = "SELECT rendezveny.idopont, rendezveny.napokszama, rendezveny.letszam\r\nFROM rendezveny INNER JOIN helyszin ON rendezveny.kapcsolatId = helyszin.id\r\nWHERE helyszin.nev = \"Szeged\";";
            olvaso = lekerdezes.ExecuteReader();
            while (olvaso.Read())
            {
                Console.WriteLine($"{olvaso.GetDateTime(0).ToString("yyyy. MMMM d.")} kezdőnappal, {olvaso.GetInt32(1)} napos, {olvaso.GetInt32(2)} fős rendezvény(eket) szerveztek Szegeden.");
            }
            olvaso.Close();

            Console.Write("5.feladat: TABLE UPDATED! ");
            lekerdezes.CommandText = "UPDATE kapcsolat\r\nSET cegnev = 'BugFix IT'\r\nWHERE nev = \"Nagy Béla\";";
            olvaso = lekerdezes.ExecuteReader();
            olvaso.Close();

            Console.Write("\n6.feladat: ");
            lekerdezes.CommandText = "SELECT tipus.nev\r\nFROM tipus INNER JOIN rendezveny ON tipus.id = rendezveny.tipusId\r\nGROUP BY tipus.nev\r\nORDER BY AVG(rendezveny.letszam) DESC LIMIT 2;";
            olvaso = lekerdezes.ExecuteReader();
            while (olvaso.Read())
            {
                Console.Write($"|{olvaso.GetString(0)}| ");
            }
            olvaso.Close();

            /*Console.Write("7.feladat: ");
            lekerdezes.CommandText = "";
            olvaso = lekerdezes.ExecuteReader();
            while (olvaso.Read())
            {
                Console.WriteLine($"{olvaso.GetString(0)}");
            }
            olvaso.Close();

            Console.Write("8.feladat: ");
            lekerdezes.CommandText = "";
            olvaso = lekerdezes.ExecuteReader();
            while (olvaso.Read())
            {
                Console.WriteLine($"");
            }
            olvaso.Close();*/

            Console.ReadKey();
        }
    }
}
