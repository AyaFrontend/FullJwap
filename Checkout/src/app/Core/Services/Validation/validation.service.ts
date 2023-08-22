import { ValidationResponse } from './../../Models/validation-response';
import { FormGroup, FormControl } from '@angular/forms';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ValidationService {
 validationResponse: ValidationResponse = new ValidationResponse()
  constructor() { }

  public Validation(propName: string, formControle: FormControl) : ValidationResponse
  {
    

    
    if(formControle?.touched && formControle?.errors != null)
    {
       this.validationResponse.hidden = false;
       if(formControle?.errors["required"])
       {
        this.validationResponse.errorMessage = `${propName} is required`;
        return this.validationResponse;
       }
       else if(formControle?.errors["pattern"])
        {
          this.validationResponse.errorMessage =`${propName}} Pattern is invalid`;
        }
        else{
          this.validationResponse.errorMessage = 'there is an error'
        }
         
    }
    return this.validationResponse;
  }
}
