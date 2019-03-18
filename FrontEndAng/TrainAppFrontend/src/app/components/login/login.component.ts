import { Component, OnInit, Input } from '@angular/core';
import { Passenger } from 'src/app/shared/models/Passenger';
import { Router } from '@angular/router';
import { PassengerService } from 'src/app/shared/services/passenger.service';

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
      if (response.Password === this.model.Password && response.Email === this.model.Email) {
        localStorage.setItem("blank", "blank"); //necessary for select element at index 1
        localStorage.setItem(response.Email, response.FirstName);
        location.reload();
        this.router.navigateByUrl('');
      }
      else {
        alert("Please insert correct Username and Password");
      }
    });
  }
}
