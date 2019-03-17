import { OnInit, Component, DoCheck } from '@angular/core';
import { TripForm } from 'src/app/shared/models/TripForm';
import { TravelMode } from 'src/app/shared/models/TravelMode';
import { TripService } from 'src/app/shared/services/trip.service';
import { HttpClient } from '@angular/common/http';
import { } from '@google/maps';
import { PassengerService } from 'src/app/shared/services/passenger.service';
import { FormControl } from '@angular/forms';
import { RootObject, Step, TransitDetails } from 'src/app/shared/models/TripRootObject';

declare var google: any;

@Component({
  selector: 'app-trip', //
  templateUrl: './trip.component.html',
  styleUrls: ['./trip.component.css']
})
/**
 * No authentication
 * add switch for when toggling between TRANSIT and others- to make receipt dissapear.
 */
export class TripComponent implements OnInit, DoCheck  {
  username: string;
  rootObject: RootObject;
  tripPrice: any;
  models: any;
  model = new TripForm;
  showForm: boolean;
  map: any;
  passengersAgeList: string[];
  passAge: string;
  secondContainerFlag: boolean;
  passengersAge = new FormControl();
  travelPreferences: TravelMode[] = [
    { value: 'FEWER_TRANSFERS', viewValue: 'Fewer transfers' },
    { value: 'LESS_WALKING', viewValue: 'Less walking' }
  ];
  travelMode: TravelMode[] = [
    { value: 'DRIVING', viewValue: 'Driving' },
    { value: 'TRANSIT', viewValue: 'Train/Bus' },
    { value: 'WALKING', viewValue: 'Walking' },
    { value: 'BICYCLING', viewValue: 'Bicycling' }
  ];
  transitMode: TravelMode[] = [
    { value: 'BUS', viewValue: 'Bus' },
    { value: 'RAIL', viewValue: 'Rail' },
    { value: 'SUBWAY', viewValue: 'Subway' },
    { value: 'TRAM', viewValue: 'Light Rail' },
    { value: 'TRAIN', viewValue: 'Train' }
  ];
  seats: TravelMode[] = [
    { value: 1, viewValue: 1 },
    { value: 2, viewValue: 2 },
    { value: 3, viewValue: 3 },
    { value: 4, viewValue: 4 },
    { value: 4, viewValue: 5 }
  ];
  passengerAgeList: string[] = ['0-11 years old', '12-15 years old', '16-25 years old', 'Adult', '10 tickets'];

  constructor(private passengerService: PassengerService, private http: HttpClient, private tripService: TripService) { }


  ngOnInit() { 
    document.getElementById('continueToOrder').style.display = 'none';
    this.initMap(null);
  }

  ngDoCheck() { 
    if (this.passengersAge.value  !== null) 
      this.passAge= this.passengersAge.value[0];    
      this.passengersAgeList = this.passengersAge.value
  }

  toggle() { this.showForm = !this.showForm; }

  onSubmit() {
    this.showForm = false;
    if(this.model.DepartureArrival === "Departure") {
      this.model.DepartureTime = this.model.Time; 
      this.model.ArrivalTime = null;
    }else{
      this.model.ArrivalTime = this.model.Time;
      this.model.DepartureTime = null;
    }  
    (this.model.TravelVia === 'TRANSIT') ? document.getElementById('continueToOrder').style.display = 'block' : document.getElementById('continueToOrder').style.display = 'none';


    //  this.model = {ArrivalTime: null, DepartureTime: null, DestinationAddress: "vejle", HomeAddress:"aalborg", TransitMode: 'BUS', 
    // TripType: null, TripMode: 'TRANSIT', Time: null, Seats: 5, PassengerAge: null, Preferences: null};    
    this.initMap(this.model);
  }

  continue() {
    this.passengerService.navbarUsername.subscribe(usr => {
      this.secondContainerFlag = true;
      this.username = usr;
      if (usr === null) {
        alert("Please Login or Register to order tickets")      
      } else {
        document.getElementById("main-container").className = 'col-md-8';
        if (document.getElementById("second-container") !== null) {
          document.getElementById("second-container").className = 'col-md-4';
        }
      }
    });
  }

