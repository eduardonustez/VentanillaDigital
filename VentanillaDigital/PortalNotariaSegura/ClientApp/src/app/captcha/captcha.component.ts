import { Component, OnInit, ViewChild } from '@angular/core';
import { HomeService } from '../home/home.service';
import {ReCaptcha2Component} from 'ngx-captcha';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-captcha',
  templateUrl: './captcha.component.html',
  styleUrls: ['./captcha.component.css']
})
export class CaptchaComponent implements OnInit {



  public aFormGroup: FormGroup;
 
  constructor(private dataService: HomeService,private formBuilder: FormBuilder) {}
 
  @ViewChild('captchaElem', { static: true })  captchaElem: ReCaptcha2Component;
  
  ngOnInit() {
    this.aFormGroup = this.formBuilder.group({
      recaptcha: ['', Validators.required]
    });
  }


  resolved(captchaResponse: string) {
    this.dataService.setResultCaptcha(captchaResponse);
  }

  cbExpiration(){
    this.dataService.setResultCaptcha('');
  }

  reset(){
    this.dataService.setResultCaptcha('');
    this.captchaElem.reloadCaptcha();
  }
}
