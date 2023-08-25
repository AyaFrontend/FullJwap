import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Observable, Subject } from 'rxjs';
import { MessageDto } from 'src/Dto/message-dto';
import { environment } from 'src/environments/environment';
import { CallOfferDto } from '../Models/call-offer-dto';

@Injectable({
  providedIn: 'root'
})
export class MessagesService {
  private sharedObj = new Subject<MessageDto>() ;
  private receivedObject : MessageDto = new MessageDto();

  private sharedCallOfferObj = new Subject<CallOfferDto>() ;
  private receivedCallOfferObject! : CallOfferDto;
  private connection: signalR.HubConnection =  new signalR.HubConnectionBuilder()
  .withUrl(`https://localhost:44399/chatsocket?token=${localStorage.getItem('jwt-token')}`,
  {
    skipNegotiation: false,
    transport: signalR.HttpTransportType.WebSockets
  } ).build();

  constructor(private _http: HttpClient ) { 

    this.connection.onclose(async() =>
    {
      console.log("closed")
     
      await this.Start();
    }); 
 this.Start();
    this.connection.on("DeleteMessage", (message : MessageDto)=>
    {
      this.mapRecievedMessage(message);
    });

    this.connection.on("VoiceCall", (callOffer : CallOfferDto)=>
    {
      this.mapRecievedVoiceOffer(callOffer);
      console.log("calloffer" , callOffer)
    });
  }


  public async Start()
  {
     await this.connection.start();
  } 
  

  public DeleteMsg(msgDto: MessageDto)
  {
    return this._http.post(environment.BASE_URL + 'Message/remove-message' , msgDto).subscribe();
  }

  public VoiceCall(callOffer: CallOfferDto)
  {
    return this._http.post(environment.BASE_URL + 'Message/voice-call' , callOffer).subscribe();
  }

  public mapRecievedMessage(message: MessageDto)
{
 
  this.receivedObject.isDelete= message.isDelete;
  
   this.sharedObj.next(this.receivedObject);

}

public retriveMappedObject(): Observable<MessageDto>
{
  console.log("mnb")
  return this.sharedObj.asObservable();
}

public mapRecievedVoiceOffer(callOffer: CallOfferDto)
{
 
  this.receivedCallOfferObject.callerId = callOffer.callerId;
  this.receivedCallOfferObject.calleeId = callOffer.calleeId;
   this.sharedCallOfferObj.next(this.receivedCallOfferObject);

}

public retriveMappedVoiceOfferObject(): Observable<CallOfferDto>
{
  console.log("mnb")
  return this.sharedCallOfferObj.asObservable();
}
}
