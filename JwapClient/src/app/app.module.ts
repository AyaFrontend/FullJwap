import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {HttpClientModule, HTTP_INTERCEPTORS} from '@angular/common/http'
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LoginComponent } from './Auhentication/login/login.component';

import { RegisterComponent } from './Auhentication/register/register.component';
import { NavbarComponent } from './Shared/Navbar/navbar/navbar.component';
import { ForgetpasswordComponent } from './Auhentication/ForgetPassword/forgetpassword/forgetpassword.component';
import { CompleteRegisterComponent } from './complete-register/complete-register.component';
import { JwtTokenInterceptor } from './Core/Interceptors/jwt-token.interceptor';

import { ChangingPasswordComponent } from './Auhentication/changing-password/changing-password.component';
import { ChatComponent } from './Chatting/chat/chat.component';
import { SearchComponent } from './Shared/search/search.component';
import { FriendsComponent } from './Chatting/Connections/friends/friends.component';
import { CallingComponent } from './Calling/calling/calling.component';
import { VideoCallComponent } from './VideoCall/video-call/video-call.component';
import { AuthGuardGuard } from './Core/Guards/auth-guard.guard';
import { AuthService } from './Core/auth.service';
import { ChatService } from 'src/Services/chat.service';


@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
  
    RegisterComponent,
        NavbarComponent,
        ForgetpasswordComponent,
        CompleteRegisterComponent,
      
        ChangingPasswordComponent,
                ChatComponent,
                SearchComponent,
                FriendsComponent,
                CallingComponent,
                VideoCallComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule
    
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS , useClass: JwtTokenInterceptor , multi: true},
    AuthGuardGuard , AuthService , ChatService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
