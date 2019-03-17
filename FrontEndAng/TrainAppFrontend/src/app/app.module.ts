import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { OwlDateTimeModule, OwlNativeDateTimeModule } from 'ng-pick-datetime';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {MatInputModule, MatSelectModule, MatCheckboxModule, MatSidenavModule, MatButtonModule, MatIconModule, MatToolbarModule, MatListModule, MatDialogModule} from '@angular/material';
import {MatRadioModule} from '@angular/material/radio';
import { AgmCoreModule } from '@agm/core';
import { AgmDirectionModule } from 'agm-direction' 

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { HttpErrorHandler } from './shared/services/http-error-handler.service';
import { MessageService } from './shared/services/message.service';
import { TripComponent } from './components/trip/trip.component';
import { TripService } from './shared/services/trip.service';
import { LayoutModule } from '@angular/cdk/layout';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { Passenger } from './shared/models/Passenger';
@NgModule({
  declarations: [
    AppComponent,
    TripComponent,
    LoginComponent,
    RegisterComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    OwlDateTimeModule, 
    OwlNativeDateTimeModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    MatInputModule,
    MatSelectModule,
    MatRadioModule,
    MatDialogModule,
    HttpClientModule,
    MatButtonModule,
    MatIconModule,
    MatSidenavModule,
    MatCheckboxModule,
    AgmCoreModule.forRoot({
      apiKey: 'AIzaSyBymUNgedD8zJ3CRo6ZlrKk99WFBWWi-94'  //AIzaSyCq7O84-3Q3EfCSwVSM0HYbGpkceQWCXIU
    }),
    AgmDirectionModule,
    LayoutModule,
    MatToolbarModule,
    MatListModule
  ],
  exports:[
    MatSidenavModule,
    MatButtonModule,
    MatIconModule
  ],
  providers: [
    TripService,
    MessageService,
    HttpErrorHandler,
    LoginComponent
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

platformBrowserDynamic().bootstrapModule(AppModule); 