
using Jwap.API.Dtos;

using Jwap.BLL.Interfaces;

using Jwap.BLL.Specfications;
using Jwap.DAL.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using AutoMapper;

namespace Jwap.API.Hubs
{
    [Authorize]
    public class ChatHub : Hub<IHubs>
    {
        
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<ChatHub, IHubs> _hubs;
        private readonly UserManager<User> _user;
        private readonly IMapper _map;
        private readonly IMessageRepository _messgRepo;



        public ChatHub(IUnitOfWork unitOfWork , IHubContext<ChatHub, IHubs> hub,
           UserManager<User> user , IMapper map, IMessageRepository messgRepo)
        {
            _unitOfWork = unitOfWork;
            _hubs = hub;
            _user = user;
            _map = map;
            _messgRepo = messgRepo;
        }

        
        public override Task OnDisconnectedAsync(Exception exception)
        {
            var email = Context.User.FindFirstValue(ClaimTypes.Email);
            var currentUser =  _user.FindByEmailAsync(email).Result;

           
            

           // var user = _unitOfWork.Repository<User>().GetByIdAsync(currentUser.Id).Result;
            var spec = new ConnectionSpecifications( Context.ConnectionId );
            var connection = _unitOfWork.Repository<Connections>().Search(spec).Result;
            if(connection.Count() != 0)
            { _unitOfWork.Repository<Connections>().DeleteAsync(connection.FirstOrDefault()); }
           
            
           _hubs.Clients.All.GetStatus(false, currentUser.Id);

            currentUser.Online = false;
            _unitOfWork.Repository<User>().UpdateAsync(currentUser) ;
            return base.OnConnectedAsync();
        }

        public async Task CallStart(CallOfferDto callOfferDto)
        {
            var connections = await GetConnectionsIds(callOfferDto.CallerId, callOfferDto.CalleeId);
            callOfferDto.CallState = true;
            callOfferDto.StartCall = true;
            foreach (var item in connections)
            {
                await _hubs.Clients.Client(item.UserConnectionId).CallStart(callOfferDto);

            }

            var CallOffer = _map.Map<CallOfferDto, CallOffer>(callOfferDto);
            await _unitOfWork.Repository<CallOffer>().AddAsync(CallOffer);
        }

        public async Task CanceledCall(CallOfferDto callOfferDto)
        {

            var connections = await GetConnectionsIds(callOfferDto.CallerId, callOfferDto.CalleeId);
            callOfferDto.CallState = false;
            callOfferDto.StartCall = false;
            foreach (var item in connections)
            {
                await _hubs.Clients.Client(item.UserConnectionId).Cancel(callOfferDto);

            }
        
        }

        public async Task StartVideo(CallOfferDto callOfferDto)
        {
            var connections = await GetConnectionsIds(callOfferDto.CallerId, callOfferDto.CalleeId);
            callOfferDto.CallState = true;
            callOfferDto.StartCall = true;
            foreach (var item in connections)
            {
                await _hubs.Clients.Client(item.UserConnectionId).VideoStart(callOfferDto);

            }

            var CallOffer = _map.Map<CallOfferDto, CallOffer>(callOfferDto);
            await _unitOfWork.Repository<CallOffer>().AddAsync(CallOffer);
        }

        public async Task InCalling(CallOfferDto callOfferDto, long size , string type)
         {

            var connections = await GetConnectionsIds(callOfferDto.CallerId, callOfferDto.CalleeId);
            var data = new FormData();
            data.size = size;
            data.type = type;
            foreach (var item in connections)
            {
                await _hubs.Clients.Client(item.UserConnectionId).InCall(data);
            }

        
        }
           

        public async Task VoiceCall(CallOfferDto callOffer)
        {
            var caller = await _unitOfWork.Repository<User>().GetByIdAsync(callOffer.CallerId);
            var callee = await _unitOfWork.Repository<User>().GetByIdAsync(callOffer.CalleeId);
           
            var connections = await GetConnectionsIds(callOffer.CallerId, callOffer.CalleeId);
            callOffer.CallState = true;
            callOffer.CallType = "voice";
            callOffer.CalleeName = callee.FName + " " + callee.LName;
            callOffer.CalleePic = callee.ProfilePicture;
            callOffer.CallerName = caller.FName + " " + caller.LName;
            callOffer.CallerPic = caller.ProfilePicture;
            foreach (var item in connections)
            {
                await _hubs.Clients.Client(item.UserConnectionId).Call(callOffer);

            }
           
        }

        public async Task videoCall(CallOfferDto callOffer)
        {

            var connections = await GetConnectionsIds(callOffer.CallerId, callOffer.CalleeId);
            callOffer.CallState = true;
            callOffer.CallType = "video";
           
            foreach (var item in connections)
            {
                callOffer.Id = item.UserId;
                await _hubs.Clients.Client(item.UserConnectionId).VideoCall(callOffer);

            }

        }

        public async Task DeleteMessageAsync(MessageDto msg)
        {
            
            msg.IsDelete = true;
            var connections = await GetConnectionsIds(msg.SenderId, msg.RecieverId);
            string LastMessage = _messgRepo.GetLastMessageById(msg.SenderId);
            foreach (var item in connections)
            {
                await _hubs.Clients.Client(item.UserConnectionId).DeleteMessage(msg);
                await _hubs.Clients.Client(item.UserConnectionId).LastMessage(LastMessage, item.UserId);
            }

            Message message = _map.Map<MessageDto, Message>(msg);

            await _unitOfWork.Repository<Message>().UpdateAsync(message);

            
        }


        public async Task Typing(string ReciverId)
        {
            
            List<Connections> Ids = await GetConnectionsIds(null, ReciverId);


            foreach (var item in Ids)
            {

                await _hubs.Clients.Client(item.UserConnectionId).LastMessage("Typing...", ReciverId);
            }
          
        }

     
        private async Task<List<Connections>> GetConnectionsIds(string userId, string recierverId)
        {
            var userSpec = new ConnectionSpecifications(userId, recierverId);
            var connectionIds = await _unitOfWork.Repository<Connections>().Search(userSpec);


            return connectionIds.ToList();
        }
    }
}
