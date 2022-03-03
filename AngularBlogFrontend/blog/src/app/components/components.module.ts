import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgxPaginationModule } from 'ngx-pagination';
import { MenuCategoryComponent } from './menu-category/menu-category.component';
import { RouterModule } from '@angular/router';

import { PageTitleComponent } from './page-title/page-title.component';

@NgModule({
  declarations: [MenuCategoryComponent, PageTitleComponent],
  imports: [CommonModule, RouterModule, NgxPaginationModule],
  exports: [MenuCategoryComponent, PageTitleComponent],
})
export class ComponentsModule {}
