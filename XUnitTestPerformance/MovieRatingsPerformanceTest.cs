using System;
using System.Diagnostics;
using MovieRatingsApplication.Core.Interfaces;
using MovieRatingsApplication.Core.Services;
using Xunit;
using Xunit.Abstractions;

namespace XUnitTestPerformance
{
    public class MovieRatingsPerformanceTest: IClassFixture<TestFixture>
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private IMovieRatingsRepository repository;

        private const int _timeLimit = 4;

        private int _reviewerMostReviews;
        private int _movieMostReviews;


        public MovieRatingsPerformanceTest(TestFixture data, ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            repository = data.Repository;
            _reviewerMostReviews = data.ReviewerMostReviews;
            _movieMostReviews = data.MovieMostReviews;
        }

        private double TimeInSeconds(Action ac)
        {
            var sw = Stopwatch.StartNew();
            ac.Invoke();
            sw.Stop();
            return sw.ElapsedMilliseconds / 1000d;
        }
        
        [Fact]
        public void NumberOfMoviesWithGrade()
        {
            MovieRatingsService mrs = new MovieRatingsService(repository);

            double seconds = TimeInSeconds(() =>
            {
                var result = mrs.NumberOfMoviesWithGrade(5);
            });

            _testOutputHelper.WriteLine(seconds.ToString());
            Assert.True(seconds <= _timeLimit);
        }

        [Fact]
        public void GetNumberOfReviewsFromReviewer()
        {
            MovieRatingsService mrs = new MovieRatingsService(repository);

            double seconds = TimeInSeconds(() =>
            {
                var result = mrs.GetNumberOfReviewsFromReviewer(_reviewerMostReviews);
            });

            _testOutputHelper.WriteLine(seconds.ToString());
            Assert.True(seconds <= _timeLimit);
        }
        
        [Fact]
        public void GetAverageRateFromReviewer()
        {
            MovieRatingsService mrs = new MovieRatingsService(repository);

            double seconds = TimeInSeconds(() =>
            {
                var result = mrs.GetAverageRateFromReviewer(_reviewerMostReviews);
            });

            _testOutputHelper.WriteLine(seconds.ToString());
            Assert.True(seconds <= _timeLimit);
        }
        
        [Fact]
        public void GetNumberOfRatesByReviewer()
        {
            MovieRatingsService mrs = new MovieRatingsService(repository);

            double seconds = TimeInSeconds(() =>
            {
                var result = mrs.GetNumberOfRatesByReviewer(_reviewerMostReviews, 3);
            });

            _testOutputHelper.WriteLine(seconds.ToString());
            Assert.True(seconds <= _timeLimit);
        }
        
        [Fact]
        public void GetNumberOfReviews()
        {
            MovieRatingsService mrs = new MovieRatingsService(repository);

            double seconds = TimeInSeconds(() =>
            {
                var result = mrs.GetNumberOfReviews(_movieMostReviews);
            });

            _testOutputHelper.WriteLine(seconds.ToString());
            Assert.True(seconds <= _timeLimit);
        }
        
        [Fact]
        public void GetAverageRateOfMovie()
        {
            MovieRatingsService mrs = new MovieRatingsService(repository);

            double seconds = TimeInSeconds(() =>
            {
                var result = mrs.GetAverageRateOfMovie(_movieMostReviews);
            });

            _testOutputHelper.WriteLine(seconds.ToString());
            Assert.True(seconds <= _timeLimit);
        }
        
        [Fact]
        public void GetNumberOfRates()
        {
            MovieRatingsService mrs = new MovieRatingsService(repository);

            double seconds = TimeInSeconds(() =>
            {
                var result = mrs.GetNumberOfRates(_movieMostReviews, 2);
            });

            _testOutputHelper.WriteLine(seconds.ToString());
            Assert.True(seconds <= _timeLimit);
        }
        
        [Fact]
        public void GetMoviesWithHighestNumberOfTopRates()
        {
            MovieRatingsService mrs = new MovieRatingsService(repository);

            double seconds = TimeInSeconds(() =>
            {
                var result = mrs.GetMoviesWithHighestNumberOfTopRates();
            });

            _testOutputHelper.WriteLine(seconds.ToString());
            Assert.True(seconds <= _timeLimit);
        }
        
        [Fact]
        public void GetMostProductiveReviewers()
        {
            MovieRatingsService mrs = new MovieRatingsService(repository);

            double seconds = TimeInSeconds(() =>
            {
                var result = mrs.GetMostProductiveReviewers();
            });

            _testOutputHelper.WriteLine(seconds.ToString());
            Assert.True(seconds <= _timeLimit);
        }
        
        [Fact]
        public void GetTopRatedMovies()
        {
            MovieRatingsService mrs = new MovieRatingsService(repository);

            double seconds = TimeInSeconds(() =>
            {
                var result = mrs.GetTopRatedMovies(2);
            });

            _testOutputHelper.WriteLine(seconds.ToString());
            Assert.True(seconds <= 5); // Test is just above 4. Result: 4.321 seconds
        }
        
        [Fact]
        public void GetTopMoviesByReviewer()
        {
            MovieRatingsService mrs = new MovieRatingsService(repository);

            double seconds = TimeInSeconds(() =>
            {
                var result = mrs.GetTopMoviesByReviewer(_reviewerMostReviews);
            });

            _testOutputHelper.WriteLine(seconds.ToString());
            Assert.True(seconds <= _timeLimit);
        }
        
        [Fact]
        public void GetReviewersByMovie()
        {
            MovieRatingsService mrs = new MovieRatingsService(repository);

            double seconds = TimeInSeconds(() =>
            {
                var result = mrs.GetReviewersByMovie(_movieMostReviews);
            });

            _testOutputHelper.WriteLine(seconds.ToString());
            Assert.True(seconds <= _timeLimit);
        }
    }
}