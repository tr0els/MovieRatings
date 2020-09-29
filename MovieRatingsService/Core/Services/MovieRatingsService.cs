using MovieRatingsApplication.Core.Interfaces;
using MovieRatingsApplication.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieRatingsApplication.Core.Services
{
    public class MovieRatingsService
    {
        private readonly IMovieRatingsRepository RatingsRepository;

        public MovieRatingsService(IMovieRatingsRepository repo)
        {
            RatingsRepository = repo;
        }

        public int NumberOfMoviesWithGrade(int grade)
        {
            if (grade < 1 || grade > 5)
            {
                throw new ArgumentException("Grade must be 1 - 5");
            }

            HashSet<int> movies = new HashSet<int>();
            foreach (MovieRating rating in RatingsRepository.GetAllMovieRatings())
            {
                if (rating.Grade == grade)
                {
                    movies.Add(rating.Movie);
                }
            }
            return movies.Count;
        }
    }
}
