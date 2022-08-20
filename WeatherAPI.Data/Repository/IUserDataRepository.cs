using WeatherAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAPI.Data.Repository
{
    public interface IUserDataRepository
    {
        string CreateUser(UserData newUser);

        UserData AuthenticateUser(string APIKey, string requiredAccess);

        int RemoveIdleUsers(DateTime lastLogin);

        void RemoveSingleUser(string APIKey);

        void UpdateLoginTime(string APIKey, DateTime loginTime);

    }
}
