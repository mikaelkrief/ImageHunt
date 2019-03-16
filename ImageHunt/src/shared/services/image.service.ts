@Injectable()
export class ImageService {
  constructor(private http: HttpClient) {

  }

  uploadImage(file: File) {
    const headers = new HttpHeaders();
    headers.delete("Content-Type");
    const formData = new FormData();
    formData.append("file", file);
    const options = { headers: headers };

    return this.http.post("api/Image", formData, options);
  }
}
