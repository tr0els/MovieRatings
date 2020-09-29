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
        // returns the number movies which have got the grade N.

        [Theory]
        [InlineData(1,1)]
        [InlineData(3, 1)]
        [InlineData(5,2)]
        public void NumberOfMoviesWithGrade(int grade, int expected)
        { 
            IList<MovieRating> ratings = new List<MovieRating>()
            {
                new MovieRating(1, 1, 3, DateTime.Now),
                new MovieRating(2, 1, 3, DateTime.Now),
                new MovieRating(3, 1, 4, DateTime.Now),

                new MovieRating(3, 5, 5, DateTime.Now),
                new MovieRating(3, 2, 5, DateTime.Now),
                new MovieRating(4, 2, 5, DateTime.Now)
            };

            Mock<IMovieRatingsRepository> repoMock = new Mock<IMovieRatingsRepository>();
            repoMock.Setup(repo => repo.GetAllMovieRatings()).Returns(() => ratings);

            MovieRatingsService mrs = new MovieRatingsService(repoMock.Object);

            int result = mrs.NumberOfMoviesWithGrade(grade);

            Assert.Equal(expected, result);
            repoMock.Verify( repo => repo.GetAllMovieRatings(), Times.Once);
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
            Action ac = () =>  
            { 
                int result = mrs.NumberOfMoviesWithGrade(grade); 
            };

            // assert
            ac.Should().Throw<ArgumentException>().WithMessage("Grade must be 1 - 5");
        }
    }
}
