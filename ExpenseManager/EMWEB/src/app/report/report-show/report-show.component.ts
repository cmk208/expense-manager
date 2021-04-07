import { Component, OnInit } from '@angular/core';
import { ChartType, ChartOptions } from 'chart.js';
import { SingleDataSet, Label, monkeyPatchChartJsLegend, monkeyPatchChartJsTooltip } from 'ng2-charts';
import { SharedService } from 'src/app/shared.service';
import { DatePipe } from '@angular/common';
import { TransactionType } from 'src/app/models/TransactionType';
import { APIResponse } from 'src/app/models/ApiResponse';
import { Transaction } from 'src/app/models/Transaction';
import { SearchTransaction } from 'src/app/models/searchTransaction';
import { Report } from 'src/app/models/report';

declare var $: any;

@Component({
  selector: 'app-report-show',
  templateUrl: './report-show.component.html',
  styleUrls: ['./report-show.component.css']
})
export class ReportShowComponent implements OnInit {

  pieChartOptions: ChartOptions = {
    responsive: true,
  };
  pieChartLabels: Label[] = [];
  pieChartData: SingleDataSet = [];
  pieChartType: ChartType = 'pie';
  pieChartLegend = true;
  pieChartPlugins = [];
  ReportList: Report[];
  TransactionTypeList:TransactionType[];
  SearchResult: boolean = false;
  TransactionTypeID:string;
  searchType: string;
  WYear:number;
  WeekNo:number;
  WStartDate:any;
  WEndDate:any;
  MYear:number;
  MMonth:number;
  Year:number;
  StartDate:Date;
  EndDate:Date;
  ModalTitle:string;
  SelectedTransactionList:Transaction[];
  searchNow:boolean = false;

  constructor(
    private service:SharedService,
    private datePipe: DatePipe
  ) {}

  ngOnInit() {
    this.searchNow = false;
    this.refreshTransactionTypeList();
  }

  refreshTransactionTypeList(transationTypeID?:string) {
    this.service.getTransactionTypeList().subscribe(response => {
      if(response.status == 200) {
        var apiResponse = new APIResponse<TransactionType[]>(response.body);

        this.TransactionTypeList = apiResponse.Content.filter(x => x.TransactionTypeName !== 'Transfer');

        if(transationTypeID) {
          this.TransactionTypeID = transationTypeID;
        }
      } else {
        alert('Error! Cannot get transaction type list.');
      }
    }, error => {
      alert(error.message);
    });
  }

  onSearchTypeSelected(searchType:string) {
    this.searchType = searchType;
  }

  onWeekNoSelected(weekNo:number) {
    this.WeekNo = weekNo;

    if(this.WYear) {
      let sd = this.getDateOfISOWeek(this.WeekNo, this.WYear);
      let ed = this.getDateOfISOWeek(this.WeekNo, this.WYear);
      ed = new Date(ed.setDate(ed.getDate() + 6));

      this.WStartDate = this.datePipe.transform(sd, 'yyyy-MM-dd');
      this.WEndDate = this.datePipe.transform(ed, 'yyyy-MM-dd');
    }
  }

