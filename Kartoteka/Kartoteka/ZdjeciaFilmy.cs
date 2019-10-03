using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kartoteka
{
    class ZdjeciaFilmy
    {
        public static void UtworzWyswietl(string id, string nazwisko, string imie)
        {
            string sciezka = @"C:\Kartoteka\ZdjeciaFilmy\" + nazwisko + imie + id;
            if (Directory.Exists(sciezka))
            {
                Process.Start(sciezka);
            }
            else
            {
                Directory.CreateDirectory(sciezka + "\\Zdjecia");
                Directory.CreateDirectory(sciezka + "\\Filmy");
                Process.Start(sciezka);
            }
        }
    }
}