  public initMap(model: TripForm): void {
    //Google Maps Center/Focus point
    var mapCanvas = document.getElementById("map");
    var myCenter = new google.maps.LatLng(57.0488, 9.9217);
    var mapOptions = {
      center: myCenter,
      zoom: 10
    }
    this.map = new google.maps.Map(mapCanvas, mapOptions);

    //Google Maps Marker
    var marker = new google.maps.Marker({
      position: new google.maps.LatLng(57.0488, 9.9217),
      map: this.map,
      title: 'Aalborg' //hometown
    });

    //Google Maps InfoWindow
    google.maps.event.addListener(marker, 'click', (function (marker) {
      var infowindow = new google.maps.InfoWindow();
      return function () {
        infowindow.setContent('Aalborg');
        infowindow.open(this.map, marker);
      }
    })(marker));
    marker.setMap(this.map);

    //Google Maps Directions
    var directionsService = new google.maps.DirectionsService;
    var directionsDisplay = new google.maps.DirectionsRenderer;
    document.getElementById('right-panel').textContent = null;
    this.calculateAndDisplayRoute(directionsService, directionsDisplay, model);
  }

  calculateAndDisplayRoute(directionsService, directionsDisplay, tripModel: TripForm) {
    if (tripModel !== null) { 
      directionsService.route({
        origin: tripModel.HomeAddress,
        destination: tripModel.DestinationAddress,
        travelMode: tripModel.TravelVia,
        transitOptions: {
          departureTime: tripModel.DepartureTime,
          arrivalTime: tripModel.ArrivalTime,
          routingPreference: tripModel.Preference,
          modes: [tripModel.TransitMode]
        },
        unitSystem: google.maps.UnitSystem.METRIC
      }, function (response, status) {
        if (status === 'OK') {
          directionsDisplay.setDirections(response);
          if (tripModel.TravelVia === "TRANSIT") {
            convertToTripDTO(response.routes[0].legs[0], tripModel.Preference);
          }
        } else {
          window.alert('Directions request failed due to ' + status);
        }
      });

      directionsDisplay.setMap(this.map);
      directionsDisplay.setPanel(document.getElementById('right-panel'));
      document.getElementById('map').className = 'col-md-8';
      document.getElementById('right-panel').className = 'col-md-4';      
    }
  };

  calculateTripPrice(){
    var root = this.rootObject.routes[0].legs[0];
    root.passengersAge = this.passengersAgeList;
    //root.steps[] = this.model.Seats;
    root.Username = this.username;
    console.log(this.rootObject)
    this.tripService.calculatePrice(this.rootObject).subscribe(response => this.tripPrice = response) //check here
}
  finish(){
    console.log(this.model);
    console.log(this.tripPrice);
    var confirm=true;
    this.tripPrice > 0 ? this.tripService.confirmPayment(confirm) : false;
 }

 /**
  * Necessary to trim the google maps generated API object. 
  * @param response, the generated response from API. 
  */
 getRootObject(response: any, tripPreference: string): RootObject{
var rootObjectSteps: Step[] = [];
   for (let i = 0; i < response.steps.length; i++) {
     var transitDetail: TransitDetails;
     console.log(response.steps[i].distance.text)
     if (response.steps[i].transit === null || response.steps[i].transit === undefined) {
       transitDetail = {
         arrival_stop: null,
         arrival_time: null,
         departure_stop: null,
         departure_time: null
       }
     } else {
       transitDetail = {
         arrival_stop: response.steps[i].transit.arrival_stop.name,
         arrival_time: response.steps[i].transit.arrival_time.value,
         departure_stop: response.steps[i].transit.departure_stop.name,
         departure_time: response.steps[i].transit.departure_time.value
       }
     }
     var step: Step = {
       distance: response.steps[i].distance.text,
       duration: response.steps[i].duration.text,
       travel_mode: response.steps[i].travel_mode,
       transit: transitDetail
     };
     rootObjectSteps.push(step);
    }

   this.rootObject = {
     routes: [{
       legs: [{
        arrival_time: response.arrival_time.value,
        departure_time: response.departure_time.value,
        distance: response.distance.text,
        duration: response.duration.text,
        end_address: response.end_address,
        start_address: response.start_address,
        steps: rootObjectSteps,
        passengersAge: null,
        routePreference: tripPreference,
        seats: null,
        Username: null
       }]
     }]
   };
   return this.rootObject;
 }
}

/**
 * Necessary to handle maps object and convert into a more usable one.
 * @param response, the object generated by the API. 
 */
function convertToTripDTO(response: any, tripPreference: string) {
  TripComponent.prototype.getRootObject(response, tripPreference);
};
