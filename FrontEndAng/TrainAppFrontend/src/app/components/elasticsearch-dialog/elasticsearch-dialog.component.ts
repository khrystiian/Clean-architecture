import { Component, OnInit, Inject } from '@angular/core';
import { DialogData } from '../elasticsearch/elasticsearch.component';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'app-elasticsearch-dialog',
  templateUrl: './elasticsearch-dialog.component.html',
  styleUrls: ['./elasticsearch-dialog.component.css']
})
export class ElasticsearchDialogComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<ElasticsearchDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData) { }

  onNoClick(): void {
    this.dialogRef.close();
  }

  ngOnInit() {
  }

}
