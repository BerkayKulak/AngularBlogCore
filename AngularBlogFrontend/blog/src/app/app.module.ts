import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { HttpClientModule } from '@angular/common/http';
import { MainModule } from './pages/main.module';
import { AppComponent } from './app.component';
import { AdminModule } from './admin-pages/admin.module';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    MainModule,
    AdminModule,
  
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
