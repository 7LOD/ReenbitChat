import { HttpClient } from '@angular/common/http';
import { Injectable, EventEmitter } from '@angular/core';
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
  isSystem: boolean;
}

@Injectable({ providedIn: 'root' })
export class ChatService {
  private hub?: signalR.HubConnection;
  public messages: MessageDto[] = [];

  public messagesChanged = new EventEmitter<void>();

  constructor(private http: HttpClient) { }

  start() {
    this.hub = new signalR.HubConnectionBuilder()
      .withUrl(environment.signalrHubUrl, { withCredentials: true })
      .withAutomaticReconnect()
      .build();

    this.hub.on('ReceiveMessage', (m: MessageDto) => {
      console.log('Message received:', m);
      this.messages.push(m);
      this.messagesChanged.emit();
    });

    this.hub.onreconnected(id => console.log('Reconnected:', id));
    this.hub.onreconnecting(err => console.warn('Reconnecting...', err));
    this.hub.onclose(err => console.log('Hub closed:', err));

    console.log('Connecting to SignalR...');
    return this.hub.start()
      .then(() => console.log('SignalR connected'))
      .catch(err => console.error('SignalR start error:', err));
  }
  async connect(): Promise<void> {
    if (!this.hub) {
      this.hub = new signalR.HubConnectionBuilder()
        .withUrl(environment.signalrHubUrl, { withCredentials: true })
        .withAutomaticReconnect()
        .build();
    }
    if (this.hub.state !== signalR.HubConnectionState.Connected) {
      try {
        await this.hub.start();
        console.log('✅ Connected to hub');
      } catch (err) {
        console.error('❌ Hub connection failed:', err);
      }
    }
  }

  async join(room: string) {
    await this.connect(); 
    console.log('🟢 Joining room:', room);
    return this.hub!.invoke('JoinRoom', room);
  }

  send(room: string, userName: string, text: string) {
    console.log('🟢 sending message:', room);
    console.log('Sending:', { room, userName, text });
    return this.hub!.invoke('SendMessage', { room, userName, text });
  }

  async loadHistory(room: string): Promise<MessageDto[]> {
    const url = `${environment.apiUrl}/api/messages?room=${room}&take=50`;
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
