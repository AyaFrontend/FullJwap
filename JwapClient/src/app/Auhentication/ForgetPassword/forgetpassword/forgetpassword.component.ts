import { Router } from '@angular/router';
import { AuthService } from 'src/app/Core/auth.service';
import { ValidationResponse } from './../../../Core/Models/validation-response';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { ValidationService } from 'src/app/Core/Services/Validation/validation.service';

@Component({
  selector: 'app-forgetpassword',
  templateUrl: './forgetpassword.component.html',
  styleUrls: ['./forgetpassword.component.css']
})
export class ForgetpasswordComponent implements OnInit {

  forgetPasswordForm: any = null;
  serverError: string = '';
  validationResponse: ValidationResponse = new ValidationResponse();

  constructor(private _valid: ValidationService ,
    private _auth: AuthService , private _router: Router) {
    this.forgetPasswordForm = new FormGroup(
      {
        email: new FormControl(null , [Validators.required , Validators.email])
      }
    )
   }

  ngOnInit(): void {
  }

  public Validation(propName: string , control: FormControl)
  {
     this._valid.Validation(propName , control);
  }
  public HideAlert()
  {
    this.validationResponse.hidden = true;
  }
  public ForgetPassword(formData: FormGroup)
  {
    
    this._auth.ForgetPassword(formData.value).subscribe(
      res=>
      {
        alert(`Please check your email..`);
        localStorage.setItem('jwt-token' , res.token);
      }, 
      err => 
      {
        
        alert(`404, that email dose not exist`);
      }

      );
  }
}
