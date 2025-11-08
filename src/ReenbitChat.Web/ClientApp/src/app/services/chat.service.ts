import { HttpClient } from '@angular/common/http';
import { Injectable, EventEmitter } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { environment } from '../../environments/environment';
import { firstValueFrom } from 'rxjs';

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
      this.messages.push(m);
      this.messagesChanged.emit();
    });

    return this.hub.start();
  }

  async connect(): Promise<void> {
    if (!this.hub) {
      this.hub = new signalR.HubConnectionBuilder()
        .withUrl(environment.signalrHubUrl, { withCredentials: true })
        .withAutomaticReconnect()
        .build();
    }

    if (this.hub.state !== signalR.HubConnectionState.Connected) {
      await this.hub.start();
    }
  }

  async join(room: string) {
    await this.connect();
    return this.hub!.invoke('JoinRoom', room);
  }

  send(room: string, userName: string, text: string) {
    return this.hub!.invoke('SendMessage', { room, userName, text });
  }

  async loadHistory(room: string): Promise<MessageDto[]> {
    const url = `${environment.apiUrl}/api/messages?room=${room}&take=50`;
    try {
      const data = await firstValueFrom(this.http.get<MessageDto[]>(url));
      return data ?? [];
    } catch {
      return [];
    }
  }
}
