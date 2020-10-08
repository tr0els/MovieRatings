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

        private int reviewerMostReviews;
        private int movieMostReviews;


        public MovieRatingsPerformanceTest(TestFixture data, ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            repository = data.Repository;
            reviewerMostReviews = data.ReviewerMostReviews;
            movieMostReviews = data.MovieMostReviews;
        }

        private double TimeInSeconds(Action ac)
        {
            Stopwatch sw = Stopwatch.StartNew();
            ac.Invoke();
            sw.Stop();
            return sw.ElapsedMilliseconds / 1000d;
        }

        [Fact]
        public void GetNumberOfReviewsFromReviewer()
        {
            MovieRatingsService mrs = new MovieRatingsService(repository);

            double seconds = TimeInSeconds(() =>
            {
                int result = mrs.GetNumberOfReviewsFromReviewer(reviewerMostReviews);
            });

            _testOutputHelper.WriteLine(seconds.ToString());
            Assert.True(seconds <= 4);
        }
    }
}