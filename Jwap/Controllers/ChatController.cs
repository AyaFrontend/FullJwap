using AutoMapper;
using Jwap.API.Dtos;
using Jwap.API.Helper;
using Jwap.API.Hubs;
using Jwap.BLL.Interfaces;
using Jwap.BLL.Specfications;
using Jwap.DAL.Entities;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
//using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
namespace Jwap.API.Controllers
{

    public class ChatController : BaseController
    {
         private readonly IHubContext<ChatHub , IHubs> _hub;
        
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _user;
        private readonly ChatHub _cHub;
        private readonly IMapper _map;

        public ChatController(IHubContext<ChatHub, IHubs>  hub,
            ChatHub cHub, IUnitOfWork unitOfWork , UserManager<User> user , IMapper map)
        {
            _hub = hub;
            _cHub = cHub;
            _unitOfWork = unitOfWork;
            _user = user;
            _map = map;
        }

        [HttpPost("post")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

     

        [HttpPost("send")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> SendMessageAsync()
        {
           // var audio = form.Files.GetFile("audio");
            //string messageStringJson = form["message"];
            var formCollection = await Request.ReadFormAsync();
            var file = formCollection?.Files?.FirstOrDefault();
            string messageStringJson = formCollection["message"];

            var option = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, PropertyNameCaseInsensitive = false };
            MessageDto message =  JsonSerializer.Deserialize<MessageDto>(messageStringJson , option);
            message.Id = Guid.NewGuid().ToString();
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _user.FindByEmailAsync(email);
            message.SenderId = user.Id;

            //var mess = new Message()
            //{
            //    RecieverId = message.RecieverId,
            //    SenderId = message.SenderId,
            //    Messege = message.Messege

            //    // AudioUrl = message.AudioUrl
            //};

            if ( message.MessegeType.Contains("audio") || message.MessegeType.Contains("image") || message.MessegeType.Contains("application"))
            {
                // IFormFile audio = DecumentSettings.ConvertFromJsonStringToAudio(message.Messege);
                message.AudioUrl = DecumentSettings.UploadFile(file, "UploadFiles");
                
             
                message.Messege = "";
            }

            
            List<Connections> Ids = await GetConnectionsIds(user.Id , message.RecieverId);
            
            
            foreach (var item in Ids)
            {
                await _hub.Clients.Client(item.UserConnectionId).BtroadcastMessage(message);
                await _hub.Clients.Client(item.UserConnectionId).LastMessage(message.Messege, message.SenderId);
            }






   
            var msg = _map.Map<MessageDto, Message>(message);
            //msg.Id =  Guid.NewGuid().ToString();
        _unitOfWork.Repository<Message>().AddSync(msg);

            await AddFriend(message.RecieverId , user.Id);
            return Ok();
        }
 
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("typing/{ReciverId}")]
        public async Task<IActionResult> Typing(string ReciverId)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _user.FindByEmailAsync(email);

            List<Connections> Ids = await GetConnectionsIds(null , ReciverId);


            foreach (var item in Ids)
            {
               
                await _hub.Clients.Client(item.UserConnectionId).LastMessage("Typing...", ReciverId);
            }
            return Ok();
        }

       
        [HttpGet("getChat/{ReciverId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> GetChat(string ReciverId)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _user.FindByEmailAsync(email);

            var spec = new MessagesWithReciverIdAndSenderIdSpecification(ReciverId, user.Id);
            var messages =  await _unitOfWork.Repository<Message>().Search(spec);

            var messagesDto = _map.Map<IEnumerable<Message>, IEnumerable<MessageDto>>(messages);
            var chat = new ChatDto()
            {
                Messages = messagesDto.ToList(),
                ReceiverId = ReciverId,
                SenderId = user.Id,
                
            };
           
                return Ok(chat);
          
            
        }

        private async Task AddFriend(string friendId , string userId)
        {
            var spec = new FriendsWithSpecification(friendId);
            var friend = await _unitOfWork.Repository<Friends>().Search(spec);

            if (friend.ToList().Count == 0)
            {
                await _unitOfWork.Repository<Friends>().AddAsync(new Friends()
                {
                    UserId = userId,
                    FreindId = friendId
                });

            }
        }

       
        private async Task<List<Connections>> GetConnectionsIds(string userId , string recierverId)
        {
            var userSpec = new ConnectionSpecifications(userId , recierverId);
            var connectionIds = await _unitOfWork.Repository<Connections>().Search(userSpec);

           
            return connectionIds.ToList();
        }
    }
}
