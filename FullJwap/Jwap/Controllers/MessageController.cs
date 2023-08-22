using AutoMapper;
using Jwap.API.Dtos;
using Jwap.API.Hubs;
using Jwap.BLL.Interfaces;
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
using System.Text.Json;
using System.Threading.Tasks;

namespace Jwap.API.Controllers
{

    public class MessageController : BaseController
    {
        private readonly IHubContext<ChatHub, IHubs> _hub;

        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _user;
        private readonly ChatHub _cHub;
        private readonly IMapper _map;
        private readonly IMessageRepository _messgRepo;

        public MessageController(IHubContext<ChatHub, IHubs> hub,
            ChatHub cHub, IUnitOfWork unitOfWork, UserManager<User> user, IMapper map , IMessageRepository messgRepo)
        {
            _hub = hub;
            _cHub = cHub;
            _unitOfWork = unitOfWork;
            _user = user;
            _map = map;
            _messgRepo = messgRepo;
        }


        [HttpPost("remove-message")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> DeleteMessageAsync(MessageDto msg)
        {
           // var email = User.FindFirstValue(ClaimTypes.Email);
          //  var user = await _user.FindByEmailAsync(email);
            msg.IsDelete = true;
            var connections = await GetConnectionsIds(msg.SenderId, msg.RecieverId);
            string LastMessage = _messgRepo.GetLastMessageById(msg.SenderId);
            foreach (var item in connections)
            {
               await _hub.Clients.Client(item.UserConnectionId).DeleteMessage(msg);
                await _hub.Clients.Client(item.UserConnectionId).LastMessage(LastMessage, item.UserId);
            }

            Message message = _map.Map<MessageDto, Message>(msg);
            
            await _unitOfWork.Repository<Message>().UpdateAsync(message);

            return Ok();
        }



        [HttpPost("voice-call")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> VoiceCall(CallOfferDto callOffer)
        {
          
            var connections = await GetConnectionsIds(callOffer.CallerId, callOffer.CalleeId);
            callOffer.CallState = true;
            foreach (var  item in connections)
            {
                await _hub.Clients.Client(item.UserConnectionId).Call(callOffer);
                
            }
            return Ok();
        }

        [HttpPost("canceled-call")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> CanceledCall(CallOfferDto callOffer)
        {

            var connections = await GetConnectionsIds(callOffer.CallerId, callOffer.CalleeId);
            callOffer.CallState = false;
            callOffer.StartCall = false;
            foreach (var item in connections)
            {
                await _hub.Clients.Client(item.UserConnectionId).Cancel(callOffer);

            }
            return Ok();
        }

        [HttpPost("start-call")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CallStart(CallOfferDto callOffer)
        {

            var connections = await GetConnectionsIds(callOffer.CallerId, callOffer.CalleeId);
            callOffer.CallState = true;
            callOffer.StartCall = true;
            foreach (var item in connections)
            {
                await _hub.Clients.Client(item.UserConnectionId).CallStart(callOffer);

            }
            return Ok();
        }


        [HttpPost("incalling")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> InCalling()
        {
            var formCollection = await Request.ReadFormAsync();
            var blob = formCollection?.Files?.FirstOrDefault();
            string callOfferStringJson = formCollection["callInfo"];
            var option = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, PropertyNameCaseInsensitive = false };
            CallOfferDto callOffer = JsonSerializer.Deserialize<CallOfferDto>(callOfferStringJson, option);

            var connections = await GetConnectionsIds(callOffer.CallerId, callOffer.CalleeId);
            var data = new FormData();
            data.size = blob.Length;
            data.type = blob.ContentType;
            foreach (var item in connections)
            {
                await _hub.Clients.Client(item.UserConnectionId).InCall(data);
            }
            return Ok();
        }

        private async Task<List<Connections>> GetConnectionsIds(string userId, string recierverId)
        {
            var userSpec = new ConnectionSpecifications(userId, recierverId);
            var connectionIds = await _unitOfWork.Repository<Connections>().Search(userSpec);


            return connectionIds.ToList();
        }
    }
}
