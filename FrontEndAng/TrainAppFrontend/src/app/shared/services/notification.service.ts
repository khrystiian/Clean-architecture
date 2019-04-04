import { Injectable } from '@angular/core';
import { Subject, Observable } from 'rxjs';

declare var $: any;

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  public connection: any;
  public hub: any;
  public userMessage = new Subject<string>(); //here is stored the CLIENT - HUB - CLIENT object
  public notification = new Subject<any>(); //here is stored the notification object


  constructor() {   
    this.signalRHubConnection();
  }

  //start the connection
  private signalRHubConnection(): void {

    //create the connection with the signalr hub.
    this.connection = $.hubConnection('http://localhost:56287/');
    this.hub = this.connection.createHubProxy("notify");

    //check for incoming messages
    this.receiveMessageFromServer();
  }
  

  public sendMessageToServer(msg: string): void{

    //start the connection with signalr hub.
    this.connection.start({ jsonp: true })
      .done(() => {
        this.hub = this.connection.createHubProxy("notify");
        console.log("Connection open !");

        //send messages to the hub. Client - Hub - Client
        this.hub.invoke('setMessage', msg);

      })
      .fail(err => console.error('Error while starting connection: ' + err));   
  }

  //Get hub messages
  public receiveMessageFromServer() {
    this.hub.on('setMessage', (message) => {
      this.userMessage.next(message);
    })
  }

  //Get SQL Server notifications
  public tripNotification() {
    this.hub.on('tripNotification', (notification) => {
      this.notification.next(notification);
    })
  }

} 
