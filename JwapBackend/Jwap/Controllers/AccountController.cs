using AutoMapper;
using Hanssens.Net;
using Jwap.API.Dtos;
using Jwap.API.Errors;
using Jwap.API.Helper;
using Jwap.API.Hubs;
using Jwap.BLL.Interfaces;
using Jwap.BLL.Services;
using Jwap.DAL.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;


namespace Jwap.API.Controllers
{

    public class AccountController : BaseController
    {
        private IConfiguration _configuration;
        private readonly UserManager<User> _user;
        private readonly SignInManager<User> _signIn;
        private readonly IMapper _map;
        private readonly IMailService _mailService;
        private readonly IResponseCacheRepository _cache;
        private readonly ITokenService _token;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ChatHub _cHub;

        public AccountController(UserManager<User> user, SignInManager<User> signIn,
            IMapper map, IMailService mailService, IResponseCacheRepository cache,
            ITokenService token , IUnitOfWork unitOfWork , ChatHub cHub , IConfiguration configuration)
        {
            _user = user;
            _signIn = signIn;
            _map = map;
            _mailService = mailService;
            _cache = cache;
            _token = token;
            _unitOfWork = unitOfWork;
            _cHub = cHub;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerData)
        {
            if (ModelState.IsValid)
            {
                if (IsEmailExist(registerData.email).Result.Value) return BadRequest("That email aready exist");

                await _cache.CacheResponseAsync("registeredData", registerData, TimeSpan.FromMinutes(5));
                var vaidationCode = _mailService.GenerateRandom4DigitsCode();
                await _cache.CacheResponseAsync(vaidationCode.ToString(), vaidationCode, TimeSpan.FromMinutes(2));


                await _mailService.SendEmailAsync("ayamohamedabdelrahman868@gmail.com", registerData.email, "Validation Code From Jwap", $"Your validation code is ( {vaidationCode} )");


                return Ok();
            }
            return BadRequest();
        }

        [HttpPost("completeRegister")]
        public async Task< ActionResult<UserDto>> CompleteRegister(ValidationCodeDto code)
        {

            var cachedCode = await _cache.GetCachedResponse(code.ToString());
            if (cachedCode == null) return NotFound(new ApiResponse(404, "Expired Date"));
            else if (cachedCode != code.ValidationCode) return BadRequest(new ApiResponse(400 , "incorrect code"));

            var stringRegistredData = await _cache.GetCachedResponse("registeredData");

            if (string.IsNullOrEmpty(stringRegistredData)) return NotFound(new ApiResponse(400, "expired date"));

            var registredData = JsonSerializer.Deserialize<RegisterDto>(stringRegistredData);
            var user = _map.Map<RegisterDto, User>(registredData);
            user.UserName = registredData.email.Split("@")[0];
            var result = await _user.CreateAsync(user, registredData.password);
            if (!result.Succeeded) return BadRequest(result);
            var userDto = new UserDto()
            {
                Id = user.Id,
                ProfilePicture = user.ProfilePicture,
                Email = user.Email,
                DisplayName = $"{user.FName}",
                Token = await _token.CreateToken(user, _user)
            };
            return Ok(userDto);

        }

        [HttpGet("isEmailExist")]
        public async Task<ActionResult<bool>> IsEmailExist([FromQuery] string email)
        {
            return await _user.FindByEmailAsync(email) != null;
          
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            User user = new User();
            try { 
             user = await _user.FindByEmailAsync(loginDto.Email);
            }
          catch(Exception ex)
          { }
            

           
            if (user == null) return BadRequest("Email or password incorrect");

            var result = await _signIn.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (result == null) return BadRequest("Email or password incorrect");

            var userDto = new UserDto()
            {
                Id = user.Id,
                DisplayName = $"{user.FName} {user.LName}",
                Email = user.Email,
                ProfilePicture = user.ProfilePicture,
                Token = await _token.CreateToken(user, _user)
            };

            
           
            Settings.UserId = user.Id;
            return Ok(userDto);
        }
      
        [HttpGet("currentUser")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _user.FindByEmailAsync(email);

           
            var userDto = new UserDto()
            {
                Email = user.Email,
                DisplayName = $"{user.FName} {user.LName}",
                Token = await _token.CreateToken(user, _user)
            };

            return Ok(userDto);
        }


        [HttpPost("forgetPassword")]
        public async Task<ActionResult> ForgetPassword (ForgetPasswordDto data)
        {
            if(IsEmailExist(data.Email).Result.Value)
            {
                var user = await _user.FindByEmailAsync(data.Email);
                var token = await _token.CreateToken(user, _user);
                string link = $"http://localhost:4200/change-password/{data.Email}/{token}";
                await _cache.CacheResponseAsync("changePasswordLink", link, TimeSpan.FromMinutes(5));
                await _mailService.SendEmailAsync("ayamohamedabdelrahman868@gmail.com", data.Email,"Reset Your Password", "To change your password please click here " + link);
                return Ok(token);
            }
            return NotFound(new ApiResponse(404, "That email dose not exist"));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("changePassword")]
        
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            if (await _cache.GetCachedResponse("changePasswordLink") == null ) return BadRequest(new ApiResponse(404 , "Link is expired, please try again"));

            var user = await _user.FindByEmailAsync(changePasswordDto.Email);
            if (user == null) return BadRequest(new ApiResponse(404, "This email not found"));
            user.Password = changePasswordDto.Password;
            await _unitOfWork.Repository<User>().UpdateAsync(user);
           // var result = await _user.ResetPasswordAsync(user, changePasswordDto.Token, changePasswordDto.Password);
            //if (!result.Succeeded) return BadRequest(new ApiResponse(400 , "Password faild to change"));
                
             return Ok(new ApiResponse(200 , "password is changed correctly"));


        }

    }



     



}
