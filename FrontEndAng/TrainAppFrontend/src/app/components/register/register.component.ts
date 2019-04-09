import { Component, OnInit } from '@angular/core';
import { Passenger } from 'src/app/shared/models/Passenger';
import { Router } from '@angular/router';
import { PassengerService } from 'src/app/shared/services/passenger.service';
import { FormGroup, Validators, FormControl } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  public model: Passenger = new Passenger();
  form: FormGroup;
  errmsg: any;

  constructor(private router: Router, private passengerService: PassengerService) { }

  ngOnInit() {
    this.form = new FormGroup({
      FirstName: new FormControl('', [Validators.required]),
      LastName: new FormControl('', [Validators.required]),
      Address: new FormControl('', [Validators.required]),
      Email: new FormControl('', [Validators.required]),
      Password: new FormControl('', [Validators.required, Validators.minLength(6)]),
    });  
  }

  onSubmit() {
      this.passengerService.addPassenger(this.form.value).subscribe(() => {
        localStorage.setItem(this.form.value.FirstName, this.form.value.Email);
        this.router.navigateByUrl('');
      });
  }
  
}
