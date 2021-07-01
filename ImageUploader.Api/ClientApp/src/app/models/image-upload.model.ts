export class ImageUploadModel {
  constructor() {
    this.tags = new Array<string>();
  }
  file: File;
  title: string;
  description: string;
  tags: Array<string>;

  valid(): boolean {
    return this.file && this.title && this.description && this.tags != null;
  }
}
