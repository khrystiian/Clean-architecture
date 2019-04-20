import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Leg } from '../../shared/models/TripRootObject';

@Component({
  selector: 'app-elasticsearch-dialog',
  templateUrl: './elasticsearch-dialog.component.html',
  styleUrls: ['./elasticsearch-dialog.component.css']
})
export class ElasticsearchDialogComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<ElasticsearchDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Leg) { }

  onNoClick(): void {
    this.dialogRef.close();
  }

  ngOnInit() {
  }

}
