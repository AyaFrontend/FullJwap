import { ConnectionsDto } from './../app/Core/Models/connections-dto';

import { MessageDto } from './../Dto/message-dto';
import { environment } from './../environments/environment';

import {Injectable } from '@angular/core';  
import * as signalR from '@microsoft/signalr';
import { Observable, Subject, Subscription } from 'rxjs';
import { HttpClient } from '@angular/common/http';

import { ChatDto } from 'src/app/Core/Models/chat-dto';
import { OnlineConnectionsDto } from 'src/app/Core/Models/OnlineConnectionsDto';

import { CallOfferDto } from 'src/app/Core/Models/call-offer-dto';

import { UserDto } from 'src/app/Core/Models/user-dto';





@Injectable({
  providedIn: 'root'
})
export class ChatService {

  
  private sharedCallOfferObj = new Subject<CallOfferDto>() ;
  private receivedCallOfferObject : CallOfferDto = new CallOfferDto();
  public sharedBlob = new Subject<Blob>();
  public reciveId = new Subject<string>();
  private receivedObject : MessageDto = new MessageDto();
  private sharedObj = new Subject<MessageDto>() ;
  private userDtoObject! : UserDto ;
  private sharedUserDtoObj = new Subject<UserDto>() ;
  private UserConnectionId: string = '';
  private sharedConnection = new Subject<ConnectionsDto>();
  private connectionDto: ConnectionsDto = new ConnectionsDto();
  private con: OnlineConnectionsDto= new OnlineConnectionsDto();
  private connection: signalR.HubConnection =  new signalR.HubConnectionBuilder()
  .withUrl(`https://localhost:44399/chatsocket?token=${localStorage.getItem('jwt-token')}`,
  {
    skipNegotiation: false,
    transport: signalR.HttpTransportType.WebSockets
  } ).withAutomaticReconnect().build();



