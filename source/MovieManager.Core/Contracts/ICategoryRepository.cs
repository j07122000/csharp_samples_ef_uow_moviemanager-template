using MovieManager.Core.DataTransferObjects;
using MovieManager.Core.Entities;
using System.Collections.Generic;

namespace MovieManager.Core.Contracts
{
    public interface ICategoryRepository
    {
        void AddRange(IEnumerable<Category> categories);
        CategoryStatistic GetMostMovies();
        int GetYearWithAction(string categoryAction);
        CategoryStatistic[] GetCategoryStatistic();
        (string CategoryName, double AverageDuration)[] GetAverageDuration();
    }
}
