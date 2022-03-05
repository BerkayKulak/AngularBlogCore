import { Component, OnInit } from '@angular/core';
import {
  FormControl,
  FormGroup,
  Validators,
  AbstractControl,
} from '@angular/forms';
import { Router } from '@angular/router';
import { Category } from 'src/app/models/category';
import { ArticleService } from 'src/app/services/article.service';
import { CategoryService } from 'src/app/services/category.service';
import { MyvalidationService } from 'src/app/services/myvalidation.service';
import * as DecoupledEditor from '@ckeditor/ckeditor5-build-decoupled-document';

@Component({
  selector: 'app-article-add',
  templateUrl: './article-add.component.html',
  styleUrls: ['./article-add.component.css'],
})
export class ArticleAddComponent implements OnInit {
  public Editor = DecoupledEditor;
  public onReady(editor) {
    editor.ui
      .getEditableElement()
      .parentElement.insertBefore(
        editor.ui.view.toolbar.element,
        editor.ui.getEditableElement()
      );
  }
  fileData: File = null;
  picture: string = null;
  articleForm: FormGroup;
  success: boolean;
  loading: boolean;
  info: string;
  categories: Category[];

  constructor(
    private articleService: ArticleService,
    private categoryService: CategoryService,
    private router: Router,
    public myvalidationService: MyvalidationService
  ) {}

  ngOnInit(): void {
    this.getCategory();

    this.articleForm = new FormGroup({
      title: new FormControl('makale 1', Validators.required),
      contentSummary: new FormControl('makale özet 1', Validators.required),
      contentMain: new FormControl('', Validators.required),
      category: new FormControl('', Validators.required),
      picture: new FormControl(''),
    });
  }

  onSubmit() {
    if (this.articleForm.valid) {
      this.loading = true;
      this.articleService.addArticle(this.articleForm.value).subscribe(
        (data) => {
          this.success = true;
          // alert("geldi");
          this.router.navigateByUrl('/admin/makale/liste');
        },
        (error) => {
          this.success = false;
          this.info = 'bir hata meydana geldi:';
          console.log(error);
        }
      );
    }
  }

  displayCategoryName(category) {
    return category.name;
  }

  getCategory() {
    this.categoryService.getCategories().subscribe((result) => {
      this.categories = result;
    });
  }

  get getControls() {
    return this.articleForm.controls;
  }

  upload(files) {
    this.fileData = files.target.files[0];

    let formData = new FormData();

    formData.append('picture', this.fileData);

    this.articleService.saveArticlePicture(formData).subscribe((result) => {
      console.log(result.path);
      this.picture = result.path;

      this.articleForm.controls.picture.setValue(this.picture);
    });
  }
}
