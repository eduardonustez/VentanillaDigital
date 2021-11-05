export class util {
    
    static date_regex(newValue){
        var date_regex = /(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d/;
        if (date_regex.test(newValue)) {
            return true;
        }
        return false;
      }
      
    
    static parseFormatDatePicker(value) {
        if (value) {
          if (Object.keys(value).length === 3 && this.isNumber(value.month) && this.isNumber(value.day) && this.isNumber(value.year)) {
            return  `${this.isNumber(value.month) ? this.padNumber(value.month) : ''}/${this.isNumber(value.day) ? this.padNumber(value.day) : ''}/${value.year}`;
          }
        }
        return null;
      }
    
    static   isNumber(value: any): value is number {
        return !isNaN(this.toInteger(value));
      }
      
    static  isInteger(value: any): value is number {
        return typeof value === 'number' && isFinite(value) && Math.floor(value) === value;
      }
      
    static  isDefined(value: any): boolean {
        return value !== undefined && value !== null;
      }
      
    static  padNumber(value: number) {
        if (this.isNumber(value)) {
          return `0${value}`.slice(-2);
        } else {
          return '';
        }
      }
    
    static   toInteger(value: any): number {
        return parseInt(`${value}`, 10);
      }
      
    static  isString(x: any): x is string {
        return typeof x === "string";
      }
}