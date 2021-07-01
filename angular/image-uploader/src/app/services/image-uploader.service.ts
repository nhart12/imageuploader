import { HttpClient, HttpEvent, HttpEventType, HttpParams, HttpRequest } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { mergeMap } from 'rxjs/operators';

import { ImageUploadModel } from "../models/image-upload.model";
import { ImageMetadataModel } from "./image-api.model";

@Injectable({
  providedIn: 'root',
})
export class ImageUploadService {

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  saveImage(model: ImageUploadModel): Observable<ImageMetadataModel> {
    return this.uploadImage(model.file).pipe(mergeMap((res) => {
      return this.saveImageMetadata(new ImageMetadataModel(model.title, model.description, model.tags), res);
    }));
  }

  getAllImages(): Observable<Array<ImageMetadataModel>> {
    return this.http.get<Array<ImageMetadataModel>>(`${this.baseUrl}api/images`);
  }

  getAllTags(): Observable<Array<string>> {
    return this.http.get<Array<string>>(`${this.baseUrl}api/tags`);
  }

  getImagesWithTags(tags: Array<string>): Observable<Array<ImageMetadataModel>> {
    const params = new HttpParams({
      fromObject: { 'tags': tags }
    });
    return this.http.get<Array<ImageMetadataModel>>(`${this.baseUrl}api/images/search`, { params });
  }

  deleteImage(id: string): Observable<Object> {
    return this.http.delete(`${this.baseUrl}api/images/${id}`);
  }

  private saveImageMetadata(model: ImageMetadataModel, id: string): Observable<ImageMetadataModel> {
    return this.http.post<ImageMetadataModel>(`${this.baseUrl}api/images/${id}/metadata`,
      model
    );
  }

  private uploadImage(file: File): Observable<string> {
    const formData: FormData = new FormData();
    formData.append('file', file, file.name);
    return this.http
      .post<string>(`${this.baseUrl}api/images`, formData);
  }
}
