import { Component, EventEmitter, Output } from '@angular/core';
import { ImageUploadModel } from '../models/image-upload.model';
import { ImageUploadService } from '../services/image-uploader.service'
import { MatChipInputEvent } from '@angular/material/chips';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { MatSnackBar } from '@angular/material/snack-bar';
import { FormControl, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
@Component({
  selector: 'image-uploader',
  templateUrl: './image-uploader.component.html',
  styleUrls: ['./image-uploader.component.scss']
})
export class ImageUploadComponent {


  constructor(private imageUploadService: ImageUploadService, private snackBar: MatSnackBar, public dialogRef: MatDialogRef<ImageUploadComponent>) {
    this.model = new ImageUploadModel();
  }
  fileFormControl = new FormControl('', [Validators.required]);
  tagsFormControl = new FormControl('', [Validators.required]);
  titleFormControl = new FormControl('', [Validators.required]);
  descriptionFormControl = new FormControl('', [Validators.required]);
  tagsSelectable = true;
  tagsRemovable = true;
  tagsAddOnBlur = true;
  loading: boolean = false;

  readonly tagsSeparatorKeysCodes: number[] = [ENTER, COMMA];
  public model: ImageUploadModel;

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
    this.imageUploadService.saveImage(this.model).subscribe(r => {
      this.loading = false;
      this.dialogRef.close();
    },
      err => {
        //show error toast
        this.snackBar.open("Error uploading image", '', {
          duration: 2000
        });
        this.loading = false;
      });
  }

  onCancel() {
    this.dialogRef.close();
  }
}