  search() {
    $('#DivOverlay').show();

    var searchTransaction = new SearchTransaction();
    
    this.searchNow = true;

    switch(this.searchType) {
      case 'Weekly':
        if((this.TransactionTypeID != null && this.TransactionTypeID != '') &&
            this.WeekNo != null) {
          searchTransaction.TransactionTypeID = this.TransactionTypeID;
          searchTransaction.StartDate = this.WStartDate;
          searchTransaction.EndDate = this.WEndDate;
  
          var tt = '', sd = '', ed = '';
  
          if(searchTransaction.TransactionTypeID) {
            tt = searchTransaction.TransactionTypeID;
          }
  
          if(searchTransaction.StartDate) {
            sd = this.datePipe.transform(searchTransaction.StartDate, 'yyyy-MM-dd');
          }
          
          if(searchTransaction.EndDate) {
            ed = this.datePipe.transform(searchTransaction.EndDate, 'yyyy-MM-dd');
          }
  
          this.service.getReportListByDate(
            sd,
            ed,
            tt
          ).subscribe(response => {
            var apiResponse = new APIResponse<Report[]>(response.body);
  
            if(apiResponse.StatusCode == 200) {
              this.SearchResult = true;
  
              if(apiResponse.Content.length == 0) {
                this.SearchResult = false;
                
                $('#DivOverlay').hide();

                alert('Result not found.');
  
                return;
              }
  
                this.pieChartLabels = apiResponse.Content.reduce((newArr, obj) => (obj.CategoryName && newArr.push(obj.CategoryName), newArr), []);
                this.pieChartData = apiResponse.Content.reduce((newArr, obj) => (obj.Amount && newArr.push(obj.Amount.toFixed(2)), newArr), []);
  
                this.ReportList = apiResponse.Content;
            } else {
              this.SearchResult = false;
              
              alert('Result not found.');
            }

            $('#DivOverlay').hide();

            this.searchNow = false;
          }, error => {
            alert(error.message);

            $('#DivOverlay').hide();

            this.searchNow = false;
          }); 
        } else {
          $('#DivOverlay').hide();
        }
        break;
      case 'Monthly':
        if((this.TransactionTypeID != null && this.TransactionTypeID != '') &&
            this.MYear != null &&
            this.MMonth != null) {
          searchTransaction.TransactionTypeID = this.TransactionTypeID;
          searchTransaction.Year = this.MYear;
          searchTransaction.Month = this.MMonth;
  
          var tt = '', yr = '', mth = '';
  
          if(searchTransaction.TransactionTypeID) {
            tt = searchTransaction.TransactionTypeID;
          }
  
          if(searchTransaction.Year) {
            yr = searchTransaction.Year.toString();
          }
          
          if(searchTransaction.Month) {
            mth = searchTransaction.Month.toString();
          }
  
          this.service.getReportListByMonth(
            yr,
            mth,
            tt
          ).subscribe(response => {
            var apiResponse = new APIResponse<Report[]>(response.body);
  
            if(apiResponse.StatusCode == 200) {
              this.SearchResult = true;
  
              if(apiResponse.Content.length == 0) {
                this.SearchResult = false;
  
                $('#DivOverlay').hide();

                alert('Result not found.');
  
                return;
              }
  
                this.pieChartLabels = apiResponse.Content.reduce((newArr, obj) => (obj.CategoryName && newArr.push(obj.CategoryName), newArr), []);
                this.pieChartData = apiResponse.Content.reduce((newArr, obj) => (obj.Amount && newArr.push(obj.Amount.toFixed(2)), newArr), []);
  
                this.ReportList = apiResponse.Content;
            } else {
              this.SearchResult = false;
              
              alert('Result not found.');
            }

            $('#DivOverlay').hide();

            this.searchNow = false;
          }, error => {
            alert(error.message);

            $('#DivOverlay').hide();

            this.searchNow = false;
          });
        } else {
          $('#DivOverlay').hide();
        }
        break;
      case 'Annually':
        if((this.TransactionTypeID != null && this.TransactionTypeID != '') &&
            this.Year != null) {
          searchTransaction.TransactionTypeID = this.TransactionTypeID;
          searchTransaction.Year = this.Year;
  
          var tt = '', yr = '';
  
          if(searchTransaction.TransactionTypeID) {
            tt = searchTransaction.TransactionTypeID;
          }
  
          if(searchTransaction.Year) {
            yr = searchTransaction.Year.toString();
          }
  
          this.service.getReportListByYear(
            yr,
            tt
          ).subscribe(response => {
            var apiResponse = new APIResponse<Report[]>(response.body);
  
            if(apiResponse.StatusCode == 200) {
              this.SearchResult = true;
  
              if(apiResponse.Content.length == 0) {
                this.SearchResult = false;
  
                $('#DivOverlay').hide();

                alert('Result not found.');
  
                return;
              }
  
                this.pieChartLabels = apiResponse.Content.reduce((newArr, obj) => (obj.CategoryName && newArr.push(obj.CategoryName), newArr), []);
                this.pieChartData = apiResponse.Content.reduce((newArr, obj) => (obj.Amount && newArr.push(obj.Amount.toFixed(2)), newArr), []);
  
                this.ReportList = apiResponse.Content;
            } else {
              this.SearchResult = false;
              
              alert('Result not found.');
            }

            $('#DivOverlay').hide();

            this.searchNow = false;
          }, error => {
            alert(error.message);

            $('#DivOverlay').hide();

            this.searchNow = false;
          });
        } else {
          $('#DivOverlay').hide();
        }
        break;
      case 'DateRange':
        if((this.TransactionTypeID != null && this.TransactionTypeID != '') &&
            this.StartDate != null &&
            this.EndDate != null) {
          searchTransaction.TransactionTypeID = this.TransactionTypeID;
          searchTransaction.StartDate = this.StartDate;
          searchTransaction.EndDate = this.EndDate;
  
          var tt = '', sd = '', ed = '';
  
          if(searchTransaction.TransactionTypeID) {
            tt = searchTransaction.TransactionTypeID;
          }
  
          if(searchTransaction.StartDate) {
            sd = this.datePipe.transform(searchTransaction.StartDate, 'yyyy-MM-dd');
          }
          
          if(searchTransaction.EndDate) {
            ed = this.datePipe.transform(searchTransaction.EndDate, 'yyyy-MM-dd');
          }
  
          this.service.getReportListByDate(
            sd,
            ed,
            tt
          ).subscribe(response => {
            var apiResponse = new APIResponse<Report[]>(response.body);
  
            if(apiResponse.StatusCode == 200) {
              this.SearchResult = true;
  
              if(apiResponse.Content.length == 0) {
                this.SearchResult = false;
  
                $('#DivOverlay').hide();

                alert('Result not found.');
  
                return;
              }
  
                this.pieChartLabels = apiResponse.Content.reduce((newArr, obj) => (obj.CategoryName && newArr.push(obj.CategoryName), newArr), []);
                this.pieChartData = apiResponse.Content.reduce((newArr, obj) => (obj.Amount && newArr.push(obj.Amount.toFixed(2)), newArr), []);
  
                this.ReportList = apiResponse.Content;
            } else {
              this.SearchResult = false;
              
              alert('Result not found.');
            }
            
            $('#DivOverlay').hide();

            this.searchNow = false;
          }, error => {
            alert(error.message);
            
            $('#DivOverlay').hide();

            this.searchNow = false;
          });
        } else {
          $('#DivOverlay').hide();
        }
        break;
        default:
          $('#DivOverlay').hide();
          break;
    }

    monkeyPatchChartJsTooltip();
    monkeyPatchChartJsLegend();
  }

  viewTransaction(report:Report) {
    this.SelectedTransactionList = report.Transactions;
    this.ModalTitle = 'View Report In Details';
  }

  closeModal() {
  }

  getDateOfISOWeek(w, y) {
    var simple = new Date(y, 0, 1 + (w - 1) * 7);
    var dow = simple.getDay();
    var ISOweekStart = simple;

    if (dow <= 4) {
      ISOweekStart.setDate(simple.getDate() - simple.getDay() + 1);
    } else {
      ISOweekStart.setDate(simple.getDate() + 8 - simple.getDay());
    }

    return ISOweekStart;
  }
}
