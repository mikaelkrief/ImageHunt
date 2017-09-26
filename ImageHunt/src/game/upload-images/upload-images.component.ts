import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from "@angular/router";
import { GameService } from "../../shared/services/game.service";

@Component({
  selector: 'app-upload-images',
  templateUrl: './upload-images.component.html',
  styleUrls: ['./upload-images.component.css']
})
export class UploadImagesComponent implements OnInit {
  @ViewChild('fileInput') fileInput;
  gameId: number;
  constructor(private _route: ActivatedRoute, private _gameService: GameService) { }

  ngOnInit() {
    this.gameId = this._route.snapshot.params["gameId"];
  }
  upload() {
    let fileBrowser = this.fileInput.nativeElement;
    if (fileBrowser.files) {
      this._gameService.upload(fileBrowser.files, this.gameId).subscribe(res => {
        // do stuff w/my uploaded file
      });
    }
  }
}
