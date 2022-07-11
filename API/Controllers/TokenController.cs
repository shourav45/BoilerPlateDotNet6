using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Service;
using System.Text;
using DTO;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly UserService service;
        public TokenController(IConfiguration config)
        {
            _configuration = config;
            service = new UserService();
        }
        [HttpPost]
        [Route("~/user/login")]
        public async Task<IActionResult> userLogin(UserAccountDTO user)
        {
            try
            {
                JWTTokenDTO TokenObject = new JWTTokenDTO();

                string UserValidationMessage = service.userLoginValidation(user).Result.ToString();
                //IF USER IS NOT VALIDATE
                if (UserValidationMessage != "OK") throw new Exception(UserValidationMessage);
                //NOW USER NAME AND PASSWORD IS VALID -- GENERATE TOKEN
                else
                {
                    var UserData = service.getUserAccount(user.UserName).Result;
                    //create claims details based on the user information
                    var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserName", UserData.UserName.ToString()),
                        new Claim("MobileNo", UserData.MobileNo),
                        new Claim("Email", UserData.Email),
                        new Claim("FullName", UserData.FullName),
                        new Claim("CIF", UserData.Cif),
                        new Claim("UP", UserData.UserPassword)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(60),
                        signingCredentials: signIn);

                    TokenObject = new JWTTokenDTO
                    {
                        UserName = UserData.UserName,
                        MobileNo = UserData.MobileNo,
                        Email = UserData.Email,
                        FullName = UserData.FullName,
                        CIF = UserData.Cif,
                        TokenExpire = DateTime.UtcNow.AddMinutes(60),
                        Token = new JwtSecurityTokenHandler().WriteToken(token)
                    };
                };

                return Ok(TokenObject);
            }
            catch (Exception ex) { return getResponse(ex); }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("~/user/login/token/refresh")]
        public async Task<IActionResult> userLoginTokenRefresh()
        {
            JWTTokenDTO TokenObject = new JWTTokenDTO();
            string ExpiredToken = Request.Headers["Authorization"].ToString();
            if(ExpiredToken.Length > 0)
            {
                ExpiredToken = ExpiredToken.Remove(0, 7);

                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(ExpiredToken);
                var tokenS = jsonToken as JwtSecurityToken;
                var UserName = tokenS.Claims.First(claim => claim.Type == "UserName").Value;
                var UserPassword = tokenS.Claims.First(claim => claim.Type == "UP").Value;
                //NOW CREATE NEW TOKEN- USERNAME FOUND FROM EXPIRED TOKEN
                var UserData = service.getUserAccount(UserName).Result;
                //RE CHECKING USER PASSWORD FROM EXPIRED TOKEN AND DATABASE
                if (UserData.UserPassword != UserPassword) return BadRequest("Please Login");
                //create claims details based on the user information
                var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserName", UserData.UserName.ToString()),
                        new Claim("MobileNo", UserData.MobileNo),
                        new Claim("Email", UserData.Email),
                        new Claim("FullName", UserData.FullName),
                        new Claim("CIF", UserData.Cif),
                        new Claim("UP", UserData.UserPassword)
                    };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(1),
                    signingCredentials: signIn);

                TokenObject = new JWTTokenDTO
                {
                    UserName = UserData.UserName,
                    MobileNo = UserData.MobileNo,
                    Email = UserData.Email,
                    FullName = UserData.FullName,
                    CIF = UserData.Cif,
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    TokenExpire = DateTime.UtcNow.AddMinutes(1)
                };


                return Ok(TokenObject);
            }
            else
            {
                return BadRequest("Please Login");
            }
            
        }

      
    }
}

