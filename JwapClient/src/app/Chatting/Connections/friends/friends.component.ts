import { ChatService } from './../../../../Services/chat.service';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ConnectionsDto } from 'src/app/Core/Models/connections-dto';
import { ChatDto } from 'src/app/Core/Models/chat-dto';
import { Subject } from 'rxjs';



@Component({
  selector: 'app-friends',
  templateUrl: './friends.component.html',
  styleUrls: ['./friends.component.css']
})
export class FriendsComponent implements OnInit {
  searchKey: string = '';
  
  friends: ConnectionsDto[] = [];
  @Output() reciveId = new EventEmitter<string>();
  @Output() chat = new EventEmitter<ChatDto>();

  constructor(private chatService: ChatService) { }

  ngOnInit(): void {
    this.GetFriends();
 
    this.chatService.retriveMappedConnection().subscribe(
      res =>{
     
        this.friends.map((x)=>
      {
        
        if(x.id == res.id )
        {
          x.online = res.online;
          
        }
       
      })
      
     
    }
     
   
    );
    this.chatService.retriveMappedLastMsg().subscribe(
      res =>{
      if(res.lastMessage != "")
      {
        console.log(this.friends)
        this.friends.map((x)=>
      {
        
        if(x.id == res.id || x.id == localStorage.getItem("recieverId") || localStorage.getItem('id') == localStorage.getItem("recieverId") )
        {
          x.lastMessage = res.lastMessage;
         console.log('rec' , localStorage.getItem("recieverId"),x.id)
        }
       
      })
      }
     
    }
     
   
    );
   
  }
 public GetFriends()
 {
  
  this.chatService.GetFriends().subscribe(
    res=>
    {
      
      this.friends = res;

    }
  );
 }
  public Search()
  {
    
    this.chatService.SearchForConnections(this.searchKey).subscribe(
      res=>
      {
        this.friends=res;
        
      }
    );
  }

  public GetMessages(id: string)
  {
       this.reciveId.emit(id)
       this.chatService.GetMessages(id).subscribe(
        res=>
        {
          console.log('chatfr', res)
          this.chat.emit(res);
          
        },
        err=>
        console.log("err" , err)
       );
      
  }
}
