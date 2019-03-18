import { Component, OnInit } from '@angular/core';
import { Passenger } from 'src/app/shared/models/Passenger';
import { Router } from '@angular/router';
import { PassengerService } from 'src/app/shared/services/passenger.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
 public model: Passenger = new Passenger();

  constructor(private router: Router, private passengerService: PassengerService) { }

  ngOnInit() {
  }

  onSubmit() {
    this.passengerService.addPassenger(this.model).subscribe(() => {
      localStorage.setItem("blank", "blank");
      localStorage.setItem(this.model.Email, this.model.FirstName);
      location.reload();
      this.router.navigateByUrl('');
    }); 
  }
  
}
