import { ImageService } from "services/image.service";

@Component({
  selector: "upload-image",
  templateUrl: "./upload-image.component.html",
  styleUrls: ["./upload-image.component.scss"]
})
/** upload-image component*/
export class UploadImageComponent {
  /** upload-image ctor */
  @Output()
  pictureId = new EventEmitter<number>();

  constructor(public bsModalRef: BsModalRef, private _imageService: ImageService) {

  }

  uploadFiles(files) {
    this._imageService.uploadImage(files[0])
      .subscribe(res => {
        this.pictureId.emit((res) as any);
        this.bsModalRef.hide();
      });

  }

}
