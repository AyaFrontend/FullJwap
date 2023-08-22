import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { Subscription } from 'rxjs';
import { ChatService } from 'src/Services/chat.service';
import { CallOfferDto } from 'src/app/Core/Models/call-offer-dto';

@Component({
  selector: 'app-calling',
  templateUrl: './calling.component.html',
  styleUrls: ['./calling.component.css']
})
export class CallingComponent implements OnInit , OnDestroy{
    sub1: Subscription = new Subscription();
    sub2: Subscription = new Subscription();
    sub3: Subscription = new Subscription();
    sub4 : Subscription = new Subscription();
  private mediaRecorder?: MediaRecorder;
  private audioChunks: any[] = [];
  private blob: Blob = new Blob();
  private currentBlob: Blob = new Blob();
  private base64AudioMessage:string ='';
  recording = false;
  startRecording = false;

 
  //URL of Blob
  public url:any;

  @Input() Display: string ='';
  @Input() none: string ='';
  @Input() path: string ='';
  @Input() name: string ='';
  @Input() CallInfo: CallOfferDto = new CallOfferDto();
  constraint: { audio: boolean} | undefined;
  stream!: MediaStream
 
  constructor(private _serv: ChatService , private domSanitizer: DomSanitizer) { }
  ngOnDestroy(): void {
     this.sub1.unsubscribe();
     this.sub2.unsubscribe();
     this.sub3.unsubscribe();
     this.sub4.unsubscribe();
  }
  sanitize(url: string) {
    return this.domSanitizer.bypassSecurityTrustUrl(url);
    }

  ngOnInit(): void {
   this.sub1 =  this._serv.retriveMappedVoiceOfferObject().subscribe((callOffer: CallOfferDto)=>
    {
     
      if(!callOffer.startCall && this.recording && callOffer.callType=="voice")
      {
        
          this.stop();
          this.Display = 'display: none !important';
          this.constraint!.audio = false; 
          this.recording = false;  
         
         
      }
    });
    this.sub2=  this._serv.retriveMappedVoiceOfferObject().subscribe((callOffer: CallOfferDto)=>
    {
      
      if(callOffer.startCall && !this.recording&& callOffer.callType=="voice")
      {
        
         this.start();
       
      }
    });
  
    this.sub3 =  this._serv.retriveMappedVoiceOfferObject().subscribe(res=>
    {
  
      if(!res.callState && res.callType=="voice")
      {
         this.Display = 'display:none !important';
      }
   });

   this.sub4=this._serv.retriveMappedCall().subscribe((blobbing :Blob)=>
        {
       if(this.recording)
       {
          if(blobbing != null)
          {
            // this.blob = blobbing;
            console.log('this.currentBlob',this.currentBlob)
             this.playBlob(this.currentBlob)
           
          }
        
       }
      
        });
        
       
  }
  Minimize(poss: HTMLDivElement)
  {
    poss.classList.remove('top-0', 'start-0');
    poss.classList.add('top-50' , 'w-25','fade-in')
   
  }


  Maximize(poss: HTMLDivElement)
  {
    poss.classList.remove('top-50' , 'w-25');
    poss.classList.add('top-0', 'start-0')
    
  }

  async start()
  {
    this.recording = true;
    this.constraint = {
      audio : true 

    }
    navigator.mediaDevices.getUserMedia(this.constraint).then(stream=>
      {
        this.stream = stream;
        this.mediaRecorder = new MediaRecorder(stream);
      if(this.recording)
      {
          this.mediaRecorder.start(5000);
      

      
        this.mediaRecorder.addEventListener('dataavailable', async (event: { data: any; })=>
        {
       
          this.audioChunks[0] = await event.data;
          let blob =   new  Blob(this.audioChunks , {type:'audio/webm;codecs=opus'}) 
          
          await this.Send(blob);
        
          
        });

      } 
      });
  }

  async OpenCall(element: HTMLLIElement)
  {
 //.startCa  
   this.none = 'display: none !important'
   await this._serv.InvokStartCall(this.CallInfo);

  }

 

  async playBlob(blob: Blob)
  {
    try{
    this.url = URL.createObjectURL(blob);

    let audio = new Audio(this.url)
  
   // audio.load();
    audio.autoplay = true;
    await audio.play().then(res=>{}).catch(err=>{});
   
   this.stop();
   //this.start() 
   //this.mediaRecorder?.stop();
   this.mediaRecorder?.start();
  }
  catch(err: any)
  {
    console.log('err',err)
  }

  }
  async stop()
  {
    if(this.mediaRecorder?.state == 'recording'){
      this.stream.getAudioTracks()[0].enabled = false;
      await  this.mediaRecorder?.stop();
      
      
    }
  }
async Send(blob: Blob)
{
 // const formData = new FormData();
 // formData.append("file" , blob , "blob.webm");
 // formData.append("callInfo",JSON.stringify(this.CallInfo))
  this.currentBlob = blob;
  
  if(this.recording)
       await this._serv.InvokCall(this.CallInfo , blob.size , blob.type);
 
}
  async closeCall()
  {
  
  await this._serv.InvokCancelCall(this.CallInfo)
   
    
    
  }
}
