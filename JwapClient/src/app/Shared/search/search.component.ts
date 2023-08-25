import { ConnectionsDto } from './../../Core/Models/connections-dto';
import { ChatService } from './../../../Services/chat.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {


  constructor(private chatService: ChatService) { }

  ngOnInit(): void {

  }


}
