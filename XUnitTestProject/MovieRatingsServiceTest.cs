using FluentAssertions;
using Moq;
using MovieRatingsApplication.Core.Interfaces;
using MovieRatingsApplication.Core.Model;
using MovieRatingsApplication.Core.Services;
using System;
using System.Collections.Generic;
using System.Data;
using Xunit;

namespace XUnitTestProject
{
    public class MovieRatingsServiceTest
    {
        private List<MovieRating> ratings = null;
        private Mock<IMovieRatingsRepository> repoMock;

        public MovieRatingsServiceTest()
        {
            repoMock = new Mock<IMovieRatingsRepository>();
            repoMock.Setup(repo => repo.GetAllMovieRatings()).Returns(() => ratings);
        }

        // returns the number movies which have got the grade N.

        [Theory]
        [InlineData(4, 1)]
        [InlineData(3, 1)]
        [InlineData(5, 2)]
        public void NumberOfMoviesWithGrade(int grade, int expected)
        {
            // arrange
            ratings = new List<MovieRating>()
            {
                new MovieRating(1, 1, 3, DateTime.Now),
                new MovieRating(2, 1, 3, DateTime.Now),
                new MovieRating(3, 1, 4, DateTime.Now),
                new MovieRating(3, 5, 5, DateTime.Now),
                new MovieRating(3, 2, 5, DateTime.Now),
                new MovieRating(4, 2, 5, DateTime.Now)
            };

            MovieRatingsService mrs = new MovieRatingsService(repoMock.Object);

            // act
            int result = mrs.NumberOfMoviesWithGrade(grade);

            // assert
            Assert.Equal(expected, result);
            repoMock.Verify(repo => repo.GetAllMovieRatings(), Times.Once);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(6)]
        public void NumberOfMoviesWithGradeInvalidExpectArgumentException(int grade)
        {
            // arrange
            Mock<IMovieRatingsRepository> repoMock = new Mock<IMovieRatingsRepository>();
            MovieRatingsService mrs = new MovieRatingsService(repoMock.Object);

            // act
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                int result = mrs.NumberOfMoviesWithGrade(grade);
            });

