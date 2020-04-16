using MovieManager.Core.Entities;
using System.Collections.Generic;

namespace MovieManager.Core.Contracts
{
    public interface ICategoryRepository
    {
        void AddRange(IEnumerable<Category> categories);
    }
}
