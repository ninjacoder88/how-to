namespace HowTo.DataAccess.Models
{
    public sealed class LoginUserModel
    {
        public string UserId { get; set; }

        public string Username { get; set; }

        public string HashedPassword { get; set; }

        public byte[] Salt { get; set; }

        public int FailedLoginCount { get; set; }
    }
}
