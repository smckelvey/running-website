using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Website.Data;
using Website.Models;

namespace Website.Business
{
    public static class MembershipProvider
    {
        private static IUserRepository _repository;
        private static IUserRepository Repository => _repository ?? (_repository = new RunningRepository());

        public static IEnumerable<User> ReadAll()
        {
            return Repository.GetUsers();
        }
    }
}
