import {
  Component,
  inject,
  Input,
  OnInit,
  EventEmitter,
  Output,
} from '@angular/core';
import { FileUploader, FileUploadModule } from 'ng2-file-upload';
import { Member } from 'src/app/_models/member';
import { Photo } from 'src/app/_models/photo';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';
import { environment } from 'src/environments/environment.development';
import { NgIf, NgFor, NgClass, NgStyle, DecimalPipe } from '@angular/common';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css'],
  standalone: true,
  imports: [NgIf, NgFor, NgClass, FileUploadModule, NgStyle, DecimalPipe],
})
export class PhotoEditorComponent implements OnInit {
  private accountService = inject(AccountService);
  @Input() member: Member | undefined;
  @Output() memberChange = new EventEmitter<Member>();
  uploader: FileUploader | undefined;
  hasBaseDropZoneOver = false;
  baseUrl = environment.apiUrl;
  user = this.accountService.currentUser();

  constructor(private memberService: MembersService) {}

  ngOnInit(): void {
    this.initializeUploader();
  }

  fileOverBase(e: any) {
    this.hasBaseDropZoneOver = e;
  }

  deletePhoto(photo: Photo) {
    this.memberService.deletePhotos(photo.id).subscribe({
      next: (_) => {
        const updatedMember: Member = { ...this.member! };
        updatedMember.photos =
          updatedMember.photos?.filter((x) => x.id !== photo.id) ?? [];
        this.memberChange.emit(updatedMember);
      },
    });
  }

  setMainPhoto(photo: Photo) {
    this.memberService.setMainPhoto(photo.id).subscribe({
      next: () => {
        const user = this.accountService.currentUser();
        if (user) {
          user.photoUrl = photo.url;
          this.accountService.setCurrentUser(user);
        }
        const updatedMember: Member = {
          ...this.member!,
          photos: this.member?.photos ?? [],
        };
        updatedMember.photoUrl = photo.url;
        updatedMember.photos.forEach((p) => {
          if (p.isMain) p.isMain = false;
          if (p.id === photo.id) p.isMain = true;
        });
        this.memberChange.emit(updatedMember);
      },
    });
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'users/add-photo',
      authToken: 'Bearer ' + this.user?.token,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024,
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    };

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      const photo = JSON.parse(response);
      const updatedMember: Member = { ...this.member! };
      updatedMember.photos.push(photo);
      this.memberChange.emit(updatedMember);
      if (photo.isMain) {
        const user = this.accountService.currentUser();
        if (user) {
          user.photoUrl = photo.url;
          this.accountService.setCurrentUser(user);
        }
      }
      updatedMember.photoUrl = photo.url;
      updatedMember.photos.forEach((p) => {
        if (p.isMain) p.isMain = false;
        if (p.id === photo.id) p.isMain = true;
      });
      this.memberChange.emit(updatedMember);
    };
  }
}
