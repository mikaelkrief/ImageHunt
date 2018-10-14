import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable()
export class ImageService {
    constructor(private http: HttpClient) {

  }
  uploadImage(file: File) {
    let headers = new HttpHeaders();
    headers.delete('Content-Type');
    const formData = new FormData();
    formData.append('file', file);
    let options = { headers: headers };

    return this.http.post('api/Image', formData, options);
  }
}
