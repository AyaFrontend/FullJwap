
import { Component, ElementRef, EventEmitter, Input, OnDestroy, OnInit, Output, ViewChild } from '@angular/core';

import { MediaConnection, Peer } from "peerjs";
import { Subject, Subscription } from 'rxjs' ;
import { ChatService } from 'src/Services/chat.service';
import { CallOfferDto } from 'src/app/Core/Models/call-offer-dto';
import { ConnectionsDto } from 'src/app/Core/Models/connections-dto';


@Component({
  selector: 'app-video-call',
  templateUrl: './video-call.component.html',
  styleUrls: ['./video-call.component.css']
})
export class VideoCallComponent implements OnInit , OnDestroy{

 
 @Input() DisplayVideoCall: string = '';
 hiddenOpenVideoBtn: string = '';
 userStream! : MediaStream;
 remoteStream! : MediaStream;
 currentCall?: MediaConnection;
 @ViewChild('localVideo') localVideo!: ElementRef<HTMLVideoElement>;
 @ViewChild('remoteVideo') remoteVideo!: ElementRef<HTMLVideoElement>;
 @Input() myPeerId: string = '';
 @Input() remotePeerId: string = '';
 @Input() peer!: Peer;
 CallInfo: CallOfferDto = new CallOfferDto();
 friends: ConnectionsDto[] = [];
 call?: MediaConnection ;
 constraint: { audio: boolean, video: boolean} = {audio: true , video: true}
 @Output() peerEvent = new EventEmitter<string>();
  //remoteVideo: any = document.getElementById('remote-video');
  constructor(private _serv: ChatService ) { }
 
 
  ngOnInit(): void {
    
  
   
 
    this._serv.retriveMappedVideoOfferObject().subscribe((callOfferDetailes: CallOfferDto)=>
    {
       this.CallInfo = callOfferDetailes;
       if (!callOfferDetailes.callState && !callOfferDetailes.startCall &&(callOfferDetailes.callType == 'video')) 
       {
        this.endVideoCall();
        
       }
        if (callOfferDetailes.callState && (callOfferDetailes.callType == 'video')) {

        this.DisplayVideoCall = 'display: flex !important';
        this.remotePeerId = this.peer.id != callOfferDetailes.callerId? callOfferDetailes.callerId:callOfferDetailes.calleeId;
        if(callOfferDetailes.callerId == localStorage.getItem('currentUser'))
        {
          console.log(callOfferDetailes.callerId , localStorage.getItem('currentUser'))
          this.hiddenOpenVideoBtn = 'display: none !important';
        }
      }
     
    });
    
    this.peer.on('call', call=>
    {  
      console.log('oppeen')
      navigator.mediaDevices.getUserMedia(this.constraint).then(stream=>
        {
         this.userStream = stream;
          this.localVideo.nativeElement.srcObject = stream;
          this.localVideo.nativeElement.play();

           call.answer(stream);
           this.currentCall = call;

          this.remoteVideo.nativeElement.srcObject=stream;
          this.remoteVideo.nativeElement.play();
         
        
       
    
    })
    });
  }
  
  ngOnDestroy(): void {
  
  }
  openVideoCall()
  {
    

     navigator.mediaDevices.getUserMedia(this.constraint).then(stream=>
     {
      this.remoteStream = stream
      this.call =this.peer.call(this.remotePeerId , stream );
      
       this.call.on('stream', (remotestream:any)=>
       {
       
        this.remoteVideo.nativeElement.srcObject=remotestream;
        this.remoteVideo.nativeElement.play();

        this.localVideo.nativeElement.srcObject=remotestream;
        this.localVideo.nativeElement.play();
    //  
       });
      this.call.on('error', (err)=>
       {
        console.log('err', err);
       });
       /*call.on('close' , ()=>
       {
        this.constraint!.video = false;
        this.constraint!.audio = false;
         console.log('xxx')
        console.log('zzzz')
       })*/

      // this.currentCall = call;
     })
  }
  InvokCancelCall()
  {
    this._serv.InvokCancelCall(this.CallInfo)
  }
  endVideoCall()
  {
    this.DisplayVideoCall = 'display: none !important';
    console.log('userstream',this.userStream,this.userStream.getVideoTracks()[0])
    this.userStream!.getVideoTracks()[0]!.enabled = false;
    this.userStream!.getAudioTracks()[0]!.enabled = false;

 

   
           
      
 }
CreatePeerId(id: string)
{
   this.peer = new Peer(id)
  
 
}
  



}
