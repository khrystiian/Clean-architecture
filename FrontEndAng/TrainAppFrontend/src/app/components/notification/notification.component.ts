import { Component, OnInit, NgZone } from '@angular/core';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { TripForm } from '../../shared/models/TripForm';

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.css']
})
export class NotificationComponent implements OnInit {
  notify: string = ' ';
  tripForm: TripForm = new TripForm();
  show: boolean;

  constructor(private nService: NotificationService) { }

  ngOnInit() {
    this.show = false;
    //can be enabled!
    //CLIENT - HUB - CLIENT
    this.nService.sendMessageToServer("Waiting for incoming notifications");

    //receive user message
    this.nService.userMessage.subscribe(userMessage => {
      console.log(userMessage);
    });



    //SQL Server notification
    this.nService.tripNotification();

    //receive sql notification
    this.nService.notification.subscribe(data => {
      this.show = true;
      this.notify = '1';
      this.tripForm = data;
      console.log("Notified !");
    });
  }
   

  ngOnDestroy() {
    this.nService.notification.unsubscribe();
  }

  dropdown() {
  }

  openItem() {
    this.tripForm = new TripForm();
    this.show = false;
    this.notify = ' ';
  }
 
}
