using System;
using SQLite;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Rezervační_systém
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new SQLiteConnection("./data.db");
            db.CreateTable<Mistnosti>();
            db.CreateTable<Mistnosti_obsazenost>();

            if (db.Table<Mistnosti>().Count() == 0)
            {
                Console.WriteLine("");
                Console.WriteLine("V této databízi se nenacházejí žádné místnosti!");
                Console.WriteLine("");
                Console.WriteLine("Stisnitím entru přejdete k vytvoření...");
                Console.ReadLine();
                Console.Clear();
                string show_error = "";

                string name = "";
                while (name == "")
                {
                    Console.WriteLine("------------------------------------");
                    Console.WriteLine("Vytváření místnosti | Krok 1/3");
                    Console.WriteLine("------------------------------------");
                    Console.WriteLine("");
                    Console.WriteLine("Zadejte název místnosti:");
                    name = Console.ReadLine();
                    Console.Clear();
                }
                string rady_S = "";
                int rady = 0;
                while (true)
                {
                    do
                    {
                        Console.WriteLine("------------------------------------");
                        Console.WriteLine("Vytváření místnosti | Krok 2/3");
                        Console.WriteLine("------------------------------------");
                        if (show_error != "")
                        {
                            Console.WriteLine("");
                            Console.WriteLine("Chyba! Můžete zadat pouze validní číslo!");
                        }
                        Console.WriteLine("");
                        Console.WriteLine("Zadejte počet řad:");
                        rady_S = Console.ReadLine();
                        Console.Clear();
                    } while (!int.TryParse(rady_S, out rady));
                    var pattern = new Regex(@"^[1-9]\d*$");
                    if (pattern.Match(rady.ToString()).Success)
                    {
                        show_error = "";
                        break;
                    }
                    else
                    {
                        show_error = "only_nums";
                    }
                }

                string sloupce_S = "";
                int sloupce = 0;
                while (true)
                {
                    do
                    {
                        Console.WriteLine("------------------------------------");
                        Console.WriteLine("Vytváření místnosti | Krok 2/3");
                        Console.WriteLine("------------------------------------");
                        if (show_error != "")
                        {
                            Console.WriteLine("");
                            Console.WriteLine("Chyba! Můžete zadat pouze validní číslo!");
                        }
                        Console.WriteLine("");
                        Console.WriteLine("Zadejte počet řad:");
                        sloupce_S = Console.ReadLine();
                        Console.Clear();
                    } while (!int.TryParse(sloupce_S, out sloupce));
                    var pattern = new Regex(@"^[1-9]\d*$");
                    if (pattern.Match(sloupce.ToString()).Success)
                    {
                        show_error = "";
                        break;
                    }
                    else
                    {
                        show_error = "only_nums";
                    }
                }
                var _Mistnost = new Mistnosti();
                _Mistnost.nazev = name;
                _Mistnost.rady = rady;
                _Mistnost.sloupce = sloupce;
                db.Insert(_Mistnost);
            }
            while (true)
            {
                var mistnosti = db.Query<Mistnosti>("SELECT * FROM Mistnosti");

                Console.WriteLine("------------------------------------");
                Console.WriteLine("Rezervační systém");
                Console.WriteLine("------------------------------------");
                Console.WriteLine("");
                Console.WriteLine("Aktuálně máte uloženo " + db.Table<Mistnosti>().Count() + " místností.");
                Console.WriteLine("");
                Console.WriteLine("Seznam místností:");

                foreach (var mistnost in mistnosti)
                {
                    Console.WriteLine(mistnost.nazev);
                }
                Console.WriteLine("");
                Console.WriteLine("Nabídka akcí:");
                Console.WriteLine("1 - Úprava v místnosti");
                Console.WriteLine("2 - Vytvoření místnosti");
                Console.WriteLine("3 - Smazání místnosti");
                Console.WriteLine("");
                Console.WriteLine("Zvolte akci:");
                string akce = Console.ReadLine();
                Console.Clear();

                switch (akce)
                {
                    case "1":
                        if (!mistnosti.Any())
                        {
                            break;
                        }
                        string error1 = "";
                        int exit_c = 0;
                        while (true)
                        {
                            if (exit_c == 1)
                            {
                                break;
                            }
                            Console.WriteLine("------------------------------------");
                            Console.WriteLine("Úprava v místnosti");
                            Console.WriteLine("------------------------------------");
                            if (error1 != "")
                            {
                                Console.WriteLine("");
                                Console.WriteLine("Chyba! Místnost neexistuje!");
                            }
                            Console.WriteLine("");
                            Console.WriteLine("Seznam místností:");

                            foreach (var mistnost in mistnosti)
                            {
                                Console.WriteLine(mistnost.nazev);
                            }
                            Console.WriteLine("");
                            Console.WriteLine("Napište název místnosti:");
                            string nazev_mistnosti_del = Console.ReadLine();
                            Console.Clear();
                            int matches = 0;
                            foreach (var mistnost in mistnosti)
                            {
                                if (mistnost.nazev == nazev_mistnosti_del)
                                {
                                    matches++;
                                }
                            }
                            if (matches != 0)
                            {
                                while (true)
                                {
                                    Console.WriteLine("------------------------------------");
                                    Console.WriteLine("Úprava v místnosti");
                                    Console.WriteLine("------------------------------------");
                                    Console.WriteLine("");
                                    Console.WriteLine("Aktuální obsazenost:");
                                    var mistonst_uprava = db.Query<Mistnosti>("SELECT * FROM Mistnosti WHERE nazev = ?", nazev_mistnosti_del);
                                    int sloupce_uprava = 0;
                                    int rady_uprava = 0;
                                    int id_uprava = 0;
                                    foreach (var mistnost in mistonst_uprava)
                                    {
                                        sloupce_uprava = mistnost.sloupce;
                                        rady_uprava = mistnost.rady;
                                        id_uprava = mistnost.id;

                                    }
                                    var mistonst_info = db.Query<Mistnosti_obsazenost>("SELECT * FROM Mistnosti_obsazenost WHERE mistnost_id = ?", id_uprava);

                                    string res = "";
                                    for (int i = 1; i <= rady_uprava; i++)
                                    {
                                        res = "";
                                        for (int z = 1; z <= sloupce_uprava; z++)
                                        {
                                            int volno = 1;
                                            foreach (var misto in mistonst_info)
                                            {
                                                if (misto.rada == i && misto.sloupec == z)
                                                {
                                                    volno = 0;
                                                }
                                            }
                                            if (volno == 0)
                                            {
                                                res = res + "x ";
                                            }
                                            else
                                            {
                                                res = res + "o ";
                                            }
                                        }
                                        Console.WriteLine(res);
                                    }
                                    Console.WriteLine("");
                                    Console.WriteLine("Nabídka akcí:");
                                    Console.WriteLine("1 - Nová rezervace");
                                    Console.WriteLine("2 - Odstranit rezervaci");
                                    Console.WriteLine("3 - Odejít");
                                    Console.WriteLine("");
                                    Console.WriteLine("Zvolte akci:");
                                    string akce_uprava = Console.ReadLine();
                                    Console.Clear();
                                    if (akce_uprava == "1")
                                    {
                                        string vytvareni_r = "";
                                        int vytvareni_rady = 0;
                                        string vytvareni_s = "";
                                        int vytvareni_sloupce = 0;
                                        string vytvareni_error = "";
                                        string obsazeno_error = "";
                                        while (true)
                                        {
                                            while (true)
                                            {
                                                do
                                                {
                                                    Console.WriteLine("------------------------------------");
                                                    Console.WriteLine("Vytváření rezervace | Krok 1/4");
                                                    Console.WriteLine("------------------------------------");
                                                    if (vytvareni_error != "")
                                                    {
                                                        Console.WriteLine("");
                                                        Console.WriteLine("Chyba! Můžete zadat pouze validní číslo!");
                                                    }
                                                    else if (obsazeno_error == "obsazeno")
                                                    {
                                                        Console.WriteLine("");
                                                        Console.WriteLine("Chyba! Místo je již obsazené, vyberte nějaké jiné!");
                                                        obsazeno_error = "";
                                                    }
                                                    else if (obsazeno_error == "outof")
                                                    {
                                                        Console.WriteLine("");
                                                        Console.WriteLine("Chyba! Místo je mimo rozsah místnosti!");
                                                        obsazeno_error = "";
                                                    }
                                                    Console.WriteLine("");
                                                    Console.WriteLine("Zadejte řadu:");
                                                    vytvareni_r = Console.ReadLine();
                                                    Console.Clear();
                                                } while (!int.TryParse(vytvareni_r, out vytvareni_rady));
                                                var pattern = new Regex(@"^[1-9]\d*$");
                                                if (pattern.Match(vytvareni_rady.ToString()).Success)
                                                {
                                                    vytvareni_error = "";
                                                    break;
                                                }
                                                else
                                                {
                                                    vytvareni_error = "only_nums";
                                                }
                                            }
                                            while (true)
                                            {
                                                do
                                                {
                                                    Console.WriteLine("------------------------------------");
                                                    Console.WriteLine("Vytváření rezervace | Krok 2/4");
                                                    Console.WriteLine("------------------------------------");
                                                    if (vytvareni_error != "")
                                                    {
                                                        Console.WriteLine("");
                                                        Console.WriteLine("Chyba! Můžete zadat pouze validní číslo!");
                                                    }
                                                    Console.WriteLine("");
                                                    Console.WriteLine("Zadejte sloupec:");
                                                    vytvareni_s = Console.ReadLine();
                                                    Console.Clear();
                                                } while (!int.TryParse(vytvareni_s, out vytvareni_sloupce));
                                                var pattern = new Regex(@"^[1-9]\d*$");
                                                if (pattern.Match(vytvareni_sloupce.ToString()).Success)
                                                {
                                                    vytvareni_error = "";
                                                    break;
                                                }
                                                else
                                                {
                                                    vytvareni_error = "only_nums";
                                                }
                                            }
                                            var check = db.Query<Mistnosti_obsazenost>("SELECT * FROM Mistnosti_obsazenost WHERE rada = ? AND sloupec = ?", vytvareni_rady, vytvareni_sloupce);
                                            if (!check.Any())
                                            {
                                                if (sloupce_uprava >= vytvareni_sloupce && rady_uprava >= vytvareni_rady)
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    obsazeno_error = "outof";
                                                }

                                            }
                                            else
                                            {
                                                obsazeno_error = "obsazeno";
                                            }
                                        }
                                        string vytvareni_jmeno = "";
                                        while (true)
                                        {
                                            Console.WriteLine("------------------------------------");
                                            Console.WriteLine("Vytváření rezervace | Krok 3/4");
                                            Console.WriteLine("------------------------------------");
                                            if (vytvareni_error != "")
                                            {
                                                Console.WriteLine("");
                                                Console.WriteLine("Chyba! Zadejte prosim jmeno bez diakritky, obsahovat může pouze písmena!");
                                            }
                                            Console.WriteLine("");
                                            Console.WriteLine("Zadejte jmeno:");
                                            vytvareni_jmeno = Console.ReadLine();
                                            Console.Clear();
                                            var pattern = new Regex(@"^[A-Za-z]*$");
                                            if (pattern.Match(vytvareni_jmeno.ToString()).Success)
                                            {
                                                vytvareni_error = "";
                                                break;
                                            }
                                            else
                                            {
                                                vytvareni_error = "only_lett";
                                            }
                                        }
                                        string vytvareni_email = "";
                                        while (true)
                                        {
                                            Console.WriteLine("------------------------------------");
                                            Console.WriteLine("Vytváření rezervace | Krok 4/4");
                                            Console.WriteLine("------------------------------------");
                                            if (vytvareni_error != "")
                                            {
                                                Console.WriteLine("");
                                                Console.WriteLine("Chyba! Zadejte prosim jmeno bez diakritky, obsahovat může pouze písmena!");
                                            }
                                            Console.WriteLine("");
                                            Console.WriteLine("Zadejte email:");
                                            vytvareni_email = Console.ReadLine();
                                            Console.Clear();
                                            var pattern = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                                            if (pattern.Match(vytvareni_email.ToString()).Success)
                                            {
                                                vytvareni_error = "";
                                                break;
                                            }
                                            else
                                            {
                                                vytvareni_error = "only_lett";
                                            }
                                        }
                                        var _Mistnosti_obsazenost = new Mistnosti_obsazenost();
                                        _Mistnosti_obsazenost.mistnost_id = id_uprava;
                                        _Mistnosti_obsazenost.rada = vytvareni_rady;
                                        _Mistnosti_obsazenost.sloupec = vytvareni_sloupce;
                                        _Mistnosti_obsazenost.jmeno = vytvareni_jmeno;
                                        _Mistnosti_obsazenost.email = vytvareni_email;
                                        db.Insert(_Mistnosti_obsazenost);
                                    }
                                    else if (akce_uprava == "2")
                                    {
                                        string vytvareni_r = "";
                                        int vytvareni_rady = 0;
                                        string vytvareni_s = "";
                                        int vytvareni_sloupce = 0;
                                        string vytvareni_error = "";
                                        string obsazeno_error = "";
                                        do
                                        {
                                            Console.WriteLine("------------------------------------");
                                            Console.WriteLine("Odstranění rezervace | Krok 1/2");
                                            Console.WriteLine("------------------------------------");
                                            if (vytvareni_error != "")
                                            {
                                                Console.WriteLine("");
                                                Console.WriteLine("Chyba! Můžete zadat pouze validní číslo!");
                                            }
                                            Console.WriteLine("");
                                            Console.WriteLine("Zadejte řadu:");
                                            vytvareni_r = Console.ReadLine();
                                            Console.Clear();
                                        } while (!int.TryParse(vytvareni_r, out vytvareni_rady));

                                        do
                                        {
                                            Console.WriteLine("------------------------------------");
                                            Console.WriteLine("Odstranění rezervace | Krok 2/2");
                                            Console.WriteLine("------------------------------------");
                                            if (vytvareni_error != "")
                                            {
                                                Console.WriteLine("");
                                                Console.WriteLine("Chyba! Můžete zadat pouze validní číslo!");
                                            }
                                            Console.WriteLine("");
                                            Console.WriteLine("Zadejte sloupec:");
                                            vytvareni_s = Console.ReadLine();
                                            Console.Clear();
                                        } while (!int.TryParse(vytvareni_s, out vytvareni_sloupce));
                                        db.Query<Mistnosti_obsazenost>("DELETE FROM Mistnosti_obsazenost WHERE mistnost_id = ? AND rada = ? AND sloupec = ?", id_uprava, vytvareni_rady, vytvareni_sloupce);
                                    }
                                    else if (akce_uprava == "3")
                                    {
                                        exit_c = 1;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                error1 = "neexistuje";
                            }
                        }
                        break;
                    case "2":
                        string show_error = "";
                        string name = "";
                        while (name == "")
                        {
                            Console.WriteLine("------------------------------------");
                            Console.WriteLine("Vytváření místnosti | Krok 1/3");
                            Console.WriteLine("------------------------------------");
                            Console.WriteLine("");
                            Console.WriteLine("Zadejte název místnosti:");
                            name = Console.ReadLine();
                            Console.Clear();
                        }
                        string rady_S = "";
                        int rady = 0;
                        while (true)
                        {
                            do
                            {
                                Console.WriteLine("------------------------------------");
                                Console.WriteLine("Vytváření místnosti | Krok 2/3");
                                Console.WriteLine("------------------------------------");
                                if (show_error != "")
                                {
                                    Console.WriteLine("");
                                    Console.WriteLine("Chyba! Můžete zadat pouze validní číslo!");
                                }
                                Console.WriteLine("");
                                Console.WriteLine("Zadejte počet řad:");
                                rady_S = Console.ReadLine();
                                Console.Clear();
                            } while (!int.TryParse(rady_S, out rady));
                            var pattern = new Regex(@"^[1-9]\d*$");
                            if (pattern.Match(rady.ToString()).Success)
                            {
                                show_error = "";
                                break;
                            }
                            else
                            {
                                show_error = "only_nums";
                            }
                        }

                        string sloupce_S = "";
                        int sloupce = 0;
                        while (true)
                        {
                            do
                            {
                                Console.WriteLine("------------------------------------");
                                Console.WriteLine("Vytváření místnosti | Krok 2/3");
                                Console.WriteLine("------------------------------------");
                                if (show_error != "")
                                {
                                    Console.WriteLine("");
                                    Console.WriteLine("Chyba! Můžete zadat pouze validní číslo!");
                                }
                                Console.WriteLine("");
                                Console.WriteLine("Zadejte počet řad:");
                                sloupce_S = Console.ReadLine();
                                Console.Clear();
                            } while (!int.TryParse(sloupce_S, out sloupce));
                            var pattern = new Regex(@"^[1-9]\d*$");
                            if (pattern.Match(sloupce.ToString()).Success)
                            {
                                show_error = "";
                                break;
                            }
                            else
                            {
                                show_error = "only_nums";
                            }
                        }
                        var _Mistnost = new Mistnosti();
                        _Mistnost.nazev = name;
                        _Mistnost.rady = rady;
                        _Mistnost.sloupce = sloupce;
                        db.Insert(_Mistnost);

                        break;
                    case "3":
                        if (!mistnosti.Any())
                        {
                            break;
                        }
                        string error = "";
                        while (true)
                        {
                            Console.WriteLine("------------------------------------");
                            Console.WriteLine("Odstranění místnosti | Krok 1/1");
                            Console.WriteLine("------------------------------------");
                            if (error != "")
                            {
                                Console.WriteLine("");
                                Console.WriteLine("Chyba! Místnost neexistuje!");
                            }
                            Console.WriteLine("");
                            Console.WriteLine("Seznam místností:");

                            foreach (var mistnost in mistnosti)
                            {
                                Console.WriteLine(mistnost.nazev);
                            }
                            Console.WriteLine("");
                            Console.WriteLine("Napište název místnosti:");
                            string nazev_mistnosti_del = Console.ReadLine();
                            Console.Clear();
                            int matches = 0;
                            int id_uprava = 0;
                            foreach (var mistnost in mistnosti)
                            {
                                if (mistnost.nazev == nazev_mistnosti_del)
                                {
                                    matches++;
                                    id_uprava = mistnost.id;
                                }
                            }
                            if (matches != 0)
                            {
                                var delete = db.Query<Mistnosti>("DELETE FROM Mistnosti WHERE nazev = ?", nazev_mistnosti_del);
                                db.Query<Mistnosti_obsazenost>("DELETE FROM Mistnosti_obsazenost WHERE mistnost_id = ?", id_uprava);
                                break;
                            }
                            else
                            {
                                error = "neexistuje";
                            }
                        }
                        break;

                }

            }
        }
    }
    public class Mistnosti
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        [Unique]
        public string nazev { get; set; }
        public int rady { get; set; }
        public int sloupce { get; set; }
    }
    public class Mistnosti_obsazenost
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public int mistnost_id { get; set; }
        public int rada { get; set; }
        public int sloupec { get; set; }
        [MaxLength(50)]
        public string jmeno { get; set; }
        [MaxLength(50)]
        public string email { get; set; }
    }
}