import { HttpClient, HttpEvent, HttpEventType, HttpParams, HttpRequest } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { flatMap } from 'rxjs/operators';

import { ImageUploadModel } from "../models/image-upload.model";
import { ImageMetadataModel } from "./image-api.model";

@Injectable({
  providedIn: 'root',
})
export class ImageUploadService {

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  saveImage(model: ImageUploadModel): Observable<ImageMetadataModel> {
    return this.uploadImage(model.file).pipe(flatMap((res) => {
      return this.saveImageMetadata(new ImageMetadataModel(model.title, model.description, model.tags), res);
    }));
  }

  private saveImageMetadata(model: ImageMetadataModel, id: string): Observable<ImageMetadataModel> {
    return this.http.post<ImageMetadataModel>(`${this.baseUrl}api/images/${id}/metadata/`,
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
