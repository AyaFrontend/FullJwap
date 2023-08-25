import { MessageDto } from 'src/Dto/message-dto';
export interface ChatDto {
    senderId?: string ;
    receiverId?: string;
    messages?: MessageDto[]; 
}
