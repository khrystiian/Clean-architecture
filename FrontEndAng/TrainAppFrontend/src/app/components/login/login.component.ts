import { Component, OnInit, Input } from '@angular/core';
import { Passenger } from 'src/app/shared/models/Passenger';
import { Router } from '@angular/router';
import { PassengerService } from 'src/app/shared/services/passenger.service';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  model = new Passenger();


  constructor(private router: Router, private passengerService: PassengerService) { }

  ngOnInit() { }

  onSubmit() {

    this.passengerService.login(this.model).subscribe(response => {
    console.log(response.Password);

    (response.Password === this.model.Password) ? this.router.navigateByUrl('') : alert("Please insert correct Username and Password");    
    });
  }
}
