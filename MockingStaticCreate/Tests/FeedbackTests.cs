using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using MockingStaticCreate.App;
using Ploeh.AutoFixture;
using Xunit;

namespace MockingStaticCreate.Tests
{
    public class FeedbackTests
    {
        private static readonly Random Rnd = new Random();

        [Fact(Skip = "Default AutoFixture.Create fails.")]
        public void CreatingMockWorks_AutoFixture_Create_NaiveApproach()
        {
            var fixture = new Fixture();
            var feedback = fixture.Create<Feedback>();
            Console.WriteLine("Feedback quality is: " + feedback.Quality);
            feedback.Quality.Should().BeGreaterOrEqualTo(0);
            feedback.Quality.Should().BeLessOrEqualTo(100);
        }

        [Fact]
        public void CreatingMockWorks_AutoFixture_Create_UsingBuild_ForSingleUsage()
        {
            var fixture = new Fixture();
            Feedback feedback = fixture.Build<Feedback>()
                .With(f => f.Quality, RandomPercentage())
                .Create();

            feedback.Quality.Should().BeGreaterOrEqualTo(0);
            feedback.Quality.Should().BeLessOrEqualTo(100);
        }

        [Fact(Skip = "Default AutoFixture.Create for many fails.")]
        public void CreatingMockWorks_AutoFixture_Create_UsingCustomize_ForRepeatedUsage_Naive()
        {
            var fixture = new Fixture();
            fixture.Customize<Feedback>(c => c
                .With(f => f.Quality, RandomPercentage())); // This will always assign the same number!!

            var fb1 = fixture.Create<Feedback>();
            var fb2 = fixture.Create<Feedback>();

            fb1.Quality.Should().NotBe(fb2.Quality);
        }

        [Fact(Skip = "Default AutoFixture.CreateMany<T> for fails.")]
        public void CreatingMockWorks_AutoFixture_Create_UsingCustomize_ForRepeatedUsage()
        {
            var fixture = new Fixture();
            fixture.Customize<Feedback>(c => c
                .With(f => f.Quality, RandomPercentage())); // This will always assign the same number!!

            IEnumerable<Feedback> feedbacks = fixture.CreateMany<Feedback>();
            var fb1 = feedbacks.First();
            var fb2 = feedbacks.Last();

            fb1.Quality.Should().NotBe(fb2.Quality);
        }

        private int RandomPercentage()
        {
            return Rnd.Next(0, 100);
        }
    }
}