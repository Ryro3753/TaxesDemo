import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http'
import { MatDialog } from '@angular/material/dialog';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { TaxesItem } from './model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'TaxesClient';
  displayedColumns: string[] = ['Municipality', 'Date', 'TaxesRatio', 'TaxesSchedule'];
  loadedTaxes: any;
  addTaxesModel: TaxesItem = { id: 0, taxesRatio: 0, municipality: '', date: undefined, taxesSchedule: undefined }
  constructor(readonly httpClient: HttpClient, readonly dialog: MatDialog) {

  }

  list() {
    this.httpClient.get('http://localhost:5005/api/Taxes').subscribe(i => {
      this.loadedTaxes = i;
    })
  }

  ngOnInit() {

    this.list()
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(TaxesAddDialog, {
      width: '250px',
      data: { id: 0, taxesRatio: 0, municipality: '', date: "2020-10-03", taxesSchedule: undefined }
    });

    dialogRef.afterClosed().subscribe(result => {
      this.addTaxesModel = result;
      this.httpClient.post('http://localhost:5005/api/Taxes', this.addTaxesModel).subscribe(i => {
        this.list()
      })


    });
  }

}
@Component({
  selector: 'taxes-add-dialog.html',
  templateUrl: 'taxes-add-dialog.html',
})
export class TaxesAddDialog {

  constructor(
    public dialogRef: MatDialogRef<TaxesAddDialog>,
    @Inject(MAT_DIALOG_DATA) public data: TaxesItem) { }

  onNoClick(): void {
    this.dialogRef.close();
  }

}