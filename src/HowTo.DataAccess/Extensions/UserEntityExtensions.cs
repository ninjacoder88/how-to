using HowTo.DataAccess.Entities;
using HowTo.DataAccess.Models;

namespace HowTo.WebApi.Extensions
{
    internal static class UserEntityExtensions
    {
        public static UserModel ToModel(this UserEntity entity)
        {
            return new UserModel
            {
                EmailAddress = entity.EmailAddress,
                UserId = entity._id.ToString(),
                Username = entity.Username,
            };
        }

        public static LoginUserModel ToLoginModel(this UserEntity entity)
        {
            return new LoginUserModel
            {
                FailedLoginCount = entity.FailedLoginCount,
                HashedPassword = entity.HashedPassword,
                Salt = entity.Salt,
                Username = entity.Username,
                UserId = entity._id.ToString(),
            };
        }
    }
}
