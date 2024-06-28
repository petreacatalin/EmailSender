import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MailRequest } from '../models/mailrequest';
import { EmailSendResult } from '../models/emailSendResult';

@Injectable({
  providedIn: 'root',
})
export class EmailService {
  private apiUrl = 'https://localhost:7243/api';

  constructor(private http: HttpClient) { }

  sendEmail(mailRequest: MailRequest): Observable<EmailSendResult> {
    return this.http.post<any>(`${this.apiUrl}/send`, mailRequest);
  }
}
