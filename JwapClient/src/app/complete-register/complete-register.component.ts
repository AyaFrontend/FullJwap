import { Router } from '@angular/router';
import { ValidationService } from './../Core/Services/Validation/validation.service';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { AuthService } from '../Core/auth.service';
import { ValidationResponse } from '../Core/Models/validation-response';

@Component({
  selector: 'app-complete-register',
  templateUrl: './complete-register.component.html',
  styleUrls: ['./complete-register.component.css']
})
export class CompleteRegisterComponent implements OnInit {

  ValidationForm:  any ;
  validationResponse:ValidationResponse = new ValidationResponse();
  constructor(private _validationService: ValidationService , 
              private _auth: AuthService , private _router: Router) { 
    this.ValidationForm =  new FormGroup(
      {
        validationCode: new FormControl(null , [Validators.required , Validators.minLength(4) , Validators.maxLength(4) , Validators.pattern(/[0-9]/)])
      }
    );
  }

  ngOnInit(): void {
  }

  public CompleteRegister(validationForm: FormGroup)
  {
      this._auth.CompleteRegister(validationForm.value).subscribe(
        res=>{
         
          this._router.navigateByUrl("/login")
          localStorage.setItem('jwt-token',res.token)
        },
        err=> 
        {
        
          this.validationResponse.hidden = false;
          this.validationResponse.errorMessage = `${err.error.statusCode} ${err.error.messageError}`;
          
        }
      );
  }
  public CheckCode(propName: string , formControle: FormControl)
  {
    this.validationResponse = this._validationService.Validation(propName,formControle);
    console.log(propName,formControle);
  }
  alertHide() : void
  {
    this.validationResponse.hidden = true
  }
}
