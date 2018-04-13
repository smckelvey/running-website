using System.Collections.Generic;
using Website.Models;

namespace Website.Data
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUsers();
    }
}