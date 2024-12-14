using Batyushkova_lr1.Models;

namespace Batyushkova_lr1
{
    public static class SharedData
    {
        public static List<User> Users { get; } = new List<User>
        {
            new User(){ Login = "user", Password = "user" },
            new User(){ Login = "admin", Password = "admin" },
        };
    }
}
