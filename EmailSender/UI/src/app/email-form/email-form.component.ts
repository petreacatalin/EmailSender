import {
  ChangeDetectorRef,
  Component,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { MailRequest } from '../models/mailrequest';
import { EmailService } from './email.service';
import { ChipListComponent } from '@syncfusion/ej2-angular-buttons';
import {
  AnimationSettingsModel,
  DialogComponent,
} from '@syncfusion/ej2-angular-popups';
import { tap } from 'rxjs';
import { EmailSendResult } from '../models/emailSendResult';
import { HttpErrorResponse, HttpResponse } from '@angular/common/http';

@Component({
  selector: 'app-email-form',
  templateUrl: './email-form.component.html',
  styleUrls: ['./email-form.component.scss'],
})
export class EmailFormComponent implements OnInit, OnDestroy {
  emailForm!: FormGroup;
  public emailRegex = new RegExp(
    '^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+.[a-zA-Z]{2,}$'
  );
  public emailGresit: boolean = false;
  emailListNotEmpty: boolean = false; // Variabilă pentru a indica dacă există adrese de email sau nu

  public today: Date = new Date();
  public currentYear: number = this.today.getFullYear();
  public currentMonth: number = this.today.getMonth();
  public currentDay: number = this.today.getDate();
  public currentHour: number = this.today.getHours();
  public currentMinute: number = this.today.getMinutes();
  public currentSecond: number = this.today.getSeconds();
  public date: Date = new Date(new Date().setDate(14));
  public minDate: Date = new Date(this.today);
  public maxDate: Date = new Date(
    this.currentYear,
    this.currentMonth,
    27,
    this.currentHour,
    this.currentMinute,
    this.currentSecond
  );
  buttonOpenDialog: boolean | undefined;
  buttons: unknown[] | undefined;
  modalHeader: string = 'Informare!';
  modalContent: string = ``;
  animationSettings: AnimationSettingsModel = { effect: 'Zoom' };
  responseBackEnd: EmailSendResult | undefined;
  @ViewChild('chips') public chips?: ChipListComponent;
  @ViewChild('saveModal') public saveModal: DialogComponent | undefined;

  get f() {
    return this.emailForm.controls;
  }

  constructor(
    private fb: FormBuilder,
    private emailService: EmailService,
    private cd: ChangeDetectorRef
  ) {
    this.buttons = [
      {
        click: this.hideSaveDialog.bind(this),
        buttonModel: {
          content: 'Închide',
        },
      },
    ];
  }

  ngOnInit(): void {
    this.createForm();
  }

  ngOnDestroy(): void {
    var len = this.chips!.chips.length;
    for (var i = 0; i < len; i++) {
      this.chips!.remove(0);
    }
  }

  createForm() {
    this.emailForm = this.fb.group({
      toEmail: [null, []],
      numberOfEmails: [1, Validators.required],
      startTime: [null],
      endTime: [null],
      secondsBetweenEmails: [2],
    });
  }

  sendEmails() {
    if (this.f['secondsBetweenEmails']?.value == null) {
      this.f['secondsBetweenEmails'].setValue(2);
      this.f['secondsBetweenEmails'].updateValueAndValidity();
    }
    if (this.emailForm.valid && this.chips?.chips?.length! !== 0) {
      const emails = this.chips?.chips.map((chip) => chip);
      this.emailGresit = false;
      if (emails && emails.length > 0) {
        this.emailForm.get('toEmail')!.setValue(emails);
        this.emailListNotEmpty = true;
        const mailRequest: MailRequest = this.emailForm.value;
        this.emailService
          .sendEmail(mailRequest)
          .pipe(
            tap((response: EmailSendResult) => {
              this.responseBackEnd = response;
              if (response.isSuccess) {
                this.modalContent =
                  this.f['numberOfEmails'].value == 1
                    ? `Va fi trimis ${this.f['numberOfEmails']!
                        .value!}  mail către următoarele adrese introduse: ${this.chips?.chips.join(
                        ', '
                      )}`
                    : `Vor fi trimise un număr de ${this.f['numberOfEmails']!
                        .value!} mail-uri către următoarele adrese introduse: ${this.chips?.chips.join(
                        ', '
                      )}`;
                this.onOpenModal();
              } else if (!response.isSuccess) {
                this.modalContent =
                  'A apărut o eroare la trimiterea email-urilor.';
                this.onOpenModal();
              }
            })
          )
          .subscribe();
        if (this.responseBackEnd == null) {
          this.modalContent =
            'A apărut o eroare la trimiterea email-urilor. Vă rugăm contactați administratorul.';
          this.onOpenModal();
        }
      }
    } else if (
      (this.emailForm.invalid || this.emailForm.valid) &&
      this.chips?.chips?.length! === 0
    ) {
      console.error('No email addresses to send.');
      this.emailListNotEmpty = false;
      return;
    }
    this.f['toEmail'].setValue(null);
    this.f['toEmail'].updateValueAndValidity();
  }

  addChips() {
    if (this.emailRegex.test(this.f['toEmail'].value)) {
      this.chips!.add(this.f['toEmail'].value);
      this.emailListNotEmpty = true;
      console.log(this.f['toEmail'].value);
      console.log(this.chips?.chips);
      this.f['toEmail'].setValue(null);
      this.f['toEmail'].updateValueAndValidity();
      this.emailGresit = false;
      this.cd.detectChanges();
    } else {
      this.emailGresit = true;
    }
  }

  onReset() {
    var len = this.chips!.chips.length;
    for (var i = 0; i < len; i++) {
      this.chips!.remove(0);
    }
    setTimeout(() => {
      this.f['numberOfEmails'].setValue(1);
      this.f['numberOfEmails'].updateValueAndValidity();
    }, 0);
    this.cd.detectChanges();
    this.emailListNotEmpty = false;
  }

  onOpenModal = (): void => {
    this.buttonOpenDialog = true;
    if (this.saveModal) this.saveModal.show();
  };

  hideSaveDialog() {
    this.saveModal?.hide();
  }
}
