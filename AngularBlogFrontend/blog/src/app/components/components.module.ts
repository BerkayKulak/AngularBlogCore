import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MenuCategoryComponent } from './menu-category/menu-category.component';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [MenuCategoryComponent],
  imports: [CommonModule, RouterModule],
  exports: [MenuCategoryComponent],
})
export class ComponentsModule {}
