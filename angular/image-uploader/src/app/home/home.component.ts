import { Component, Inject } from '@angular/core';
import { MatChip } from '@angular/material/chips';
import { MatDialog } from '@angular/material/dialog';
import { Observable } from 'rxjs/internal/Observable';
import { ImageUploadComponent } from '../image-uploader/image-uploader.component';
import { ImageMetadataModel } from '../services/image-api.model';
import { ImageUploadService } from '../services/image-uploader.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {
  constructor(private dialog: MatDialog, private imageUploadService: ImageUploadService, @Inject('BASE_URL') private baseUrl: string) { }

  imagesMetadata: Array<ImageMetadataModel>;
  allTags: Array<string>;
  selectedTags: Set<string> = new Set<string>();
  ngOnInit() {
    this.loadImages();
    this.loadTags();
  }

  loadTags() {
    this.imageUploadService.getAllTags().subscribe(res => {
      this.allTags = res;
    })
  }

  loadImages() {
    let obs: Observable<ImageMetadataModel[]>;
    if (this.selectedTags && this.selectedTags.size > 0) {
      obs = this.imageUploadService.getImagesWithTags(Array.from(this.selectedTags.values()));
    }
    else {
      obs = this.imageUploadService.getAllImages();
    }
    obs.subscribe(res => {
      this.imagesMetadata = res;
    })
  }

  getImagePermalink(img: ImageMetadataModel) {
    return `${this.baseUrl}api/images/${img.path}`;
  }

  toggleTagSelection(chip: MatChip) {
    chip.toggleSelected();
    if (chip.selected) {
      this.selectedTags.add(chip.value);
    }
    else {
      this.selectedTags.delete(chip.value);
    }
    this.loadImages()
  }

  openUpload() {
    const dialogRef = this.dialog.open(ImageUploadComponent, {
      width: '400px',
      data: {}
    });

    dialogRef.afterClosed().subscribe(result => {
      this.selectedTags.clear();
      this.loadImages();
      this.loadTags();
    });
  }

  delete(img: ImageMetadataModel) {
    this.imageUploadService.deleteImage(img.id).subscribe(res => {
      this.loadImages();
      this.loadTags();
    });
  }
}
