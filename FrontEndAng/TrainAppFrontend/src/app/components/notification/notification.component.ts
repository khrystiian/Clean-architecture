import { Component, OnInit, NgZone } from '@angular/core';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { TripForm } from '../../shared/models/TripForm';
import { FormArray } from '@angular/forms';

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.css']
})
export class NotificationComponent implements OnInit {
  notify: string = '';
  tripForm: TripForm = new TripForm(); 

  constructor(private nService: NotificationService) { }

  ngOnInit() {
    //CLIENT - HUB - CLIENT
    this.nService.sendMessageToServer("Message sent from user");

    //receive user message
    this.nService.userMessage.subscribe(data => {
      console.log(data);
    });



    //SQL Server notification
    this.nService.tripNotification();

    //receive sql notification
    this.nService.notification.subscribe(data => {
      this.tripForm = data; 
      console.log(this.tripForm);
      this.notify = '1';
    });

  }


  ngOnDestroy() {
    this.nService.notification.unsubscribe();
  }

  dropdown() {
    this.notify = ' ';
  }

}
