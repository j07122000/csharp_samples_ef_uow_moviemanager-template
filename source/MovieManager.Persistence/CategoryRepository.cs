using MovieManager.Core.Contracts;
using MovieManager.Core.DataTransferObjects;
using MovieManager.Core.Entities;
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


    }
}