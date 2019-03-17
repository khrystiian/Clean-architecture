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
 models: any;

  constructor(private router: Router, private passengerService: PassengerService) { }

  ngOnInit() {
    this.model.FirstName = "Andrei";
  }

  onSubmit(){
    this.passengerService.addPassenger(this.model).subscribe(model => this.models.push(model)); 
    this.router.navigateByUrl('');
  }

  // getLoggedInUser(): Observable<string>{
  //   return this.model.FirstName;
  // }
}
