namespace HowTo.IdentityApi.DataAccess
{
    internal interface IUserRepository
    {
        Task<User?> LoadUserAsync(int id);

        Task<User?> LoadUserAsync(string username);
    }

    internal sealed class UserRepository : IUserRepository
    {
        public async Task<User?> LoadUserAsync(int id)
        {
            await Task.Delay(100);
            return _userDatabase.FirstOrDefault(x => x.Id == id);
        }

        public async Task<User?> LoadUserAsync(string username)
        {
            await Task.Delay(100);
            return _userDatabase.FirstOrDefault(x => x.Username == username);
        }

        private readonly List<User> _userDatabase = new List<User>() {
            new User { Id = 1, Username = "testaccount", FirstName = "Test", LastName = "Account", EmailAddress = "testaccount@test.com", Password = "password1234", GroupId = "user" },
            new User { Id = 2, Username = "adminaccount", FirstName = "Admin", LastName = "Account", EmailAddress = "adminaccount@test.com", Password = "1234password", GroupId = "admin" },
        };
    }

    internal class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public string GroupId { get; set; }
    }
}
