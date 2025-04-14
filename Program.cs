using System;
using MySql.Data.MySqlClient;

class Program
{
    static string connectionString = "server=127.0.0.1;user=root;database=genshin;password=";

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\nVálassz egy műveletet:");
            Console.WriteLine("1. Karakter hozzáadása");
            Console.WriteLine("2. Összes karakter kilistázása");
            Console.WriteLine("3. Vision szerinti szűrés");
            Console.WriteLine("4. Kilépés");
            Console.Write("Választás: ");

            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    InsertCharacter();
                    break;
                case "2":
                    GetCharacters();
                    break;
                case "3":
                    FilterByVision();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Érvénytelen választás.");
                    break;
            }
        }
    }

    static void InsertCharacter()
    {
        Console.Write("Karakter neve: ");
        string name = Console.ReadLine();

        Console.WriteLine("\n--- Bodytype lehetőségek ---");
        ListBodyTypes();

        Console.Write("Bodytype ID: ");
        int bodytypeId = int.Parse(Console.ReadLine());

        Console.Write("Constellation: ");
        string constellation = Console.ReadLine();

        Console.Write("Születési hónap (1-12): ");
        int birthMonth = int.Parse(Console.ReadLine());

        Console.Write("Születési nap (1-31): ");
        int birthDay = int.Parse(Console.ReadLine());

        Console.WriteLine("\n--- Vision lehetőségek ---");
        ListVisions();

        Console.Write("Vision ID: ");
        int visionId = int.Parse(Console.ReadLine());

        Console.WriteLine("\n--- Weapon lehetőségek ---");
        ListWeapons();

        Console.Write("Weapon ID: ");
        int weaponId = int.Parse(Console.ReadLine());

        Console.WriteLine("\n--- Nation lehetőségek ---");
        ListNations();

        Console.Write("Nation ID: ");
        int nationId = int.Parse(Console.ReadLine());

        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = @"INSERT INTO characters 
            (name, body_type_id, constellation, birth_month, birth_day, vision_id, weapon_id, nation_id)
            VALUES 
            (@name, @bodytypeId, @constellation, @birthMonth, @birthDay, @visionId, @weaponId, @nationId)";

            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@bodytypeId", bodytypeId);
                cmd.Parameters.AddWithValue("@constellation", constellation);
                cmd.Parameters.AddWithValue("@birthMonth", birthMonth);
                cmd.Parameters.AddWithValue("@birthDay", birthDay);
                cmd.Parameters.AddWithValue("@visionId", visionId);
                cmd.Parameters.AddWithValue("@weaponId", weaponId);
                cmd.Parameters.AddWithValue("@nationId", nationId);

                cmd.ExecuteNonQuery();
                Console.WriteLine("Karakter sikeresen hozzáadva.");
            }
        }
    }

    static void GetCharacters()
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = @"
                SELECT c.id, c.name, CONCAT(b.gender, '/', b.height) AS bodytype, 
                       n.place AS nation_name, v.element AS vision_element
                FROM characters c
                JOIN bodytypes b ON c.body_type_id = b.id
                JOIN nations n ON c.nation_id = n.id
                JOIN visions v ON c.vision_id = v.id";

            using (var cmd = new MySqlCommand(query, connection))
            using (var reader = cmd.ExecuteReader())
            {
                Console.WriteLine("\nID  | Név                  | Testtípus      | Nemzet     | Vision");
                Console.WriteLine("----+----------------------+----------------+------------+-----------");
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["id"],-3} | {reader["name"],-20} | {reader["bodytype"],-14} | {reader["nation_name"],-10} | {reader["vision_element"],-10}");
                }
            }
        }
    }

    static void FilterByVision()
    {
        Console.WriteLine("\n--- Vision lehetőségek ---");
        ListVisions();

        Console.Write("\nAdd meg a vision nevét (pl. Pyro): ");
        string visionName = Console.ReadLine();

        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = @"
                SELECT c.name, v.element 
                FROM characters c
                JOIN visions v ON c.vision_id = v.id
                WHERE LOWER(v.element) = LOWER(@vision)";

            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@vision", visionName);
                using (var reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("\nKarakterek a megadott vision szerint:");
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["name"]} ({reader["element"]})");
                    }
                }
            }
        }
    }

    static void ListBodyTypes()
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT id, gender, height FROM bodytypes";
            using (var cmd = new MySqlCommand(query, connection))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string gender = (reader.GetByte("gender") == 0) ? "Férfi" : "Nő";
                    Console.WriteLine($"{reader["id"]}: {gender}, {reader["height"]}");
                }
            }
        }
    }

    static void ListWeapons()
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT id, type FROM weapons";
            using (var cmd = new MySqlCommand(query, connection))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["id"]}: {reader["type"]}");
                }
            }
        }
    }

    static void ListNations()
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT id, place FROM nations";
            using (var cmd = new MySqlCommand(query, connection))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["id"]}: {reader["place"]}");
                }
            }
        }
    }

    static void ListVisions()
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT id, element FROM visions";
            using (var cmd = new MySqlCommand(query, connection))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["id"]}: {reader["element"]}");
                }
            }
        }
    }
}
