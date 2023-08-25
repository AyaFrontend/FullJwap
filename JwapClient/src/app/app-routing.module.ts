import { AppComponent } from './app.component';
import { ChatComponent } from './Chatting/chat/chat.component';
import { ChangingPasswordComponent } from './Auhentication/changing-password/changing-password.component';
import { CompleteRegisterComponent } from './complete-register/complete-register.component';
import { ForgetpasswordComponent } from './Auhentication/ForgetPassword/forgetpassword/forgetpassword.component';
import { LoginComponent } from './Auhentication/login/login.component';

import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RegisterComponent } from './Auhentication/register/register.component';
import { AuthGuardGuard } from './Core/Guards/auth-guard.guard';

const routes: Routes = [
 {path: '' , redirectTo:'register' , pathMatch:'full'},
 {path: 'register' , component: RegisterComponent},
 {path: 'login' , component: LoginComponent},
 {path: 'forgetpassword' , component: ForgetpasswordComponent},
 {path: 'change-password/:email/:token' , component: ChangingPasswordComponent},
 {path: 'complete-register' , component: CompleteRegisterComponent},
 {path: 'chat' , component: ChatComponent , canActivate: [AuthGuardGuard]},

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
