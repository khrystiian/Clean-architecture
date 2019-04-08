import { Component, OnInit } from '@angular/core';
import { PassengerService } from './shared/services/passenger.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  firstName: string = null;

  constructor(private pService: PassengerService) { }

  ngOnInit() {
    this.firstName = localStorage.getItem(localStorage.key(0)); //set the name in navbar.
    if (this.firstName === null) {
      this.pService.navBarUsername.subscribe(email => {
        this.firstName = email;
      })
    }

  }


  login() { }



  logout() {
    localStorage.clear();
  }


}

