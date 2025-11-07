import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { environment } from '../../environments/environment';
import { firstValueFrom } from 'rxjs';
console.log('API URL:', environment.apiUrl);
export interface MessageDto {
  id: string;
  userName: string;
  text: string;
  room: string;
  createdAtUtc: string;
  sentiment: number;
}

@Injectable({ providedIn: 'root' })
export class ChatService {
  private hub?: signalR.HubConnection;
  public messages: MessageDto[] = [];

  constructor(private http: HttpClient) { }

  start(baseUrl = environment.apiUrl) {
    this.hub = new signalR.HubConnectionBuilder()
      .withUrl(`${baseUrl}/hubs/chat`, { withCredentials: true })
      .withAutomaticReconnect()
      .build();

    this.hub.on('ReceiveMessage', (m: MessageDto) => {
      console.log('Message received:', m);
      this.messages.push(m);
    });

    this.hub.onreconnected(id => console.log('Reconnected:', id));
    this.hub.onreconnecting(err => console.warn('Reconnecting...', err));
    this.hub.onclose(err => console.log('Hub closed:', err));

    console.log('Connecting to SignalR...');
    return this.hub.start()
      .then(() => console.log('SignalR connected'))
      .catch(err => console.error('SignalR start error:', err));
  }

  join(room: string) {
    console.log('🟢 Joining room:', room);
    console.log('Join room', room);
    return this.hub!.invoke('JoinRoom', room);
  }

  send(room: string, userName: string, text: string) {
    console.log('🟢 sending message:', room);
    console.log('Sending:', { room, userName, text });
    return this.hub!.invoke('SendMessage', { room, userName, text });
  }

  async loadHistory(room: string): Promise<MessageDto[]> {
    const url = `${environment.apiUrl}/api/messages/history?room=${room}&take=50`;
    console.log('📡 Loading history from:', url);
    try {
      const data = await firstValueFrom(this.http.get<MessageDto[]>(url));
      return data ?? [];
    } catch (e) {
      console.error('loadHistory error:', e);
      return [];
    }
  }
}
