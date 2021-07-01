export class ImageMetadataModel {
  constructor(
    title: string,
    description: string,
    tags: Array<string>
  ) {
    this.title = title;
    this.tags = tags;
    this.description = description;
  }

  title: string
  description: string
  tags: Array<string>
  id: string
  path: string
  fileName: string
}
