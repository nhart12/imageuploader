import { Component } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ImageUploadComponent } from '../image-uploader/image-uploader.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  constructor(private modalService: NgbModal) { }

  openUpload() {
    const modalRef = this.modalService.open(ImageUploadComponent, { size: 'lg' });
    modalRef.componentInstance.cancel.subscribe(() => modalRef.close());
    modalRef.componentInstance.done.subscribe((r) => {
      console.log(`Uploaded image ${r.Id}`)
      modalRef.close();
    });
  }
}
