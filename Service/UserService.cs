using DataAcess.Repository;
using Model.Models;
using System.Security.Authentication;
using Utility;

namespace Service
{
    public class UserService
    {
        public async Task<string> userLoginValidation(UserAccount user)
        {
            var userData = await new GenericRepository<UserAccount>().FindOne(u => u.UserName == user.UserName);

            if (userData == null) throw new AuthenticationException("User Not Found.");
            else if (user.UserPassword != new Crypto().Decrypt(userData.UserPassword)) throw new AuthenticationException("Incorrect Password.");
            else return "OK";
        }

        public async Task<UserAccount> getUserAccount(string username)
        {
            var userData = await new GenericRepository<UserAccount>().FindOne(u => u.UserName == username);

            if (userData == null) throw new Exception("User Not Found.");
            else return userData;
        }
        public async Task<UserAccount> createUserAccount(UserAccount user)
        {
            string ValidationMessage = Validation.UserValidation(user);
            if (ValidationMessage == "")
            {
                user.UserPassword = new Crypto().Encrypt(user.UserPassword);
                return await new GenericRepository<UserAccount>().Insert(user);
            }
            else throw new Exception(ValidationMessage);
        }
        public async Task<UserAccount> resetUserPassword(UserAccount user)
        {
            var userData = await new GenericRepository<UserAccount>().FindOne(u => u.UserName == user.UserName);

            if (userData == null) throw new Exception("User Not FOund.");

            string ValidationMessage = Validation.UserValidation(user);

            if (ValidationMessage == "")
            {
                userData.UserPassword = new Crypto().Encrypt(user.UserPassword);
                return await new GenericRepository<UserAccount>().Update(userData, u => u.UserName == userData.UserName);
            }
            else throw new Exception(ValidationMessage);
        }
        public async Task<bool> deleteUserAccount(string username)
        {
            return await new GenericRepository<UserAccount>().Delete(u => u.UserName == username);
        }



    }
}
