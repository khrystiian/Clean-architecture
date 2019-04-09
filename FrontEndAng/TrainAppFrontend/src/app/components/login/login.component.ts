import { Component, OnInit} from '@angular/core';
import { Passenger } from 'src/app/shared/models/Passenger';
import { Router } from '@angular/router';
import { PassengerService } from 'src/app/shared/services/passenger.service';
import { FormGroup, Validators, FormControl } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  form: FormGroup;
  errmsg: any;

  constructor(private router: Router, private passengerService: PassengerService) { }

  ngOnInit() {
    this.form = new FormGroup({
      username: new FormControl('', [Validators.required]),
      password: new FormControl('', [Validators.required, Validators.minLength(3)]),
      grant_type: new FormControl('password'),
    });  
  }
  
  onSubmit() {
    this.passengerService.postData(this.form.value)
      .subscribe(res => {
        if (res.status === 200){
          this.passengerService.login(this.form.value.username, res.body.access_token).subscribe((response) => {
              localStorage.setItem(response.FirstName, response.Email);

          })
          this.router.navigateByUrl('');

        } else {
          this.errmsg = res.status + ' - ' + res.statusText;
        }
      },
        err => {
          if (err.status === 401) {
            this.errmsg = 'Invalid username or password.';
          }
          else if (err.status === 400) {
            this.errmsg = 'Invalid username or password.';
          }
          else {
            this.errmsg = "Invalid username or password";
          }
        });
  }   
}
