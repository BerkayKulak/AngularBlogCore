import { Injectable } from '@angular/core';
import { Comment } from '../models/comment';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class CommentServiceService {
  constructor(private httpClient: HttpClient) {}

  private apiUrl: string = 'https://localhost:44345/api/Comments';
  loading: boolean;
  addComment(comment: Comment) {
    this.loading = true;
    return this.httpClient.post(this.apiUrl, comment).pipe(
      tap((x) => {
        this.loading = false;
      })
    );
  }

  commentList(id: number) {
    return this.httpClient.get<Comment[]>(`${this.apiUrl}/${id}`);
  }
}
