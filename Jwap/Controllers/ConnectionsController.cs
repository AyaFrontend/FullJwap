using AutoMapper;
using Jwap.API.Dtos;
using Jwap.API.Hubs;
using Jwap.BLL.Interfaces;
using Jwap.BLL.Services;
using Jwap.BLL.Specfications;
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
   
    public class ConnectionsController : BaseController
    {
        private readonly IHubContext<ChatHub, IHubs> _hub;
        private readonly IResponseCacheRepository _cache;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _user;
        private readonly ChatHub _cHub;
        private readonly IMapper _map;

        public ConnectionsController(IHubContext<ChatHub, IHubs> hub,
            ChatHub cHub, IUnitOfWork unitOfWork, UserManager<User> user, IMapper map,
            IResponseCacheRepository cache)
        {
            _hub = hub;
            _cHub = cHub;
            _unitOfWork = unitOfWork;
            _user = user;
            _map = map;
            _cache = cache;
                
        }



        [HttpPost("add-connection")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> AddConnection(OnlineConnectionsDto connectionDto)
        {
            var email =  User.FindFirstValue(ClaimTypes.Email);
            var user =  await _user.FindByEmailAsync(email);

           await _hub.Clients.All.GetStatus(true, user.Id);
            ConnectionSpecifications spec = new ConnectionSpecifications(connectionDto.UserConnectionId);
            var connections =   await _unitOfWork.Repository<Connections>().Search(spec);
            if(connections.FirstOrDefault() == null)
            {
                 await _unitOfWork.Repository<Connections>().AddAsync(
                    new Connections()
                    {
                        Online =true,
                        UserConnectionId = connectionDto.UserConnectionId,
                        UserId = user.Id
                    }
                    );
                
                user.Online = true;
                await _unitOfWork.Repository<User>().UpdateAsync(user);
            }
            
            ///Settings.ConnectionId = connectionDto.UserConnectionId;
            return Ok();
        }
    }
}
