import { Component, EventEmitter, Output } from '@angular/core';
import { ImageUploadModel } from '../models/image-upload.model';
import { ImageUploadService } from '../services/image-uploader.service'
import { MatChipInputEvent } from '@angular/material';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'image-uploader',
  templateUrl: './image-uploader.component.html',
  styleUrls: ['./image-uploader.component.css']
})
export class ImageUploadComponent {


  constructor(private imageUploadService: ImageUploadService, private toastr: ToastrService) {
    this.model = new ImageUploadModel();
  }

  tagsSelectable = true;
  tagsRemovable = true;
  tagsAddOnBlur = true;
  loading: boolean = false;

  readonly tagsSeparatorKeysCodes: number[] = [ENTER, COMMA];
  public model: ImageUploadModel;
  @Output()
  cancel: EventEmitter<void> = new EventEmitter<void>();

  @Output()
  done: EventEmitter<string> = new EventEmitter<string>();

  addTag(event: MatChipInputEvent): void {
    const input = event.input;
    const value = event.value;

    if ((value || '').trim()) {
      this.model.tags.push(value.trim());
    }
    if (input) {
      input.value = '';
    }
  }

  removeTag(tag: string) {
    const index = this.model.tags.indexOf(tag);
    if (index >= 0) {
      this.model.tags.splice(index, 1);
    }
  }

  fileChange(event) {
    let fileList: FileList = event.target.files;
    if (fileList.length > 0) {
      this.model.file = fileList[0];
    }
  }

  onSave() {
    this.loading = true;
    this.imageUploadService.saveImage(this.model).subscribe(_ => {
      this.toastr.success(`Successfully triggered job to create package export!`);
      this.loading = false;
    },
      err => {
        //show error toast
        this.toastr.error("Error triggering job to create package export: " + err.message);
        this.loading = false;
      });
  }
}
