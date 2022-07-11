using AutoMapper;
using DataAcess.Repository;
using DTO;
using Model.Models;
using System.Security.Authentication;
using Utility;

namespace Service
{
    public class UserService
    {
        public async Task<string> userLoginValidation(UserAccountDTO user)
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
        public async Task<UserAccount> createUserAccount(UserAccountDTO user)
        {
            UserAccount userAccount = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<UserAccountDTO, UserAccount>())).Map<UserAccountDTO, UserAccount>(user);


            string ValidationMessage = Validation.UserValidation(userAccount);
            if (ValidationMessage == "")
            {
                user.UserPassword = new Crypto().Encrypt(userAccount.UserPassword);
                return await new GenericRepository<UserAccount>().Insert(userAccount);
            }
            else throw new Exception(ValidationMessage);
        }
        public async Task<UserAccount> resetUserPassword(UserAccountDTO user)
        {
            var userData = await new GenericRepository<UserAccount>().FindOne(u => u.UserName == user.UserName);

            if (userData == null) throw new Exception("User Not FOund.");

            UserAccount userAccount = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<UserAccountDTO, UserAccount>())).Map<UserAccountDTO, UserAccount>(user);

            string ValidationMessage = Validation.UserValidation(userAccount);

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
