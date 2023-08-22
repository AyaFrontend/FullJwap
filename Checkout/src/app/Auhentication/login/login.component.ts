import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/Core/auth.service';
import { ValidationResponse } from 'src/app/Core/Models/validation-response';
import { ValidationService } from 'src/app/Core/Services/Validation/validation.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  
  validationResponse: ValidationResponse = new ValidationResponse();

  loginForm: any = null;
  serverError : string = '';



  constructor(private _authService: AuthService , private _validationService: ValidationService,
    private router: Router) {
    this.loginForm = new FormGroup({
      email:  new FormControl(localStorage.getItem('login-email')! , [Validators.required , Validators.email]),
      password:  new FormControl(localStorage.getItem('login-password')!, [Validators.required]),
      rememberMe:  new FormControl(null)
    }
    );
   }

  ngOnInit(): void {


  }
  public Login(loginForm: FormGroup)
  {
     this._authService.Login(loginForm.value).subscribe(
      res=> {
        localStorage.setItem('jwt-token' , res.token);
        localStorage.setItem('currentUser' , res.id);
        localStorage.setItem('profilePic' , res.profilePicture);
        this._authService.isLogging = true;
         this.router.navigateByUrl('/chat')
       
  },
      (err)=>
      {  if(err)
        {
            this.serverError = `${err.error.statusCode} ${err.error.messageError}`;
            alert(this.serverError);
        }
      }   
     );
  }

  public Validation(propName: string , formControle: FormControl)
  {
    this.validationResponse = this._validationService.Validation(propName,formControle);
    console.log(propName,formControle);
  }
  alertHide() : void
  {
    this.validationResponse.hidden = true
  }

}
