using MovieManager.Core;
using MovieManager.Core.Contracts;
using MovieManager.Core.Entities;
using MovieManager.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MovieManager.ImportConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            InitData();
            AnalyzeData();

            Console.WriteLine();
            Console.Write("Beenden mit Eingabetaste ...");
            Console.ReadLine();
        }

        private static void InitData()
        {
            Console.WriteLine("***************************");
            Console.WriteLine("          Import");
            Console.WriteLine("***************************");

            Console.WriteLine("Import der Movies und Categories in die Datenbank");
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                Console.WriteLine("Datenbank löschen");
                unitOfWork.DeleteDatabase();

                Console.WriteLine("Datenbank migrieren");
                unitOfWork.MigrateDatabase();

                Console.WriteLine("Movies/Categories werden eingelesen");

                var movies = ImportController.ReadFromCsv().ToArray();
                if (movies.Length == 0)
                {
                    Console.WriteLine("!!! Es wurden keine Movies eingelesen");
                    return;
                }

                 var categories = movies
                    .Select(m => m.Category )
                    .Distinct();
                

                Console.WriteLine($"  Es wurden {movies.Count()} Movies in {categories.Count()} Kategorien eingelesen!");

                unitOfWork.CategoryRepository.AddRange(categories);
                unitOfWork.Save();

                unitOfWork.MovieRepository.AddRange(movies);
                unitOfWork.Save();
                Console.WriteLine();
            }
        }

        private static void AnalyzeData()
        {
            Console.WriteLine("***************************");
            Console.WriteLine("        Statistik");
            Console.WriteLine("***************************");

            using IUnitOfWork unitOfWork = new UnitOfWork();
            // Längster Film: Bei mehreren gleichlangen Filmen, soll jener angezeigt werden, dessen Titel im Alphabet am weitesten vorne steht.
            // Die Dauer des längsten Films soll in Stunden und Minuten angezeigt werden!
            Movie longestMovie = unitOfWork.MovieRepository.GetLongest();
            Console.WriteLine($"Längster Film: {longestMovie.Title}; Länge: {GetDurationAsString(longestMovie.Duration, false)}");


            // Top Kategorie:
            //   - Jene Kategorie mit den meisten Filmen.
            var mostMovies = unitOfWork.CategoryRepository.GetMostMovies();
            Console.WriteLine($"Kategorie mit den meisten Filmen: '{mostMovies.CategoryName}'; Filme: {mostMovies.CountMovies}");


            // Jahr der Kategorie "Action":
            //  - In welchem Jahr wurden die meisten Action-Filme veröffentlicht?
            var yearAction = unitOfWork.CategoryRepository.GetYearWithAction("Action");
            Console.WriteLine($"Jahr der Action-Filme: {yearAction}");



            // Kategorie Auswertung (Teil 1):
            //   - Eine Liste in der je Kategorie die Anzahl der Filme und deren Gesamtdauer dargestellt wird.
            //   - Sortiert nach dem Namen der Kategorie (aufsteigend).
            //   - Die Gesamtdauer soll in Stunden und Minuten angezeigt werden!
            Console.WriteLine("Kategorie Auswertung: ");
            Console.WriteLine();
            Console.WriteLine("Kategorie    Anzahl      Gesamtdauer");
            Console.WriteLine("====================================");
            foreach(var statistic in unitOfWork.CategoryRepository.GetCategoryStatistic())
            {
                Console.WriteLine($"{statistic.CategoryName,-12} {statistic.CountMovies,-12} {GetDurationAsString(statistic.TotallyDurationOfMovies, false),-12}");
            }


            // Kategorie Auswertung (Teil 2):
            //   - Alle Kategorien und die durchschnittliche Dauer der Filme der Kategorie
            //   - Absteigend sortiert nach der durchschnittlichen Dauer der Filme.
            //     Bei gleicher Dauer dann nach dem Namen der Kategorie aufsteigend sortieren.
            //   - Die Gesamtdauer soll in Stunden, Minuten und Sekunden angezeigt werden!
            //TODO
        }


        private static string GetDurationAsString(double minutes, bool withSeconds = true)
        {
            int hours = (int)minutes / 60;
            int minutesPart = (int)minutes % 60;
            int second = (int)((decimal)(minutes % 1) * 60m);

            string withoutSeconds = $"{hours} h {minutesPart} min";
            if (withSeconds)
            {
                return $"{hours} h {minutesPart} min {second} sec";
            }
            return withoutSeconds;
        }
    }
}
