using MovieManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace MovieManager.Core
{
    public class ImportController
    {
        const string Filename = "movies.csv";

        /// <summary>
        /// Liefert die Movies mit den dazugehörigen Kategorien
        /// </summary>
        public static IEnumerable<Movie> ReadFromCsv()
        {
            string[][] csvMovies = MyFile.ReadStringMatrixFromCsv(Filename, true);

            var category = csvMovies
                .GroupBy(line => line[2])
                .Select(s => new Category
                {
                    CategoryName = s.Key
                }).ToArray();

            Movie[] movies = csvMovies
                .Select(line => new Movie()
                {
                    Category = category.Single(c => c.CategoryName == line[2]),
                    Duration = int.Parse(line[3]),
                    Title = line[0],
                    Year = int.Parse(line[1]),
                }).ToArray();

            return movies;
        }
       

    }
}
