using MovieManager.Core.Contracts;
using MovieManager.Core.Entities;
using System.Collections.Generic;

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


    }
}