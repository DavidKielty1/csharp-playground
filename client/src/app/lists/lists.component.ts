import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { LikesService } from '../_services/likes.service';
import { Member } from '../_models/member';
import { FormsModule } from '@angular/forms';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { MemberCardComponent } from "../members/member-card/member-card.component";
import { PaginationModule } from 'ngx-bootstrap/pagination';

@Component({
    selector: 'app-lists',
    templateUrl: './lists.component.html',
    styleUrls: ['./lists.component.css'],
    standalone: true,
    imports: [FormsModule, ButtonsModule, MemberCardComponent, PaginationModule]
})
export class ListsComponent implements OnInit, OnDestroy {
    likeService = inject(LikesService)
    predicate = 'liked'
    pageNumber = 1
    pageSize = 5

    ngOnInit(): void {
        this.loadLikes()
    }

    getTitle() {
        switch (this.predicate) {
            case 'liked':
                return 'Members you liked'
            case 'likedBy':
                return 'Members who liked you'
            default:
                return 'Mutual likes'
        }
    }

    loadLikes() {
        this.likeService.getLikes(this.predicate, this.pageNumber, this.pageSize)
    }

    pageChanged(event: any) {
        if (this.pageNumber !== event.page) {
            this.pageNumber = event.page
            this.loadLikes()
        }
    }
    
    ngOnDestroy(): void {
        this.likeService.paginatedResult.set(null)
    }

}
