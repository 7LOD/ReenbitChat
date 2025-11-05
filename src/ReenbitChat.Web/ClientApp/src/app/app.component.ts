import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ChatService } from './services/chat.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
  room = 'general';
  user = localStorage.getItem('user') || 'Vasyl';
  text = '';

  constructor(public chat: ChatService) { }

  async ngOnInit() {
    await this.chat.start();
    await this.chat.join(this.room);
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

