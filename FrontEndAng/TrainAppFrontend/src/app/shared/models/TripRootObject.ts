
    export class TransitDetails {
        arrival_stop: string;
        arrival_time: Date;
        departure_stop: string;
        departure_time: Date;
    }

    export class Step {
        distance: string;
        duration: string;
        travel_mode: string;
        transit: TransitDetails;
    }

    export class Leg {
        Arrival_time: Date;
        Departure_time: Date;
        Distance: string;
        Duration: string;
        End_address: string;
        Start_address: string;
        Steps: Step[];
        RoutePreference: string;
        PassengersAge: string[];
        Seats: number;
      Username: string;
      Price: number;
    }

    export class Route {
        legs: Leg[];
    }

    export class RootObject {
      routes: Route[];
      status: boolean;
    }


