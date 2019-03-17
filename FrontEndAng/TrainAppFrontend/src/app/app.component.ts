import { Component, OnInit, Input } from '@angular/core';
import { Passenger } from './shared/models/Passenger';
import { RegisterComponent } from './components/register/register.component';
import { PassengerService } from './shared/services/passenger.service';
import { LoginComponent } from './components/login/login.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  firstName: string;

  constructor(private passengerService: PassengerService) { }

  ngOnInit() {
    this.passengerService.navbarUsername.subscribe(firstName => {
      this.firstName = firstName;
    });
  }

}

