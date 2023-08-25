import { ConnectionsDto } from './../../Core/Models/connections-dto';
import { CallOfferDto } from 'src/app/Core/Models/call-offer-dto';

import { Subject, Observable, Subscription } from 'rxjs';
import { ChatDto } from './../../Core/Models/chat-dto';
import { Component, OnDestroy, OnInit} from '@angular/core';
import { MessageDto } from 'src/Dto/message-dto';
import { ChatService } from 'src/Services/chat.service';
import { DomSanitizer } from '@angular/platform-browser';
import { MessagesService } from 'src/app/Core/Services/messages.service';
import Peer from 'peerjs';
import { FormControl, FormGroup } from '@angular/forms';
import { UserDto } from 'src/app/Core/Models/user-dto';




@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit , OnDestroy {
  myPeerId: string = '';
  remotePeerId: string = '';
  peer! :Peer;
  backgroundPicPath: string ='';
  userName: string ='';
  profilePicPath: string =''
  private sub1 : Subscription =new Subscription();
  private sub2 : Subscription = new Subscription()
  private mediaRecorder?: MediaRecorder;
  private audioChunks: any[] = [];
  private blob: Blob = new Blob();
  private base64AudioMessage:string ='';
  recording = false;

  //URL of Blob
  url:any;
  callOfferDto: CallOfferDto = new CallOfferDto();
  //lldisplayVideo: Subject<string> = new Subject<string>();
  displayVideo: string = 'display:none !important';
  display: string = 'display:none !important';
  displayNone: string = 'display:none !important';
  displayNoneCam: string = 'display:none !important';
  
  //Will use this flag for toggeling recording
 
  error:any;

  messages: MessageDto[] = [];
  messageDto: MessageDto = new MessageDto();
  title: any;
  chat!: ChatDto;
  none: any = ' display: none'
  //message: MessageDto = new MessageDto;

 constructor(private domSanitizer: DomSanitizer , private _serv: ChatService,
  private _servMsg: MessagesService) {
    this.profilePicPath =  localStorage.getItem('profilePic')!;
   
  }
  ngOnDestroy(): void {
    this.sub1.unsubscribe();
    this.sub2.unsubscribe();
  }

  sanitize(url: string) {
  return this.domSanitizer.bypassSecurityTrustUrl(url);
  }

  ngOnInit(): void {
    this.peer = new Peer(localStorage.getItem('currentUser')!)
    this.myPeerId = this.peer.id;
    
    this.peer.on('connection' , function(conn)
    {
      conn.on('data' , data=>
      {
        console.log('data' ,data)
      })
    })
    this.sub1 = this._serv.retriveMappedVoiceOfferObject().subscribe((recieveObj: CallOfferDto) => {

      if (recieveObj.callState && (recieveObj.callType == 'voice')) {
        this.callOfferDto = recieveObj;
        
      if(localStorage.getItem('currentUser') == recieveObj.callerId)
      {
        this.backgroundPicPath = recieveObj.calleePic;
        this.userName = recieveObj.calleeName;
      }
      else{
        this.backgroundPicPath = recieveObj.callerPic;
        this.userName = recieveObj.callerName;
      }
        this.display = 'display: flex !important';
      }

      
      if (localStorage.getItem('currentUser') == recieveObj.callerId) {
           this.displayNone = 'display: none !important';

      }
      if (localStorage.getItem('currentUser') != recieveObj.callerId) {
        this.displayNone = 'display: flex !important';

      }
    });
    this.sub2 = this._serv.retriveMappedObjectForDelete().subscribe((receivedObj: MessageDto) => {
      if (receivedObj.isDelete) {


        this.chat.messages?.map(msg => {

          if (msg.id == receivedObj.id) {

            msg.isDelete = true;

          }
        })


      }

  
          
   });

    this._serv.retriveMappedObject().subscribe( (receivedObj: MessageDto) => 
    { 
      if(!receivedObj.isDelete)
          this.addToInbox( receivedObj);
    
     
    }); 

   
   
  
  }

Send(file: any)
{
 // this.messageDto.messageType = messageType;
  const formData = new FormData();

  if(typeof file !== 'string' ) 
  {
   
    formData.append("file" , file , file.name?? "file.webm");
    console.log(formData)
  }
 else{
  this.messageDto.messegeType = "text";
 }

  formData.append("message",JSON.stringify(this.messageDto))
  this._serv.send(formData).subscribe();
  
}
reciveID(eventdata: string)
{
  this.messageDto.recieverId = eventdata;

}
  addToInbox(mes: MessageDto)
  {
     
    
     let newObj = new MessageDto();
     newObj.messege = mes.messege;
     newObj.recieverId = mes.recieverId;
     newObj.audioUrl = mes.audioUrl;
     newObj.messegeType = mes.messegeType; 
     newObj.id = mes.id;
     newObj.senderId = mes.senderId;
    localStorage.setItem("recieverId" , mes.recieverId);
    // this.messages.push(newObj);
    this.chat.messages?.push(newObj)
    this.messageDto.messege="";
  }
LoadChat(eventData: ChatDto)
{
  this.chat = eventData;
 
}


start()
  {
    this.recording = true;
    let constraint = {
      audio : true 

    }
    navigator.mediaDevices.getUserMedia(constraint).then(stream=>
      {
        this.mediaRecorder = new MediaRecorder(stream);
        this.mediaRecorder.start();
       
        
        //this.SaveChunks(this.mediaRecorder);
        this.mediaRecorder.addEventListener('dataavailable', (event: { data: any; })=>
        {
          this.audioChunks.push(event.data);
          
        })

        
      });
  }
  
  SaveChunks(mediaRecorder: MediaRecorder)
  {
    mediaRecorder.addEventListener('dataavailable', event=>
    {
      this.audioChunks.push(event.data);
  
    })
  }

  stop()
  {
    
    this.recording = false;
    this.mediaRecorder?.stop();
     
   
    this.audioBlob(this.mediaRecorder!)
   
   
  }

  audioBlob(mediaRecorder: MediaRecorder)
  {
   
    mediaRecorder.addEventListener("stop",async()=>
    {
     this.blob =  await new  Blob(this.audioChunks , {type:'audio/webm;codecs=opus'}) 
     this.save(this.blob , "audio");
   
    })
   
  }

  playBlob(blob: Blob)
  {
    this.url = URL.createObjectURL(blob);
    let audio = new Audio(this.url);
    
    audio.play();
    
  
  }
  save(file: any , messageType: string)
  {
    

    this.messageDto.connectionId = this._serv.getConnectionId();
    this.messageDto.audio = file;
    this.messageDto.messegeType = messageType;
    this.Send(file);
    
   }
   
  UploadImage(data: any)
  {
    const file: File = data.files[0];
    const form = new FormData();

    form.append('file' , file);
    this._serv.ChangeImage(form).subscribe(res=>
      {
         this.profilePicPath = res.profilePicture;
         localStorage.setItem('profilePic',res.profilePicture);
      });
  }

   UploadFile(data: any)
   {
    const file: File = data.files[0];
    this.save(file, file.type)
   }
  
   typing(recieverId: string)
   {
    this._serv.InvokTyping(recieverId);
   }

   Display(del: HTMLDivElement)
   {
   // this.none = ' display: none'
    del.style.display = "block"
   }
   Hidden(del: HTMLDivElement)
   {
   // this.none = ' display: none'
    del.style.display = "none"
   }
   Delete( msg: MessageDto)
   {
    // this._serv.DeleteMsg(msg);
     this._serv.InvokDeleteMessage(msg);
   }
   voiceCall(senderId: string ,recieverId: string  )
   {console.log(senderId,recieverId)

  
    this.callOfferDto.callerId = senderId;
    this.callOfferDto.calleeId = recieverId;
    //this._serv.VoiceCall(this.callOfferDto);
    this._serv.InvokVoiceCall(this.callOfferDto);
   }

   videoCall(senderId: string ,recieverId: string  )
   {
    

   
     this.remotePeerId = localStorage.getItem('currentUser') != recieverId? recieverId:senderId;
     let conn = this.peer.connect(this.remotePeerId);
     conn.on('open', ()=>
     {
      conn.send('hi');
     })
    this.callOfferDto.callerId = senderId;
    this.callOfferDto.calleeId = recieverId;
  
   this._serv.InvokevideoCall(this.callOfferDto);
   }
   
   
  }





