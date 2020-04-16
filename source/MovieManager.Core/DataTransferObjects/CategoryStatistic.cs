using System;
using System.Collections.Generic;
using System.Text;

namespace MovieManager.Core.DataTransferObjects
{
    public class CategoryStatistic
    {
        public String CategoryName { get; set; }

        public int CountMovies { get; set; }

        public int TotallyDurationOfMovies { get; set; }
    }
}
