namespace NegotiationApp.Domain.Entities
{
    public class User
    {
        public int Id { get;}
        public string Username { get; private set; }
        public string PasswordHash { get; private set; }
        

        public User(string username, string passwordHash)
        {
            Username = username;
            PasswordHash = passwordHash;
        }
        public void SetPasswordHash(string passwordHash)
        {
            PasswordHash = passwordHash;
        }
    }
}