  constructor(private http: HttpClient) {
    this.connection.onclose(async() =>
    {
      
     
      await this.Start()
    }); 
  
    this.Start();
    this.RegisteredEvents();
   }

   
//Start() function
public async Start()
{
  try{
   await this.connection.start();
   this.UserConnectionId = this.connection.connectionId!;
   this.AddConnection();
  }
  catch(err)
  {
    setTimeout(() => {
      this.Start();
    }, 5000);
  }
}




//retreve maped object
public retriveMappedCall(): Observable<Blob>
{
  return this.sharedBlob.asObservable();
}
public retriveMappedConnection() : Observable<ConnectionsDto>
{
  
  return this.sharedConnection.asObservable();
}
public retriveMappedLastMsg() : Observable<ConnectionsDto>
{
  
  return this.sharedConnection.asObservable();
}
public retriveMappedObject(): Observable<MessageDto>
{
  return this.sharedObj.asObservable();
}

public retriveMappedObjectForDelete(): Observable<MessageDto>
{
  return this.sharedObj.asObservable();
}
public retriveMappedVoiceOfferObject(): Observable<CallOfferDto>
{
  
  return this.sharedCallOfferObj.asObservable() as Observable<CallOfferDto>;
}

public retriveMappedVideoOfferObject(): Observable<CallOfferDto>
{
  
  return this.sharedCallOfferObj.asObservable() as Observable<CallOfferDto>;
}
public retriveMappedUserDtoObject(): Observable<UserDto>
{
  
  return this.sharedUserDtoObj.asObservable() as Observable<UserDto>;
}
//mapped
public mapUser(userDto: UserDto)
{
  this.userDtoObject.id = userDto.id;
  this.userDtoObject.profilePicture = userDto.profilePicture;
  
  this.sharedUserDtoObj.next(this.userDtoObject);
}
public mapRecievedVoiceOffer(callOffer: CallOfferDto)
{
 
  this.receivedCallOfferObject.callerId = callOffer.callerId;
  this.receivedCallOfferObject.calleeId = callOffer.calleeId;
  this.receivedCallOfferObject.callState = callOffer.callState;
  this.receivedCallOfferObject.startCall = callOffer.startCall;
  this.receivedCallOfferObject.callType = callOffer.callType;
  this.receivedCallOfferObject.id = callOffer.id;
  this.receivedCallOfferObject.calleeName = callOffer.calleeName;
  this.receivedCallOfferObject.calleePic = callOffer.calleePic;
  this.receivedCallOfferObject.callerName = callOffer.callerName;
  this.receivedCallOfferObject.callerPic = callOffer.callerPic;
   this.sharedCallOfferObj.next(this.receivedCallOfferObject);


}
public mapRecievedCall(blob: Blob)
{
  
  this.sharedBlob.next(blob);
}

public MappedConnection(status: boolean , userId: string)
{
   this.connectionDto.online = status;
   this.connectionDto.id = userId;
   this.sharedConnection.next(this.connectionDto);
 
}


public MappedLastMsg(message: string , userId: string)
{
  
   this.connectionDto.lastMessage = message;
   this.connectionDto.id = userId;

   localStorage.setItem('id' ,userId)
   this.sharedConnection.next(this.connectionDto);
 
}


public mapRecievedMessage(message: MessageDto)
{
  
  this.receivedObject.id = message.id;
  this.receivedObject.messege = message.messege;
  this.receivedObject.senderId = message.senderId;
  this.receivedObject.recieverId = message.recieverId;
  this.receivedObject.sendDate = message.sendDate;
  this.receivedObject.messegeType = message.messegeType;
  this.receivedObject.audioUrl = message.audioUrl;
  this.receivedObject.isDelete = message.isDelete;

   this.sharedObj.next(this.receivedObject);

}
///end points
//Add Connection
public AddConnection()
{
  this.con.UserConnectionId = this.UserConnectionId;
  this.http.post( `${environment.BASE_URL}Connections/add-connection` , this.con).subscribe()
}
//Delete Connection
public async DeleteConnection()
{
  this.con.UserConnectionId = this.UserConnectionId;
  this.http.post( `${environment.BASE_URL}Connections/delete-connection` , this.con).subscribe(
   
  )
}

//send method
public send(message:FormData ) : Observable<MessageDto>
{
  //message.connectionId = this.UserConnectionId;
  console.log('data', message)
 
  return this.http.post(`${environment.BASE_URL}Chat/send` , message ) as Observable<MessageDto> ;
}
//change user image
public ChangeImage(formData: FormData) :Observable<UserDto>
{
  console.log('i am here')
  return this.http.post(`${environment.BASE_URL}Settings/change-profile` , formData) as Observable<UserDto>;
}
public getConnectionId(): string
{
    return this.connection.connectionId!;
}

//search for connections connections
public SearchForConnections(searchKey: string) : Observable<ConnectionsDto[]>
{
  
   return this.http.get( environment.BASE_URL + `Users/getUser/${searchKey}` ) as Observable<ConnectionsDto[]>;
}
///Get Friends 
public GetFriends() : Observable<ConnectionsDto[]>
{

  return this.http.get( environment.BASE_URL + `Users/getFriends` ) as Observable<ConnectionsDto[]>;
 
    
}

// Get Messages with specfic friend
public GetMessages(ReciverId: string): Observable<ChatDto>
{
 
  return this.http.get( environment.BASE_URL + `Chat/getChat/${ReciverId}` ) as Observable<ChatDto>;
}

///Invoke
public InvokCancelCall(callOffer: CallOfferDto)
{
  this.connection.invoke('CanceledCall',callOffer).catch(err=> console.log('there is an err in cancel call' , err))
}

public InvokVoiceCall(callOffer: CallOfferDto)
{
  this.connection.invoke('VoiceCall',callOffer).catch(err=> console.log('there is an err in voce call' , err))
}
public InvokTyping(reciverId: string)
{
  this.connection.invoke('Typing',reciverId).catch(err=> console.log('there is an err in voce call' , err))
}

public InvokCall(callInfo: CallOfferDto , size: number , type: string)
{
  this.connection.invoke('InCalling' , callInfo , size, type).catch(err=> console.log('there is an err in incalling' , err))
}

public InvokDeleteMessage(msg: MessageDto)
{
  this.connection.invoke('DeleteMessageAsync',msg).catch(err=> console.log('there is an err in remove msg' , err))
}

public InvokStartCall(callOffer: CallOfferDto)
{
  this.connection.invoke('CallStart',callOffer).catch(err=> console.log('there is an err in start call' , err))
}
public InvokeStartVideo(callOffer: CallOfferDto)
{   console.log('open')
  this.connection.invoke('StartVideo', callOffer).catch(err=> console.log('there is an err in start call' , err))
}
public InvokevideoCall(callOffer: CallOfferDto)
{
  this.connection.invoke('videoCall',callOffer).catch(err=> console.log('there is an err in start call' , err))
}
//registerd Events
RegisteredEvents()
{
  this.connection.on("Call",(callOffer: CallOfferDto)=>{
     
    this.mapRecievedVoiceOffer(callOffer);
    
  });

  this.connection.on("ChangeProfileImage",(userDto: UserDto)=>{
    this.mapUser(userDto);
  });
  this.connection.on("VideoCall",(callOffer: CallOfferDto)=>{
     
    this.mapRecievedVoiceOffer(callOffer);
    
  });
  this.connection.on("InCall",(blob: Blob)=>{
   
    this.mapRecievedCall(blob);
    
  });
  this.connection.on("Cancel",(callOffer: CallOfferDto)=>{
   
    this.mapRecievedVoiceOffer(callOffer);
    console.log('canceling')
  });
  this.connection.on("CallStart",(callOffer: CallOfferDto)=>{
   
    this.mapRecievedVoiceOffer(callOffer);
  });
  this.connection.on("VideoStart",(callOffer: CallOfferDto)=>{
    console.log('open')
    this.mapRecievedVoiceOffer(callOffer);
  });
  this.connection.on("BtroadcastMessage",(message)=> {this.mapRecievedMessage(message);
  
  });
 

 this.connection.on("GetStatus" , (status , userId)=>
 {  
    this.MappedConnection(status , userId);
      
 });
 
 this.connection.on("LastMessage" , (message , userId)=>
 {  
    this.MappedLastMsg(message , userId);
      
 });
 this.connection.on("DeleteMessage", (message : MessageDto)=>
 {
  
  this.mapRecievedMessage(message)
  
 });
}
}
