import { Component, OnInit, NgZone } from '@angular/core';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.css']
})
export class NotificationComponent implements OnInit {
  notify: string = '1';

  constructor(private nService: NotificationService) { }

  ngOnInit() {
    this.nService.sendMessageToServer("text from component ");

    this.nService.notification.subscribe(data => {
      console.log(data);
    });

  }


  ngOnDestroy() {
    this.nService.notification.unsubscribe();
  }

  dropdown() {
    this.notify =' ';
  }

}
