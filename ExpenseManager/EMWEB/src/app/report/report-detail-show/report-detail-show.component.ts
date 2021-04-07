import { Component, Input, OnInit } from '@angular/core';
import { DatePipe } from '@angular/common';
import { Transaction } from 'src/app/models/Transaction';

@Component({
  selector: 'app-report-detail-show',
  templateUrl: './report-detail-show.component.html',
  styleUrls: ['./report-detail-show.component.css']
})
export class ReportDetailShowComponent implements OnInit {

  @Input() SelectedTransactionList:Transaction[];

  constructor(
    private datePipe: DatePipe
  ) { }

  ngOnInit() {
  }
}
