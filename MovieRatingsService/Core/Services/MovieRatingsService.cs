﻿using MovieRatingsApplication.Core.Interfaces;
using MovieRatingsApplication.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;

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
            foreach (MovieRating rating in RatingsRepository.Ratings)
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
            foreach(MovieRating m in RatingsRepository.Ratings)
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
            var movie5 = RatingsRepository.Ratings
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

        public double GetAverageRateFromReviewer(int reviewer)
        {
            var reviews = RatingsRepository.Ratings
                    .Where(r => r.Reviewer == reviewer);

            double number = reviews.Count();
            double totalGrade = reviews.Sum(review => review.Grade);
            if (number == 0) return 0;
            return Math.Round(totalGrade / number, 1);
        }

        public int GetNumberOfRatesByReviewer(int reviewer, int grade)
        {
            return RatingsRepository
                .Ratings
                .Where(r => r.Reviewer == reviewer)
                .Count(g => g.Grade == grade);
        }

        public int GetNumberOfReviews(int movie)
        {
            return RatingsRepository
                .Ratings
                .Count(m => m.Movie == movie);
        }

        public double GetAverageRateOfMovie(int movie)
        {
            var reviews = RatingsRepository.Ratings
                .Where(m => m.Movie == movie);

            double number = reviews.Count();
            double totalGrade = reviews.Sum(review => review.Grade);
            if (number == 0) return 0;
            return Math.Round(totalGrade / number, 1);
        }

        public int GetNumberOfRates(int movie, int grade)
        {
            return RatingsRepository
                .Ratings
                .Where(m => m.Movie == movie)
                .Count(g => g.Grade == grade);
        }

        public List<int> GetMostProductiveReviewers()
        {
            var reviews = RatingsRepository.Ratings
                .GroupBy(r => r.Reviewer)
                .Select(group => new { 
                    Reviewer = group.Key,
                    MovieReviews = group.Count() 
                });

            int maxNumberReviews = reviews.Max(grp => grp.MovieReviews);

            return reviews
                .Where(grp => grp.MovieReviews == maxNumberReviews)
                .Select(grp => grp.Reviewer)
                .ToList();
        } 
        
        public List<int> GetTopRatedMovies(int amount)
        {
            return RatingsRepository.Ratings
                .GroupBy(r => r.Movie)
                .Select(grp => new
                {
                    Movie = grp.Key,
                    GradeAvg = grp.Average(x => x.Grade)
                })
                .OrderByDescending(grp => grp.GradeAvg)
                .Select(grp => grp.Movie)
                .Take(amount)
                .ToList();
        }

        public List<int> GetTopMoviesByReviewer(int reviewer)
        {
            return RatingsRepository.Ratings
                .Where(r => r.Reviewer == reviewer)
                .OrderByDescending(grp => grp.Grade)
                .ThenByDescending(grp => grp.Date)
                .Select(grp => grp.Movie)
                .ToList();
        }

        public List<int> GetReviewersByMovie(int movie)
        {
            return RatingsRepository.Ratings
                .Where(m => m.Movie == movie)
                .OrderByDescending(grp => grp.Grade)
                .ThenByDescending(grp => grp.Date)
                .Select(grp => grp.Reviewer)
                .ToList();
        }
    }
}
