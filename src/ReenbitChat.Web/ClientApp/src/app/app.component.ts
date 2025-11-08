import { Component, OnInit, ElementRef, ViewChild, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ChatService } from './services/chat.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit, AfterViewInit {
  @ViewChild('scrollContainer') private scrollContainer!: ElementRef

  room = 'general';
  user = localStorage.getItem('user') || 'Vasyl';
  text = '';

  constructor(public chat: ChatService) { }

  async ngOnInit() {
    await this.chat.start();
    await this.joinRoom(this.room);

    this.chat.messagesChanged.subscribe(() => this.scrollToBottom());
  }

  ngAfterViewInit() {
    this.scrollToBottom();
  }
  private scrollToBottom(): void {
    try {
      const el = this.scrollContainer?.nativeElement;
      el.scrollTop = el.scrollHeight;
    } catch (err) { }
  }

  async joinRoom(room: string) {
    console.log('Joining room:', room);

    this.chat.messages = [];

    await this.chat.join(room);

    const history = await this.chat.loadHistory(room);
    console.log('History loaded:', history);

    this.chat.messages = (history ?? []).reverse();
    this.scrollToBottom();
  }

  async send() {
    if (!this.text.trim()) return;
    await this.chat.send(this.room, this.user, this.text);
    this.text = '';
  }

  saveUser() {
    localStorage.setItem('user', this.user);
  }
}
