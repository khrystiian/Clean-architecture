export class TripForm{
    HomeAddress: string;
    DestinationAddress: string;
    DepartureArrival: string; //departure/arrival
    TravelVia: string;//buss/train
    TransitMode: string; //bus/train/rail
    Preference: string;
    DepartureTime: Date;
    ArrivalTime: Date;
    Time: Date;
    Seats: number;
    PassengerAge: string[];

     constructor(){}
}