import { Component, OnInit, ViewChild } from '@angular/core';
import { startWith, map, filter } from 'rxjs/operators';
import { FormControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { PageEvent, MatPaginator, MatDialog } from '@angular/material';
import { ElasticsearchService } from '../../shared/services/elasticsearch.service';
import { ElasticsearchDialogComponent } from '../elasticsearch-dialog/elasticsearch-dialog.component';

export interface DialogData {
  animal: string;
  name: string;
}

export interface State {
  flag: string;
  name: string;
  population: string;
}

@Component({
  selector: 'app-elasticsearch',
  templateUrl: './elasticsearch.component.html',
  styleUrls: ['./elasticsearch.component.css']
})
export class ElasticsearchComponent implements OnInit {
  animal: string;
  name: string;
  stateCtrl = new FormControl();
  filteredStates: Observable<State[]>;

  // MatPaginator Inputs
  length = 0;
  pageSize = 4;

  // MatPaginator Output
  @ViewChild(MatPaginator) paginator: MatPaginator;


  states: State[] = [
    {
      name: 'Arkansas',
      population: '2.978M',
      // https://commons.wikimedia.org/wiki/File:Flag_of_Arkansas.svg
      flag: 'https://upload.wikimedia.org/wikipedia/commons/9/9d/Flag_of_Arkansas.svg'
    },
    {
      name: 'Calitdrnia',
      population: '39.14M',
      // https://commons.wikimedia.org/wiki/File:Flag_of_California.svg
      flag: 'https://upload.wikimedia.org/wikipedia/commons/0/01/Flag_of_California.svg'
    },
    {
      name: 'Disney',
      population: '39.14M',
      // https://commons.wikimedia.org/wiki/File:Flag_of_California.svg
      flag: 'https://upload.wikimedia.org/wikipedia/commons/0/01/Flag_of_California.svg'
    },
    {
      name: 'Florida',
      population: '20.27M',
      // https://commons.wikimedia.org/wiki/File:Flag_of_Florida.svg
      flag: 'https://upload.wikimedia.org/wikipedia/commons/f/f7/Flag_of_Florida.svg'
    },
    {
      name: 'Texas',
      population: '27.47M',
      // https://commons.wikimedia.org/wiki/File:Flag_of_Texas.svg
      flag: 'https://upload.wikimedia.org/wikipedia/commons/f/f7/Flag_of_Texas.svg'
    },
    {
      name: 'New York',
      population: '39.14M',
      // https://commons.wikimedia.org/wiki/File:Flag_of_California.svg
      flag: 'https://upload.wikimedia.org/wikipedia/commons/0/01/Flag_of_California.svg'
    },
    {
      name: 'Samsung',
      population: '20.27M',
      // https://commons.wikimedia.org/wiki/File:Flag_of_Florida.svg
      flag: 'https://upload.wikimedia.org/wikipedia/commons/f/f7/Flag_of_Florida.svg'
    },
    {
      name: 'Mexic',
      population: '27.47M',
      // https://commons.wikimedia.org/wiki/File:Flag_of_Texas.svg
      flag: 'https://upload.wikimedia.org/wikipedia/commons/f/f7/Flag_of_Texas.svg'
    },
    {
      name: 'Canada',
      population: '39.14M',
      // https://commons.wikimedia.org/wiki/File:Flag_of_California.svg
      flag: 'https://upload.wikimedia.org/wikipedia/commons/0/01/Flag_of_California.svg'
    },
    {
      name: 'Las Vegas',
      population: '20.27M',
      // https://commons.wikimedia.org/wiki/File:Flag_of_Florida.svg
      flag: 'https://upload.wikimedia.org/wikipedia/commons/f/f7/Flag_of_Florida.svg'
    },
    {
      name: 'Cal Log',
      population: '27.47M',
      // https://commons.wikimedia.org/wiki/File:Flag_of_Texas.svg
      flag: 'https://upload.wikimedia.org/wikipedia/commons/f/f7/Flag_of_Texas.svg'
    },
    {
      name: 'Kolding',
      population: '20.27M',
      // https://commons.wikimedia.org/wiki/File:Flag_of_Florida.svg
      flag: 'https://upload.wikimedia.org/wikipedia/commons/f/f7/Flag_of_Florida.svg'
    },
    {
      name: 'Vejen',
      population: '27.47M',
      // https://commons.wikimedia.org/wiki/File:Flag_of_Texas.svg
      flag: 'https://upload.wikimedia.org/wikipedia/commons/thumb/c/c3/Flag_of_France.svg/800px-Flag_of_France.svg.png'
    },
    {
      name: 'Vejle',
      population: '39.14M',
      // https://commons.wikimedia.org/wiki/File:Flag_of_California.svg
      flag: 'https://upload.wikimedia.org/wikipedia/commons/thumb/0/05/Flag_of_Brazil.svg/720px-Flag_of_Brazil.svg.png'
    },
    {
      name: 'Las Herning',
      population: '20.27M',
      // https://commons.wikimedia.org/wiki/File:Flag_of_Florida.svg
      flag: 'https://upload.wikimedia.org/wikipedia/commons/thumb/7/73/Flag_of_Romania.svg/600px-Flag_of_Romania.svg.png'
    },
    {
      name: 'Aalborg',
      population: '27.47M',
      // https://commons.wikimedia.org/wiki/File:Flag_of_Texas.svg
      flag: 'https://upload.wikimedia.org/wikipedia/commons/thumb/9/9c/Flag_of_Denmark.svg/740px-Flag_of_Denmark.svg'
    },
    {
      name: 'China',
      population: '20.27M',
      // https://commons.wikimedia.org/wiki/File:Flag_of_Florida.svg
      flag: 'https://upload.wikimedia.org/wikipedia/commons/thumb/7/73/Flag_of_Romania.svg/600px-Flag_of_Romania.svg.png'
    },
    {
      name: 'Japan',
      population: '27.47M',
      // https://commons.wikimedia.org/wiki/File:Flag_of_Texas.svg
      flag: 'https://upload.wikimedia.org/wikipedia/commons/thumb/9/9c/Flag_of_Denmark.svg/740px-Flag_of_Denmark.svg'
    }
  ];

  constructor(private elasticsearchService: ElasticsearchService, public dialog: MatDialog) {
    this.mapSearch(0,4);
    this.length = this.states.length;
  }

  ngOnInit() {
  }


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
    else if(e.previousPageIndex > e.pageIndex)  {
      first = (e.pageIndex * e.pageSize);
      last = (e.previousPageIndex * e.pageSize);
      this.mapSearch(first, last)
    }
  }

  private _filterStates(value: string): State[] {
    const filterValue = value.toLowerCase();
    //this.elasticsearchService.search(filterValue).subscribe(response => {
    //  console.log(response);
    //})

    //DIALOG BOX
    var filteredOption = this.states.filter(state => state.name.toLowerCase().indexOf(filterValue) === 0);
    this.dialog.open(ElasticsearchDialogComponent, {
      width: '40%',
      height: 'auto',
      disableClose: false,      
      data: { name: filteredOption[0].name, population: filteredOption[0].population, flag: filteredOption[0].flag }
    });
       
    return filteredOption;
  }

  private mapSearch(first?: number, last?: number) {
    this.filteredStates = this.stateCtrl.valueChanges
      .pipe(
        startWith(''),
        map(state => state ? this._filterStates(state) : this.states.slice(first,last))
      );
  }
 }
