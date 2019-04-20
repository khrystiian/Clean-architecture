import { Component, OnInit, ViewChild } from '@angular/core';
import { startWith, map, filter } from 'rxjs/operators';
import { FormControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { MatPaginator, MatDialog } from '@angular/material';
import { ElasticsearchService } from '../../shared/services/elasticsearch.service';
import { ElasticsearchDialogComponent } from '../elasticsearch-dialog/elasticsearch-dialog.component';
import { Leg } from '../../shared/models/TripRootObject';


@Component({
  selector: 'app-elasticsearch',
  templateUrl: './elasticsearch.component.html',
  styleUrls: ['./elasticsearch.component.css']
})
export class ElasticsearchComponent implements OnInit {
  diacritics: any = {
  a: 'ÀÁÂÃÄÅàáâãäåĀāąĄ',
  c: 'ÇçćĆčČ',
  d: 'đĐďĎ',
  e: 'ÈÉÊËèéêëěĚĒēęĘ',
  i: 'ÌÍÎÏìíîïĪī',
  l: 'łŁ',
  n: 'ÑñňŇńŃ',
  o: 'ÒÓÔÕÕÖØòóôõöøŌō',
  r: 'řŘ',
  s: 'ŠšśŚȘș',
  t: 'ťŤȚț',
  u: 'ÙÚÛÜùúûüůŮŪū',
  y: 'ŸÿýÝ',
  z: 'ŽžżŻźŹ'
}
  legResults: Leg[];
  stateCtrl = new FormControl();
  filteredStates: Observable<Leg[]>;

  // MatPaginator Inputs
  length = 0;
  pageSize = 4;

  // MatPaginator Output
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(private elasticsearchService: ElasticsearchService, public dialog: MatDialog) {
    this.legResults = [];
    this.mapSearch(0, 4);
  }

  ngOnInit() {}


  public handlePage(e: any) {
    this.pageSize = e.pageSize;
    this.length = e.length;

    var first: number = 4;
    var last: number = 4;
    if (e.previousPageIndex < e.pageIndex) {
      first += (e.previousPageIndex * e.pageSize);
      last += (e.pageIndex * e.pageSize);
      this.mapSearch(first, last);
    }
    else if (e.previousPageIndex > e.pageIndex) {
      first = (e.pageIndex * e.pageSize);
      last = (e.previousPageIndex * e.pageSize);
      this.mapSearch(first, last)
    }
  }


  elasticSearch(filterValue: string) {
    //CALL ELASTICSEARCH IN THE SERVER
    this.elasticsearchService.search(filterValue).subscribe(response => {
      this.legResults = [];

      for (var i = 0; i < JSON.parse(response).hits.hits.length; i++) {
        this.legResults.push(JSON.parse(response).hits.hits[i]._source);
      }
      this.length = this.legResults.length;
    })
  }

  openDialog(searchedObj: Leg) {
    this.dialog.open(ElasticsearchDialogComponent, {
      width: '40%',
      height: 'auto',
      disableClose: false,
      data: {
        From: searchedObj.Start_address,
        To: searchedObj.End_address,
        Arrival: searchedObj.Arrival_time,
        Departure: searchedObj.Departure_time,
        Distance: searchedObj.Distance,
        Duration: searchedObj.Duration,
        Price: searchedObj.Price,
        Preference: searchedObj.RoutePreference,
        Routes: searchedObj.Steps
      }
    });
  }


  private _filterStates(value: string): Leg[] {

    const filterValue = value.toLowerCase();
    this.elasticSearch(filterValue);

    return this.legResults.filter(state => this.replaceDiacritics(state.Start_address.toLowerCase()).indexOf(filterValue) === 0);
  }

  private mapSearch(first?: number, last?: number) {
    this.legResults = [];

    this.filteredStates = this.stateCtrl.valueChanges
      .pipe(
        startWith(''),
        map(state => state ? this._filterStates(state) : this.legResults.slice(first, last))
    );
  }

  replaceDiacritics(text):string {
    for (var toLetter in this.diacritics) if (this.diacritics.hasOwnProperty(toLetter)) {
      for (var i = 0, ii = this.diacritics[toLetter].length, fromLetter, toCaseLetter; i < ii; i++) {
        fromLetter = this.diacritics[toLetter][i];
        if (text.indexOf(fromLetter) < 0) continue;
        toCaseLetter = fromLetter == fromLetter.toUpperCase() ? toLetter.toUpperCase() : toLetter;
        text = text.replace(new RegExp(fromLetter, 'g'), toCaseLetter);
      }
    }
    return text;
  }

}

