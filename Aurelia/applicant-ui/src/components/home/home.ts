import { I18N } from 'aurelia-i18n';
import { inject } from 'aurelia-dependency-injection';

@inject(I18N)
export class Home {
  constructor(private i18n: I18N) {
    // console.log(`hey ${this.i18n.tr('welcome')}`);
  }
}
