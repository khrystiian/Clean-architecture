import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  firstName: string = null;

  constructor() { }

  ngOnInit() {
    this.firstName = localStorage.getItem(localStorage.key(0)); //set the name in navbar.
  }

  logout() {
    localStorage.clear(); //clear localstorage =?
  }

}

