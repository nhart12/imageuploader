import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ImageUploadComponent } from '../image-uploader/image-uploader.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  constructor(private dialog: MatDialog) { }

  openUpload() {
    const dialogRef = this.dialog.open(ImageUploadComponent, {
      width: '300px',
      data: {}
    });

    dialogRef.afterClosed().subscribe(result => {
      //this.email = result;
      console.log("done uploading file")
    });
  }
}
