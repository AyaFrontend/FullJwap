using Jwap.API.Dtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jwap.API.Hubs
{
    public interface IHubs
    {
        public Task BtroadcastMessage(MessageDto message);
        public Task GetStatus(bool status, string UserId);
        public Task LastMessage(string message, string UserId);
        public Task DeleteMessage(MessageDto message);
        public Task Cancel(CallOfferDto callOffer);
        public Task CallStart(CallOfferDto callOffer);
        public Task VideoStart(CallOfferDto callOffer);
        public Task Call(CallOfferDto callOffer);
        public Task VideoCall(CallOfferDto callOffer);
        public Task InCall(FormData blob);
        public Task ChangeProfileImage(UserDto user);
    }
}
