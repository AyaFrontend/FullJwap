using AutoMapper;
using Jwap.API.Dtos;
using Jwap.API.Errors;
using Jwap.BLL.Interfaces;
using Jwap.BLL.Specfications;
using Jwap.DAL.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Jwap.API.Controllers
{

    public class UsersController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepo;
        private readonly IMessageRepository _messgRepo;
        private readonly IMapper _map;
        private readonly UserManager<User> _user;

        public UsersController(IUnitOfWork unitOfWork , IMapper map 
            , UserManager<User> user , IUserRepository userRepo, IMessageRepository messgRepo)
        {
            _unitOfWork = unitOfWork;
            _map = map;
            _user = user;
            _userRepo = userRepo;
            _messgRepo = messgRepo;
        }

        [HttpGet("getUser/{username}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<ActionResult<IEnumerable<User>>> GetUser(string username)
        
       {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _user.FindByEmailAsync(email);

            var spec = new UsersWithSpecification( username);
            var users = await _unitOfWork.Repository<User>().Search(spec);
            var friends = users.Where(x => x.Id != user.Id);
            if (friends.Count() == 0) return NotFound(new ApiResponse(404 , "That user not existing"));

            return Ok(_map.Map< IEnumerable<User> , IEnumerable<ConnectionsDto> >(friends));

        }

        [HttpGet("getFriends")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> GetFriends()

        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _user.FindByEmailAsync(email);



            var friends =  _userRepo.GetFriends(user.Id).Where(x => x.Id != user.Id); 
            

            if (friends.Count() == 0) return NotFound(new ApiResponse(404, "That user not existing"));
            var connections = _map.Map<IEnumerable<User>, IEnumerable<ConnectionsDto>>(friends);

            foreach (var item in connections)
            {
                item.LastMessage = _messgRepo.GetLastMessageById(item.Id);
            }
            return Ok(connections);

        }
    }
}
