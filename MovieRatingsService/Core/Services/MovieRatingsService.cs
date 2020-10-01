using MovieRatingsApplication.Core.Interfaces;
using MovieRatingsApplication.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public int GetNumberOfReviewsFromReviewer(int reviewer)
        {
            int count = 0;
            foreach(MovieRating m in RatingsRepository.GetAllMovieRatings())
            {
                if (m.Reviewer == reviewer)
                {
                    count++;
                }
            }
            return count;

            //return RatingsRepository.GetAllMovieRatings()
            //    .Where(r => r.Reviewer == reviewer)
            //    .Count();
        }

        public List<int> GetMoviesWithHighestNumberOfTopRates()
        {
            var movie5 = RatingsRepository.GetAllMovieRatings()
                .Where(r => r.Grade == 5)
                .GroupBy(r => r.Movie)
                .Select(group => new { 
                    Movie = group.Key,
                    MovieGrade5 = group.Count() 
                });

            int max5 = movie5.Max(grp => grp.MovieGrade5);

            return movie5
                .Where(grp => grp.MovieGrade5 == max5)
                .Select(grp => grp.Movie)
                .ToList();
        }
    }
}
