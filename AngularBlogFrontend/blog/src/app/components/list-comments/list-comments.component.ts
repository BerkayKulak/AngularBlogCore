import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Comment } from 'src/app/models/comment';
import { CommentServiceService } from 'src/app/services/comment-service.service';

@Component({
  selector: 'app-list-comments',
  templateUrl: './list-comments.component.html',
  styleUrls: ['./list-comments.component.css'],
})
export class ListCommentsComponent implements OnInit {
  comments: Comment[] = [];
  loading: boolean;

  constructor(
    private commentService: CommentServiceService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.loading = true;
    let id = Number(this.route.snapshot.paramMap.get('id'));

    this.commentService.commentList(id).subscribe((data) => {
      this.comments = data;
      this.loading = false;
    });
  }

  public reLoad() {
    this.loading = true;
    let id = Number(this.route.snapshot.paramMap.get('id'));

    this.commentService.commentList(id).subscribe((data) => {
      this.comments = data;
      this.loading = false;
    });
  }
}
