import { Component, inject, input, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TimeagoModule } from 'ngx-timeago';
import { Message } from 'src/app/_models/message';
import { MessageService } from 'src/app/_services/message.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-member-messages',
  standalone: true,
  imports: [TimeagoModule, FormsModule, CommonModule],
  templateUrl: './member-messages.component.html',
  styleUrl: './member-messages.component.css'
})
export class MemberMessagesComponent implements OnInit {
  private messageService = inject(MessageService);
  username = input.required<string>();
  messages = input.required<Message[]>();

  ngOnInit(): void {
  }
}
