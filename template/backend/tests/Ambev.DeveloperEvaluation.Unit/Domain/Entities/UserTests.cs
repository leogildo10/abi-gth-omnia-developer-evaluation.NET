using Xunit;
using System;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    public class UserTests
    {
        [Fact(DisplayName = "User: Create with valid data sets properties correctly")]
        public void CreateUser_WithValidData_ShouldSetProperties()
        {
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                Username = "JohnDoe",
                Email = "john.doe@example.com",
                Status = UserStatus.Active,
                Role = UserRole.Customer
            };

            Assert.Equal(userId, user.Id);
            Assert.Equal("JohnDoe", user.Username);
            Assert.Equal("john.doe@example.com", user.Email);
            Assert.Equal(UserStatus.Active, user.Status);
            Assert.Equal(UserRole.Customer, user.Role);
        }

        [Fact(DisplayName = "User: Default status should be Unknown")]
        public void CreateUser_DefaultStatus_ShouldBeUnknown()
        {
            var user = new User();
            Assert.Equal(UserStatus.Unknown, user.Status);
        }
    }
}
