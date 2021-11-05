import { ElementRef, OnInit } from '@angular/core';
import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { HomeService } from './home.service';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { FormControl } from '@angular/forms';
import { ViewChild } from '@angular/core';
import { NgbDateParserFormatter, NgbDatepickerI18n, NgbTooltip } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectComponent, NgSelectConfig } from '@ng-select/ng-select';
import { CaptchaComponent } from '../captcha/captcha.component';
import { HomeI18n, I18n } from './home-i18n';
import { NgbDateESParserFormatter } from './dateFormat';
import { util } from '../utils/utils';
import { Notaria } from '../models/notaria.model';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  providers: [I18n, [{provide: NgbDatepickerI18n, useClass: HomeI18n},{provide: NgbDateParserFormatter, useClass: NgbDateESParserFormatter}]]
})

export class HomeComponent implements OnInit 
{  
  departments$: Observable<any[]>;
  notaries$: Observable<any[]>;
  cities: any[];
  notarias$: Observable<Notaria[]>;
  notariesArray: Notaria[];
  isErrorDate: boolean;
  isErrorDpto: boolean;
  isErrorCity: boolean;
  isErrorNotaria: boolean;
  isErrorEmail: boolean;
  isErrorNut: boolean;
  isErrorCaptcha: boolean;
  showResultOk: boolean;
  showResultFail: boolean;
  modelDatePicker;
  message: string;
  showMessage: boolean;
  messageEmail: string;
  showMessageEmail: boolean;
  btn_load: boolean;
  
  date: string;
  selectedDepartment;
  selectedCity;
  selectedNotary;
  email: string;
  nut: string;

  constructor(private dataService: HomeService, private fb: FormBuilder, private config:NgSelectConfig) { 
    this.config.notFoundText = "No hay coincidencias";
  }

  @ViewChild('triggerdate', { static: true }) tooltipDate: NgbTooltip;
  @ViewChild('triggerdateinput', { static: true }) dateInputElement: ElementRef;

  @ViewChild('triggerdpto', { static: true }) tooltipDpto: NgbTooltip;
  @ViewChild('triggerdptoinput', { static: true }) dptoInputElement: NgSelectComponent;

  @ViewChild('triggercity', { static: true }) tooltipCity: NgbTooltip;
  @ViewChild('triggercityinput', { static: true }) cityInputElement: NgSelectComponent;

  @ViewChild('triggernotaria', { static: true }) tooltipNotaria: NgbTooltip;
  @ViewChild('triggernotariainput', { static: true }) notariaInputElement: NgSelectComponent;

  @ViewChild('triggeremail', { static: true }) tooltipEmail: NgbTooltip;
  @ViewChild('triggeremailinput', { static: true }) emailInputElement: ElementRef;

  @ViewChild('triggernut', { static: true }) tooltipNut: NgbTooltip;
  @ViewChild('triggernutinput', { static: true }) nutInputElement: ElementRef;

  @ViewChild('CaptchaComponent', { static: true }) childCaptchaComponent: CaptchaComponent;
  
  ngOnInit() {
    this.showMessage = false;
    this.showMessageEmail = false;
    this.btn_load = false;
    this.setUpFlagError();
    this.departments$ = this.dataService.getDepartmentsAndCities();
    this.notarias$ = this.dataService.NotariasArray;
  }

  onChangeDepartment(newValue) {

    this.cities = [];
    this.selectedCity = null;
    this.selectedNotary = null;    
    this.notariesArray = null;
    if (newValue == undefined || newValue == null) return;
    this.departments$.pipe(
      map(items =>
        items.filter(item => item.departamento === newValue))
    )
      .subscribe(results => {
        this.cities = results[0].ciudades;
      });
  }

  onChangeCity(newValue) {

    this.notariesArray = [];
    this.selectedNotary = null;
    if (newValue == undefined || newValue == null) return;

    this.notarias$
    .pipe(
      map(items =>
        items.filter(item => newValue === "Bogotá D.C." ? item.ciudad === newValue : 
            item.ciudad === newValue && item.departamento === this.selectedDepartment))        
      )
    .subscribe(results => {
      results.forEach(element => {
        element.nombre = element.nombre.toUpperCase();
      });
      this.notariesArray = results;
    });
  }

