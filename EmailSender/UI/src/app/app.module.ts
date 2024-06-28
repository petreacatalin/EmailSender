import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { DropDownListModule } from '@syncfusion/ej2-angular-dropdowns';
import { AppComponent } from './app.component';
import { EmailFormComponent } from './email-form/email-form.component';
import {
  NumericTextBoxModule,
  TextBoxModule,
} from '@syncfusion/ej2-angular-inputs';
import {
  DatePickerModule,
  DateTimePickerModule,
  TimePickerModule,
} from '@syncfusion/ej2-angular-calendars';
import { EmailService } from './email-form/email.service';
import { CalendarModule } from '@syncfusion/ej2-angular-calendars';
import { ButtonModule } from '@syncfusion/ej2-angular-buttons';
import { ChipListModule } from '@syncfusion/ej2-angular-buttons';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { TooltipModule } from '@syncfusion/ej2-angular-popups';
import { DialogModule } from '@syncfusion/ej2-angular-popups';

@NgModule({
  declarations: [AppComponent, EmailFormComponent],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    DropDownListModule,
    DatePickerModule,
    NumericTextBoxModule,
    TextBoxModule,
    ButtonModule,
    DateTimePickerModule,
    TimePickerModule,
    CalendarModule,
    ChipListModule,
    BrowserAnimationsModule,
    TooltipModule,
    DialogModule,
  ],
  providers: [EmailService],
  exports: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
