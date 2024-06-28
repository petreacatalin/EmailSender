export interface MailRequest {
    toEmail: string[];
    numberOfEmails: number;
    startTime?: Date;
    endTime?: Date;
    secondsBetweenEmails: number;
}