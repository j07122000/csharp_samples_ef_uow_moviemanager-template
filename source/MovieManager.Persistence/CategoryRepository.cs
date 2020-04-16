using MovieManager.Core.Contracts;
using MovieManager.Core.DataTransferObjects;
using MovieManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieManager.Persistence
{
    internal class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddRange(IEnumerable<Category> categories)
        {
            _dbContext.Categories.AddRange(categories);
        }
        public CategoryStatistic GetMostMovies()
        {
            return _dbContext
                .Movies
                .GroupBy(c => c.Category.CategoryName)
                .Select(m => new CategoryStatistic()
                {
                    CategoryName = m.Key,
                    CountMovies = m.Count()
                })
                .OrderByDescending(count => count.CountMovies)
                .ThenBy(cat => cat.CategoryName)
                .FirstOrDefault();

        }

        public int GetYearWithAction(string categoryAction)
        {
            var res = _dbContext.Movies
                .Where(cat => cat.Category.CategoryName == categoryAction)
                .GroupBy(y => y.Year)
                .Select(s => new
                {
                    Year = s.Key,
                    Count = s.Count()
                })
                .OrderByDescending(m => m.Count).First();

            return res.Year;
        }

        public CategoryStatistic[] GetCategoryStatistic()
        {
            return _dbContext.Movies
                .GroupBy(c => c.Category.CategoryName)
                  .Select(m => new CategoryStatistic()
                  {
                      CategoryName = m.Key,
                      CountMovies = m.Count(),
                      TotallyDurationOfMovies = m.Sum(d => d.Duration)
                  })
                .OrderBy(count => count.CategoryName)
                .ToArray();
        }
        public (string CategoryName, double AverageDuration)[] GetAverageDuration()
        {
            var res = _dbContext
                .Categories
                .Select(c => new
                {
                    CategoryName = c.CategoryName,
                    AverageDuration = c.Movies.Average(m => m.Duration)
                })
                .OrderByDescending(s => s.AverageDuration)
                .ThenBy(c => c.CategoryName)
                .ToArray();

           return res
                .Select(c => ValueTuple.Create(c.CategoryName, c.AverageDuration))
                .ToArray();
        }


    }
}