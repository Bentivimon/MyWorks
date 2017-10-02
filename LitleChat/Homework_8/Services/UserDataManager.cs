using Homework_8.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Homework_8.Models;

namespace Homework_8.Services
{
    public class UserDataManager : IDataManager
    {
        public IEnumerable<User> GetUsers()
        {
            return new List<User>
            {
              new User { Id = 1, Age = 12, FName = "Test 1", LName = "Test_2" },
              new User { Id = 2, Age = 15, FName = "Test_2", LName = "Test_2" },
              new User { Id = 3, Age = 22, FName = "Test_3", LName = "Test_3" },
            };
        }
    }
}