
export class MessageDto {
    
 public senderId: string = '';
 public recieverId: string ='';
 public id: string ='';
 public messege: string = '' ;
 public messegeType: string ='';
 public sendDate: Date = new Date();
 public isRead: boolean = false;
 public isDelete:boolean = false;
 public connectionId: string ="";
 public audioUrl: string =""; 
 public audio!: Blob | File ;
}


