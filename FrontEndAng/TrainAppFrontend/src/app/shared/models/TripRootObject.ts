
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
        arrival_time: Date;
        departure_time: Date;
        distance: string;
        duration: string;
        end_address: string;
        start_address: string;
        steps: Step[];
        routePreference: string;
        passengersAge: string[];
        seats: number;
        Username: string;
    }

    export class Route {
        legs: Leg[];
    }

    export class RootObject {
      routes: Route[];
      status: boolean;
    }


