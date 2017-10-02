using Homework_8.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework_8.Abstract
{
    public interface IDataManager
    {
        IEnumerable<User> GetUsers();
    }
}
