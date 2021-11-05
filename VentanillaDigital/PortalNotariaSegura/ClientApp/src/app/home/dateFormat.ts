import { Injectable } from "@angular/core";
import { NgbDateParserFormatter, NgbDateStruct } from "@ng-bootstrap/ng-bootstrap";
import { util } from '../utils/utils';

//your NgbDateFRParserFormater extends from NgbDateParserFormatter
//Is a Injectable that have two functions
@Injectable()
export class NgbDateESParserFormatter extends NgbDateParserFormatter {

  parse(value: string): NgbDateStruct {
    if (value) {
      const dateParts = value.trim().split('/');
      if (dateParts.length === 1 && util.isNumber(dateParts[0])) {
        return { day: null, month: util.toInteger(dateParts[1]), year: null };
      } else if (dateParts.length === 2 && util.isNumber(dateParts[0]) && util.isNumber(dateParts[1])) {
        return { day: util.toInteger(dateParts[0]), month: util.toInteger(dateParts[1]),  year: null };
      } else if (dateParts.length === 3 && util.isNumber(dateParts[0]) && util.isNumber(dateParts[1]) && util.isNumber(dateParts[2])) {
        return { day: util.toInteger(dateParts[0]),  month: util.toInteger(dateParts[1]), year: util.toInteger(dateParts[2]) };
      }
    }
    return null;
  }

  format(date: NgbDateStruct): string {
    return date ?
      `${util.isNumber(date.day) ? util.padNumber(date.day) : ''}/${util.isNumber(date.month) ? util.padNumber(date.month) : ''}/${date.year}` :
      '';
  }
}
