import { CommonModule, DatePipe } from '@angular/common';
import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { TabDirective, TabsetComponent, TabsModule } from 'ngx-bootstrap/tabs';
import { TimeagoModule } from 'ngx-timeago';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';
import { MemberMessagesComponent } from "../member-messages/member-messages.component";
import { Message } from 'src/app/_models/message';
import { MessageService } from 'src/app/_services/message.service';

@Component({
  selector: 'app-member-detail',
  standalone: true,
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css'],
  imports: [CommonModule, TabsModule, GalleryModule, TimeagoModule, DatePipe, MemberMessagesComponent],
})
export class MemberDetailComponent implements OnInit {
  @ViewChild('memberTabs') memberTabs?: TabsetComponent
  private messageService = inject(MessageService)
  private memberService = inject(MembersService)
  private route = inject(ActivatedRoute)
  member: Member | undefined;
  images: GalleryItem[] = [];
  activeTab?: TabDirective;
  messages: Message[] = [];

  ngOnInit(): void {
    this.loadMember();
  }

  onTabActivated(data: TabDirective) {
    this.activeTab = data;
    if (this.activeTab.heading === 'Messages' && this.messages.length === 0 && this.member) {
      this.messageService.getMessageThread(this.member.userName).subscribe({
        next: messages => this.messages = messages
      })
    }
  }

  loadMember() {
    const username = this.route.snapshot.paramMap.get('username');
    if (!username) return;
    this.memberService.getMember(username).subscribe({
      next: (member) => {
        this.member = member;
        member.photos.map((p) => {
          this.images.push(new ImageItem({ src: p.url, thumb: p.url }));
        });
      },
    });
  }

  getImages() {
    if (!this.member) return;
    for (const photos of this.member.photos) {
      this.images.push(new ImageItem({ src: photos.url, thumb: photos.url }));
    }
  }
}
