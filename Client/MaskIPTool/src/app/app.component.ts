import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { saveAs } from "file-saver";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'MaskIPTool';
  fileReader: FileReader;
  myForm = new FormGroup({
    name: new FormControl('', [Validators.required, Validators.minLength(3)]),
    file: new FormControl('', [Validators.required]),
    fileSource: new FormControl('', [Validators.required])
  });
  selectedFile: File;
    
  constructor(private http: HttpClient) { }
      
  get f(){
    return this.myForm.controls;
  }
     
  onFileChange(fileInput: any) {
    this.selectedFile = <File>fileInput.target.files[0];
  }
     
  submit(data){
    const formData = new FormData();
    formData.append('Name', data.Name);
    formData.append('TileImage', this.selectedFile);
    this.http.post('https://localhost:44303/MaskIP/MaskIP', formData ,{responseType: "blob"})
    .subscribe(res => {
      if(res){
        saveAs(res, "Output");
      }
      alert('Uploaded!!');
    });
  }
}