  setUpFlagError(){
    this.isErrorDate = false; 
    this.isErrorDpto = false; 
    this.isErrorCity = false; 
    this.isErrorNotaria = false; 
    this.isErrorEmail = false; 
    this.isErrorNut = false; 
    this.isErrorCaptcha = false;
    this.showResultFail = false;
  }

  validationFunction() {
    
    this.setUpFlagError();
    if(this.validateFields())
    { 
      this.btn_load = true;
      this.getActa();                
    }
  }
  
  getActa() {

    setTimeout(() => 
    {  
        this.dataService.getActaNotarial(this.selectedNotary,this.date, this.email, this.nut).subscribe(result =>{
        if (result.data != "Acta no encontrada" && 
            result.data != "No se encuentra el acta notarial" &&
            !result.data.includes("!DOCTYPE html")){

        this.btn_load = false;
        this.showResultOk = true;

        const downloadLink = document.createElement("a");
        const fileName = result.tramiteId+".pdf";
        downloadLink.href = "data:application/pdf;base64," + result.data;
        downloadLink.download = fileName;
        downloadLink.click();
        this.cleanFields();
        }else{
          this.btn_load = false;
          this.showResultFail = true;
        }      
      },err => {this.btn_load = false; console.error(err), this.showResultFail = true;});  
    },
    1000);  
    
    setTimeout(() => 
    {  
      this.showResultOk = false;
    },
    10000);
  }
  
  selectDatePicker(value) {    
    this.date = value ? value.year + "-" + ('0' + value.month).slice(-2) + "-" + ('0' + value.day).slice(-2) : null;
  }

  parseFormatToController(value: string) {
    const dateParts = value.trim().split('/');
    return dateParts[2]​​​​​​​+"-"+dateParts[0]+"​​​​​​​-"+dateParts[1]​​​​;
  }​​​​​​​
  

  validateFields()
  {
    let result = false;
    if (this.date == null || this.date === undefined || this.date == '') {
      this.isErrorDate = true;
      this.tooltipDate.open();
      setTimeout(() => {
        this.dateInputElement.nativeElement.focus();
      }, 0);
    } else if (this.selectedDepartment == null || this.selectedDepartment === undefined || this.selectedDepartment == '') {
      this.isErrorDpto = true;
      this.tooltipDpto.open();

      setTimeout(() => {
        this.dptoInputElement.focus();
      }, 0);
    } else if (this.selectedCity == null || this.selectedCity === undefined || this.selectedCity == '') {

      this.isErrorCity = true;
      this.tooltipCity.open();

      setTimeout(() => {
        this.cityInputElement.focus();
      }, 0);
    } else if (this.selectedNotary == null || this.selectedNotary === undefined || this.selectedNotary == '') {

      this.isErrorNotaria = true;
      this.tooltipNotaria.open();

      setTimeout(() => {
        this.notariaInputElement.focus();
      }, 0);
    } else if (this.email === undefined || this.email == '') {
      this.isErrorEmail = true;
      this.tooltipEmail.open();

      setTimeout(() => {
        this.emailInputElement.nativeElement.focus();
      }, 0);
    } else if (this.nut === undefined || this.nut == '') {
      this.isErrorNut = true;
      this.tooltipNut.open();

      setTimeout(() => {
        this.nutInputElement.nativeElement.focus();
      }, 0);
    }else if(!this.dataService.getResultCaptcha()){
        this.isErrorCaptcha = true;
      }else{
      
      result = true;
    }    
    
    return result; 
  }

  validateFormatNut()
  {
    const numb = /^[0-9]{3,100}$/i;

    if(numb.test(this.nut))
    {
      this.message = "El código Nut debe contener al menos una letra.";
      this.showMessage = true;
      return;
    }
    this.message = "";
    this.showMessage = false;
  }

  validateEmail()
  {
    const emailFormat = /^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/;
    
    if(!emailFormat.test(this.email))
    {
      this.messageEmail = "El email no es correcto.";
      this.showMessageEmail = true;
      return;
    }
    this.messageEmail = "";
    this.showMessageEmail = false;
  }

  cleanFields() {
    if(!this.showResultFail){
      this.modelDatePicker = null; 
      this.selectedDepartment = null;
      this.selectedCity = null; 
      this.selectedNotary = null; 
      this.email = ""; 
      this.nut = ""; 
      this.childCaptchaComponent.reset();
    }      
  }  
}
