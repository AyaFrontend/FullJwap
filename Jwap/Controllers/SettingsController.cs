using AutoMapper;
using Jwap.API.Dtos;
using Jwap.API.Helper;
using Jwap.API.Hubs;
using Jwap.BLL.Interfaces;
using Jwap.DAL.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Jwap.API.Controllers
{
   
    public class SettingsController : BaseController
    {
        private readonly UserManager<User> _user;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ChatHub _cHub;
        private readonly IMapper _map;
        private readonly IHubContext<ChatHub, IHubs> _hubs;
        public SettingsController(UserManager<User> user , IUnitOfWork unitOfWork , ChatHub cHub
            , IMapper map , IHubContext<ChatHub, IHubs> hubs)
        {
            _map = map;
            _user = user;
            _unitOfWork = unitOfWork;
            _cHub = cHub;
            _hubs = hubs;
        }

        [HttpPost("change-profile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<ActionResult<UserDto>> ChangeProfileImage()
        {
            
            IFormCollection formCollection = await Request.ReadFormAsync();
            IFormFile file = formCollection?.Files?.FirstOrDefault();

            string path = DecumentSettings.UploadFile(file, "");

            string email = User.FindFirstValue(ClaimTypes.Email);
            User user = await _user.FindByEmailAsync(email);

            user.ProfilePicture = path;
            UserDto userDto = _map.Map<User, UserDto>(user);
         
            await _unitOfWork.Repository<User>().UpdateAsync(user);

            
            return Ok(userDto);
        }

       

    }
}
