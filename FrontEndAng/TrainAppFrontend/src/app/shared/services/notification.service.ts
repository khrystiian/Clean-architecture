import { Injectable } from '@angular/core';
import { Subject, Observable } from 'rxjs';

declare var $: any;

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  public connection: any;
  public hub: any;
  public notification = new Subject<string>(); //here is stored the notification object


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
        console.log('Connection with id ' + this.connection.id + 'started.' + '\n' +
          'Transport - ' + this.connection.transport.name);

        //send messages to the hub.
        this.hub.invoke('setMessage', msg);


      })
      .fail(err => console.error('Error while starting connection: ' + err));   
  }

  public receiveMessageFromServer() {
    this.hub.on('setMessage', (message) => {
      this.notification.next(message);
    })
  }

} 