            // assert
            Assert.Equal("Grade must be 1 - 5", ex.Message);
        }


        // 1. On input N, what are the number of reviews from reviewer N? 

        [Theory]
        [InlineData(1, 0)]
        [InlineData(2, 1)]
        [InlineData(3, 2)]
        public void GetNumberOfReviewsFromReviewer(int reviewer, int expected)
        {
            // arrange
            ratings = new List<MovieRating>()
            {
                new MovieRating(2, 1, 3, DateTime.Now),
                new MovieRating(3, 1, 4, DateTime.Now),
                new MovieRating(3, 2, 3, DateTime.Now),
                new MovieRating(4, 1, 4, DateTime.Now)
            };

            MovieRatingsService mrs = new MovieRatingsService(repoMock.Object);

            // act

            int result = mrs.GetNumberOfReviewsFromReviewer(reviewer);

            // assert
            Assert.Equal(expected, result);
            repoMock.Verify(repo => repo.GetAllMovieRatings(), Times.Once);
        }
        
        // 2. On input N, what is the average rate that reviewer N had given?
        [Theory]
        [InlineData(1, 0)]
        [InlineData(2, 3)]
        [InlineData(3, 3.5)]
        [InlineData(4, 4)]
        [InlineData(5, 3.3)]
        public void GetAverageRateFromReviewer(int reviewer, double expected)
        {
            // arrange
            
            ratings = new List<MovieRating>()
            {
                new MovieRating(2, 1, 3, DateTime.Now),
                new MovieRating(3, 1, 4, DateTime.Now),
                new MovieRating(3, 2, 3, DateTime.Now),
                new MovieRating(4, 1, 4, DateTime.Now),
                new MovieRating(5, 4, 5, DateTime.Now),
                new MovieRating(5, 4,3, DateTime.Now),
                new MovieRating(5, 6, 2, DateTime.Now)
            };
            
            MovieRatingsService mrs = new MovieRatingsService(repoMock.Object);
            
            // act
            
            double result = mrs.GetAverageRateFromReviewer(reviewer);
            
            // assert
            Assert.Equal(expected, result);
            repoMock.Verify(repo => repo.GetAllMovieRatings(), Times.Once);
        }
        
        // 3. On input N and R, how many times has reviewer N given rate R?
        [Theory]
        [InlineData(1, 5, 0)]
        [InlineData(2, 3, 3)]
        [InlineData(1, 1, 4)]
        public void GetNumberOfRatesByReviewer(int reviewer, int rating, int expected)
        {
            // arrange
            ratings = new List<MovieRating>()
            {
                new MovieRating(1, 1, 1, DateTime.Now),
                new MovieRating(2, 2, 3, DateTime.Now),
                new MovieRating(2, 3, 3, DateTime.Now),
                new MovieRating(2, 4, 3, DateTime.Now),
                new MovieRating(1, 5, 1, DateTime.Now),
                new MovieRating(1, 3, 1, DateTime.Now),
                new MovieRating(1, 6, 1, DateTime.Now)
            };

            MovieRatingsService mrs = new MovieRatingsService(repoMock.Object);

            // act
            int result = mrs.GetNumberOfRatesByReviewer(reviewer, rating);

            // assert
            Assert.Equal(expected, result);
            repoMock.Verify(repo => repo.GetAllMovieRatings(), Times.Once);
        }
        
        // 4. On input N, how many have reviewed movie N?
        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 0)]
        [InlineData(2, 1)]
        [InlineData(3, 4)]
        [InlineData(6, 2)]
        public void GetNumberOfReviews(int movie, int expected)
        {
            // arrange
            ratings = new List<MovieRating>()
            {
                new MovieRating(1, 2, 3, DateTime.Now),
                new MovieRating(3, 3, 4, DateTime.Now),
                new MovieRating(3, 3, 3, DateTime.Now),
                new MovieRating(4, 3, 4, DateTime.Now),
                new MovieRating(5, 3, 5, DateTime.Now),
                new MovieRating(5, 6, 5, DateTime.Now),
                new MovieRating(1, 6, 5, DateTime.Now)
            };

            MovieRatingsService mrs = new MovieRatingsService(repoMock.Object);

            // act

            int result = mrs.GetNumberOfReviews(movie);

            // assert
            Assert.Equal(expected, result);
            repoMock.Verify(repo => repo.GetAllMovieRatings(), Times.Once);
        }
        
        // 5. On input N, what is the average rate the movie N had received?
        [Theory]
        [InlineData(1, 0)]
        [InlineData(2, 3)]
        [InlineData(3, 3.5)]
        [InlineData(4, 4)]
        [InlineData(5, 3.3)]
        public void GetAverageRateOfMovie(int movie, double expected)
        {
            // arrange
            ratings = new List<MovieRating>()
            {
                new MovieRating(3, 2, 3, DateTime.Now),
                new MovieRating(3, 5, 3, DateTime.Now),
                new MovieRating(6, 5, 5, DateTime.Now),
                new MovieRating(3, 5, 2, DateTime.Now),
                new MovieRating(5, 4, 5, DateTime.Now),
                new MovieRating(7, 4, 3, DateTime.Now),
                new MovieRating(5, 3, 2, DateTime.Now),
                new MovieRating(5, 3, 5, DateTime.Now)
            };

            MovieRatingsService mrs = new MovieRatingsService(repoMock.Object);

            // act

            double result = mrs.GetAverageRateOfMovie(movie);

            // assert
            Assert.Equal(expected, result);
            repoMock.Verify(repo => repo.GetAllMovieRatings(), Times.Once);
        }
        
        // 6. On input N and R, how many times had movie N received rate R?
        [Theory]
        [InlineData(0, 1, 0)]
        [InlineData(1, 5, 0)]
        [InlineData(1, 1, 1)]
        [InlineData(2, 3, 1)]
        [InlineData(3, 1, 2)]
        [InlineData(4, 3, 3)]
        public void GetNumberOfRates(int movie, int rating, int expected)
        {
            // arrange
            ratings = new List<MovieRating>()
            {
                new MovieRating(1, 1, 1, DateTime.Now),
                new MovieRating(2, 2, 3, DateTime.Now),
                new MovieRating(2, 3, 1, DateTime.Now),
                new MovieRating(3, 3, 1, DateTime.Now),
                new MovieRating(2, 4, 3, DateTime.Now),
                new MovieRating(1, 4, 3, DateTime.Now),
                new MovieRating(3, 4, 3, DateTime.Now)
            };

            MovieRatingsService mrs = new MovieRatingsService(repoMock.Object);

            // act

            int result = mrs.GetNumberOfRates(movie, rating);

            // assert
            Assert.Equal(expected, result);
            repoMock.Verify(repo => repo.GetAllMovieRatings(), Times.Once);
        }

        // 7. What is the id(s) of the movie(s) with the highest number of top rates (5)? 
        [Fact]
        public void GetMoviesWithHighestNumberOfTopRates()
        {
            ratings = new List<MovieRating>()
            {
                new MovieRating(1, 1, 5, DateTime.Now),
                new MovieRating(1, 2, 5, DateTime.Now),
                new MovieRating(2, 1, 4, DateTime.Now),
                new MovieRating(2, 2, 5, DateTime.Now),
                new MovieRating(2, 3, 5, DateTime.Now),
                new MovieRating(3, 3, 5, DateTime.Now)
            };

            MovieRatingsService mrs = new MovieRatingsService(repoMock.Object);
            
            List<int> expected = new List<int>(){ 2, 3 };

            // act
            var result = mrs.GetMoviesWithHighestNumberOfTopRates();

            // assert
            Assert.Equal(expected, result);
            repoMock.Verify(repo => repo.GetAllMovieRatings(), Times.Once);
        }
        
        // 8. What reviewer(s) had done most reviews?
        [Fact]
        public void GetMostProductiveReviewers()
        {
            ratings = new List<MovieRating>()
            {
                new MovieRating(1, 1, 5, DateTime.Now),
                new MovieRating(1, 2, 5, DateTime.Now),
                new MovieRating(2, 1, 4, DateTime.Now),
                new MovieRating(2, 2, 5, DateTime.Now),
                new MovieRating(2, 3, 5, DateTime.Now),
                new MovieRating(1, 3, 5, DateTime.Now),
                new MovieRating(3, 4, 3, DateTime.Now),
                new MovieRating(3, 4, 4, DateTime.Now)
            };

            MovieRatingsService mrs = new MovieRatingsService(repoMock.Object);
            
            List<int> expected = new List<int>(){ 1, 2 };

            // act
            var result = mrs.GetMostProductiveReviewers();

            // assert
            Assert.Equal(expected, result);
            repoMock.Verify(repo => repo.GetAllMovieRatings(), Times.Once);
        }
        
        // 9. On input N, what is top N of movies? The score of a movie is its average rate.
        [Theory]
        [InlineData(1, 0)]
        [InlineData(2, 1)]
        [InlineData(3, 4)]
        [InlineData(6, 2)]
        public void GetTopRatedMovies(int amount, int expected)
        {
            ratings = new List<MovieRating>()
            {
                new MovieRating(1, 1, 5, DateTime.Now),
                new MovieRating(2, 1, 4, DateTime.Now),
                new MovieRating(1, 2, 5, DateTime.Now),
                new MovieRating(2, 2, 5, DateTime.Now),
                new MovieRating(2, 3, 5, DateTime.Now),
                new MovieRating(1, 3, 1, DateTime.Now),
                new MovieRating(3, 4, 3, DateTime.Now),
                new MovieRating(3, 4, 4, DateTime.Now)
            };
            
            MovieRatingsService mrs = new MovieRatingsService(repoMock.Object);
            
            // act
            //var result = mrs.GetMostProductiveReviewers();

            // assert
            //Assert.Equal(expected, result);
            repoMock.Verify(repo => repo.GetAllMovieRatings(), Times.Once);
        }
    }
}
