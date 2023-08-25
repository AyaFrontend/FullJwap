import { ApiResponse } from './ServerErrors/api-response';
import { ChangePasswordDto } from './Models/change-password-dto';
import { ForgetPasswordDto } from './Models/forget-password-dto';
import { ValidationCodeDto } from './Models/validation-code-dto';
import { environment } from './../../environments/environment';
import { RegisterDto } from './Models/register-dto';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { LoginDto} from './Models/login-dto';
import { UserDto } from './Models/user-dto';
import { StripeCardElementChangeEvent } from '@stripe/stripe-js';


@Injectable({
  providedIn: 'root'
})
export class AuthService {
isLogging: boolean = false;

  constructor(private _http: HttpClient) { }
  public Register(registerDto: RegisterDto): Observable<RegisterDto>
  {
    return this._http.post(environment.BASE_URL + 'Account/register' ,registerDto) as Observable<RegisterDto>;
  }

  public Login(loginDto: LoginDto): Observable<UserDto>
  {
    
    if(loginDto.rememberMe)
    {
      
      localStorage.setItem('login-email',loginDto.email);
      localStorage.setItem('login-password',loginDto.password);
    }

    return this._http.post(environment.BASE_URL + 'Account/login' ,loginDto) as Observable<UserDto>;
  }
  public CompleteRegister(code: ValidationCodeDto): Observable<UserDto>
  {
    return this._http.post(environment.BASE_URL + 'Account/completeRegister' ,code) as Observable<UserDto>;
  }

  public ForgetPassword(email: ForgetPasswordDto):Observable<any>
  {
    
    return this._http.post(environment.BASE_URL + 'Account/forgetPassword' ,email ) as Observable<any>
  }

  public ChangePassword(changePasswordDto: ChangePasswordDto):Observable<ApiResponse>
  {
    
    return this._http.post(environment.BASE_URL + 'Account/changePassword' , changePasswordDto ) as Observable<ApiResponse>
  }
}
