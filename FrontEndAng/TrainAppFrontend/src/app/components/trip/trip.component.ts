import { OnInit, Component, DoCheck } from '@angular/core';
import { TripForm } from 'src/app/shared/models/TripForm';
import { TravelMode } from 'src/app/shared/models/TravelMode';
import { TripService } from 'src/app/shared/services/trip.service';
import { } from '@google/maps';
import { FormControl } from '@angular/forms';
import { RootObject, Step, TransitDetails, Leg } from 'src/app/shared/models/TripRootObject';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

declare var google: any;

@Component({
  selector: 'app-trip', //
  templateUrl: './trip.component.html',
  styleUrls: ['./trip.component.css']
})
/**
 * No authentication -  username saved in the localstorage.
 * add switch for when toggling between TRANSIT and others- to make receipt dissapear.
 */
export class TripComponent implements OnInit, DoCheck  {
  rootObject: RootObject;
  model = new TripForm;
  tripResponsePrice: number;
  map: any;
  models: any;
  passAge: string;
  tripResponseID: string;
  showForm: boolean;
  secondContainerFlag: boolean;
  passengersAgeList: string[];
  passengerAgeList: string[] = ['0-11 years old', '12-15 years old', '16-25 years old', 'Adult'];
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

  constructor(private tripService: TripService, private toastr: ToastrService, private router: Router) { }


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
    if (this.model.TravelVia === 'TRANSIT') {
      document.getElementById('continueToOrder').style.display = 'block'
    } else {
      this.switchToMainView();
    }
 
    this.initMap(this.model);
  }

  switchToMainView() {
    document.getElementById("main-container").className = 'col-md-12';
    document.getElementById('map').className = 'col-md-12';
    document.getElementById('continueToOrder').style.display = 'none'
    this.secondContainerFlag = false;
  }

  continue() {
    this.secondContainerFlag = true;
    this.tripResponsePrice = undefined;
    if (localStorage.length === 0) {
        alert("Please Login or Register to order tickets")      
    } else {
      document.getElementById('right-panel').style.display = 'normal';
        document.getElementById("main-container").className = 'col-md-8';
        if (document.getElementById("second-container") !== null) {
          document.getElementById("second-container").className = 'col-md-4';
        }
      }
  }


  calculateTripPrice() { 
    var root = this.rootObject.routes[0].legs[0];
    root.passengersAge = this.passengersAgeList;
    root.seats = this.model.Seats;
    root.Username = localStorage.getItem(localStorage.key(0));

    this.tripService.calculatePrice(this.rootObject).subscribe(response => this.getDataObject(response));
  }

  getDataObject(data: any) {
    this.tripResponsePrice = data.TripPrice.toFixed(2);
    this.tripResponseID = data.ID;
  }

  finish() {
    var root: RootObject = { routes: [], status: this.tripResponsePrice > 0 ? true : false };
    this.tripService.confirmPayment(this.tripResponseID, root).subscribe(x => {
      //Optional features
    });
    this.successmsg();
    this.router.navigateByUrl('');
  }

  canceledPayment() {
    this.infomsg();
    this.router.navigateByUrl('');
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
      title: 'Aalborg' //initial location
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

 /**
  * Necessary to trim the google maps generated API object. 
  * @param response, the generated response from API. 
  */
 getRootObject(response: any, tripPreference: string): RootObject{
var rootObjectSteps: Step[] = [];
   for (let i = 0; i < response.steps.length; i++) {
     var transitDetail: TransitDetails;
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
        seats: 1,
         Username: null
       }]
     }],
     status: false
   };
   return this.rootObject;
  }


  //Toastr notification pop up
  successmsg() {
    this.toastr.success("Ticked ordered succesfully !", 'Success')
  }

  //Toastr notification pop up
  infomsg() {
    this.toastr.info("Payment canceled !", "Info")
  }
}

/**
 * Necessary to handle maps object and convert into a more usable one.
 * @param response, the object generated by the API. 
 */
function convertToTripDTO(response: any, tripPreference: string) {
  TripComponent.prototype.getRootObject(response, tripPreference);
};
