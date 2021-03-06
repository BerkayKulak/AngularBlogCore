import {
  Component,
  OnInit,
  ViewChild,
  Output,
  EventEmitter,
} from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { CommentServiceService } from 'src/app/services/comment-service.service';
import { MyvalidationService } from 'src/app/services/myvalidation.service';

@Component({
  selector: 'app-add-comment',
  templateUrl: './add-comment.component.html',
  styleUrls: ['./add-comment.component.css'],
})
export class AddCommentComponent implements OnInit {
  commentForm: FormGroup;
  success: boolean;
  info: string;
  @Output() Reload: EventEmitter<string> = new EventEmitter();
  constructor(
    public commentService: CommentServiceService,
    private route: ActivatedRoute,
    public myvalidationService: MyvalidationService
  ) {}

  ngOnInit() {
    this.commentForm = new FormGroup({
      name: new FormControl('', Validators.required),
      contentMain: new FormControl('', Validators.required),
      articleId: new FormControl(''),
    });
  }

  get getControls() {
    return this.commentForm.controls;
  }

  onSubmit() {
    if (this.commentForm.valid) {
      let id = Number(this.route.snapshot.paramMap.get('id'));

      this.commentForm.controls.articleId.setValue(id);

      this.commentService.addComment(this.commentForm.value).subscribe(
        (data) => {
          console.log(data);
          this.success = true;
          this.info = 'Yorumunuz başarıyla eklenmiştir.';
          this.Reload.emit();
        },
        (error) => {
          console.log(error);
          this.success = false;
          this.info =
            'Bir hata meydana geldi. Lütfen daha sonra tekrar deneyiniz..';
        }
      );
    }
  }
}
