using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using FluentAssertions;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoFakeItEasy;
using Xunit;

namespace MockingStaticCreate
{
    public class SomeTests
    {
        [Fact]
        public void HaveSameId_ForDifferentUsers_Should_ReturnFalse_UsingFakeItEasy()
        {
            var user1 = A.Fake<User>();
                // FakeItEasy.Core.FakeCreationExceptionFailed to create fake of type "Testing.User".
            A.CallTo(() => user1.Id).Returns(1);
            var user2 = A.Fake<User>();
            A.CallTo(() => user2.Id).Returns(2);

            IFixture fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
            var sut = fixture.Create<SomeService>();
            bool result = sut.HaveSameId(user1, user2);
            result.Should().BeFalse();
        }

        [Fact]
        public void HaveSameId_ForDifferentUsers_Should_ReturnFalse_UsingCreate()
        {
            IFixture fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
            var user1 = fixture.Create<User>();
            var user2 = fixture.Create<User>();

            var sut = fixture.Create<SomeService>();
            bool result = sut.HaveSameId(user1, user2); // throws ArgumentNullException
            result.Should().BeFalse();
        }

        [Fact]
        public void HaveSameId_ForDifferentUsers_Should_ReturnFalse_UsingBuild()
        {
            IFixture fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
            User user1 = fixture.Build<User>().With(x => x.Id, 1).Create();
                // System.ArgumentException: prop is read-only
            User user2 = fixture.Build<User>().With(x => x.Id, 2).Create();

            var sut = fixture.Create<SomeService>();
            bool result = sut.HaveSameId(user1, user2);
            result.Should().BeFalse();
        }

        [Fact]
        public void HaveSameId_ForDifferentUsers_Should_ReturnFalse_UsingFactory()
        {
            IFixture fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
            fixture.Customize<User>(o => o.FromFactory(() => User.Create("foo")));
            List<User> users = fixture.CreateMany<User>().ToList();

            var sut = fixture.Create<SomeService>();
            bool result = sut.HaveSameId(users.First(), users.Last()); // throws ArgumentNullException
            result.Should().BeFalse();
        }

        [Fact(Skip = "??")]
        public void HaveSameId_ForDifferentUsers_Should_ReturnFalse_UsingFactoryWithFakeItEasy()
        {
            IFixture fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
            // ???
        }
    }


    public class SomeService
    {
        /// <summary>
        ///     Dummy method comparing read-only property
        /// </summary>
        public bool HaveSameId(User user1, User user2)
        {
            if (user1.Id == null) throw new ArgumentNullException("user1");
            if (user2.Id == null) throw new ArgumentNullException("user2");
            //if (!user1.Id.HasValue || !user2.Id.HasValue) return false;

            return user1.Id.Value == user2.Id.Value;
        }
    }

    public class User
    {
        private User()
        {
        }

        /// <summary>
        ///     Created by Database...
        /// </summary>
        public long? Id { get; protected set; }

        public string Name { get; protected set; }

        public static User Create(string name)
        {
            var user = new User {Name = name};
            return user;
        }
    }
}