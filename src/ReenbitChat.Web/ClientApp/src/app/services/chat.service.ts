import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';

export interface MessageDto {
  id: string, userName: string, text: string, room: string, createdAtUtc: string, sentiment: number;
}
@Injectable({ providedIn: 'root' })
export class ChatService {
  private hub?: signalR.HubConnection;
  public messages: MessageDto[] = [];

  start(baseUrl = 'http://localhost:5119') {
    this.hub = new signalR.HubConnectionBuilder()
      .withUrl('${baseUrl}/hubs/chat', { withCredentials: true })
      .withAutomaticReconnect()
      .build();

    this.hub.on('ReceiveMessage', (m: MessageDto) => {
      this.messages.push(m);
    });

    return this.hub.start();
  }

  join(room: string) { return this.hub!.invoke('JoinRoom', room); }
  send(room: string, userName: string, text: string) {
    return this.hub!.invoke('SendMessage', { room, userName, text });
  }
}
