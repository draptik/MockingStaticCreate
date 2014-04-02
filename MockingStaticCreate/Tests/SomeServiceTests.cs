using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FakeItEasy;
using FluentAssertions;
using MockingStaticCreate.App;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoFakeItEasy;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace MockingStaticCreate.Tests
{
    public class SomeServiceTests
    {
        [Fact(Skip = "Fails with: FakeItEasy.Core.FakeCreationExceptionFailed to create fake of type \"Testing.User\"")]
        public void HaveSameId_ForDifferentUsers_Should_ReturnFalse_UsingFakeItEasy()
        {
            var user1 = A.Fake<User>(); // Fails here
            A.CallTo(() => user1.Id).Returns(1);
            var user2 = A.Fake<User>();
            A.CallTo(() => user2.Id).Returns(2);

            IFixture fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
            var sut = fixture.Create<SomeService>();
            bool result = sut.HaveSameId(user1, user2);
            result.Should().BeFalse();
        }

        [Fact(Skip = "throws ArgumentNullException")]
        public void HaveSameId_ForDifferentUsers_Should_ReturnFalse_UsingCreate()
        {
            IFixture fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
            var user1 = fixture.Create<User>();
            var user2 = fixture.Create<User>();

            var sut = fixture.Create<SomeService>();
            bool result = sut.HaveSameId(user1, user2); // throws ArgumentNullException
            result.Should().BeFalse();
        }

        [Fact(Skip = "prop is read-only")]
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

        [Fact(Skip = "throws ArgumentNullException")]
        public void HaveSameId_ForDifferentUsers_Should_ReturnFalse_UsingFactory()
        {
            IFixture fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
            fixture.Customize<User>(o => o.FromFactory(() => User.Create("foo")));
            List<User> users = fixture.CreateMany<User>().ToList();

            var sut = fixture.Create<SomeService>();
            bool result = sut.HaveSameId(users.First(), users.Last()); // throws ArgumentNullException
            result.Should().BeFalse();
        }

        [Fact /*(Skip = "??")*/]
        public void CreatingUserWithId_Should_Work_UsingFactoryAndSpecimenBuilder()
        {
            IFixture fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
            fixture.Customizations.Add(new UserBuilder());
            fixture.Customize<User>(o => o.FromFactory(() => User.Create("foo")));

            var user = fixture.Create<User>();

            user.Should().NotBeNull();
            user.Id.Should().HaveValue();
        }
    }

    public class UserBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            var pi = request as PropertyInfo;
            if (pi == null) return new NoSpecimen(request);

            // The following code only works for public properties... :-(
            if (pi.Name == "Id" && pi.PropertyType == typeof (long?)) return 42;

            return new NoSpecimen(request);
        }
    }
}