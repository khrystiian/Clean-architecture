import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { OwlDateTimeModule, OwlNativeDateTimeModule } from 'ng-pick-datetime';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatInputModule, MatSelectModule, MatCheckboxModule, MatSidenavModule, MatButtonModule, MatIconModule, MatToolbarModule, MatListModule, MatDialogModule, MatAutocompleteModule, MatTableDataSource, MatTableModule} from '@angular/material';
import {MatRadioModule} from '@angular/material/radio';
import { AgmCoreModule } from '@agm/core';
import { AgmDirectionModule } from 'agm-direction'
import { ToastrModule } from 'ngx-toastr';
import { MatPaginatorModule } from '@angular/material/paginator';

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
import { NotificationComponent } from './components/notification/notification.component';
import { ElasticsearchComponent } from './components/elasticsearch/elasticsearch.component';
import { ElasticsearchDialogComponent } from './components/elasticsearch-dialog/elasticsearch-dialog.component';
@NgModule({
  declarations: [
    AppComponent,
    TripComponent,
    LoginComponent,
    RegisterComponent,
    NotificationComponent,
    ElasticsearchComponent,
    ElasticsearchDialogComponent
  ],
  entryComponents: [ElasticsearchDialogComponent],
  imports: [
    BrowserModule,
    MatTableModule,
    AppRoutingModule,
    OwlDateTimeModule, 
    OwlNativeDateTimeModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    MatInputModule,
    MatSelectModule,
    MatRadioModule,
    HttpClientModule,
    MatButtonModule,
    MatIconModule,
    MatSidenavModule,
    MatCheckboxModule,
    MatAutocompleteModule,
    MatInputModule,
    MatPaginatorModule,
    MatDialogModule,
    AgmCoreModule.forRoot({
      //Google maps API Key
    }),
    AgmDirectionModule,
    LayoutModule,
    MatToolbarModule,
    MatListModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot({
      closeButton: false,
      positionClass: "toast-top-right"
    })  
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
    LoginComponent,
    MatDialogModule,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

platformBrowserDynamic().bootstrapModule(AppModule); 
