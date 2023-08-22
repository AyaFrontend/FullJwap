import { ChangePasswordDto } from './../../Core/Models/change-password-dto';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from 'src/app/Core/auth.service';
import { ValidationResponse } from 'src/app/Core/Models/validation-response';
import { ValidationService } from 'src/app/Core/Services/Validation/validation.service';

@Component({
  selector: 'app-changing-password',
  templateUrl: './changing-password.component.html',
  styleUrls: ['./changing-password.component.css']
})
export class ChangingPasswordComponent implements OnInit {

  changePasswordForm: any = null;
  validationResponse: ValidationResponse = new ValidationResponse();
  changePasswordDto: ChangePasswordDto = new ChangePasswordDto();

  constructor(private _valid: ValidationService, private _auth: AuthService ,
    private activeRouter: ActivatedRoute) {
    this.changePasswordForm = new FormGroup(
      {
        password:  new FormControl(null , [Validators.required , Validators.pattern(/[_][a-z][A-Z][0-9]{6,}/)])
      }
    )
   }

  ngOnInit(): void {
    this.activeRouter.params.subscribe(
      params => 
      {
        this.changePasswordDto.email = params['email'];
        this.changePasswordDto.token = params['token'];
        console.log(this.changePasswordDto.token)
      }

    )
  }

  public Validation(propName: string , control: FormControl)
  {
     this._valid.Validation(propName , control);
  }
  public HideAlert()
  {
    this.validationResponse.hidden = true;
  }
  public ChangePassword(formData: FormGroup)
  {
    this.changePasswordDto.password = formData.value.password;
    console.log(this.changePasswordDto);
    this._auth.ChangePassword(this.changePasswordDto).subscribe(
      res=> alert(res.messageError),
      err=> alert(`${err.error.statusCode}, ${err.error.messageError}`)
    );
  }
}
