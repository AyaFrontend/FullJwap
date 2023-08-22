import { ApiResponse } from './../../Core/ServerErrors/api-response';
import { ValidationService } from './../../Core/Services/Validation/validation.service';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthService } from 'src/app/Core/auth.service';
import { ValidationResponse } from 'src/app/Core/Models/validation-response';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  validationResponse: ValidationResponse = new ValidationResponse();

  registerForm: any = null;
  serverError : string = '';
  
  constructor(private _authService: AuthService , private _validationService: ValidationService,
    private router: Router) {
    this.registerForm = new FormGroup({
      fName: new FormControl(null , [Validators.required , Validators.minLength(3)]),
      lName:  new FormControl(null , [Validators.required , Validators.minLength(3)]),
      email:  new FormControl(null , [Validators.required , Validators.email]),
      password:  new FormControl(null , [Validators.required , Validators.pattern(/[_][a-z][A-Z][0-9]{6,}/)])
    }
    );
   }

  ngOnInit(): void {
  }
  public Register(registerForm: FormGroup)
  {
     this._authService.Register(registerForm.value).subscribe(
      res=> this.router.navigateByUrl('/complete-register'),
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
    
  }
  alertHide() : void
  {
    this.validationResponse.hidden = true
  }
}
