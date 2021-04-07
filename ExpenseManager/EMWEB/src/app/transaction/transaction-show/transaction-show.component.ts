import { Component, OnInit } from '@angular/core';
import { SharedService } from 'src/app/shared.service';
import { APIResponse } from 'src/app/models/ApiResponse'; 
import { Transaction } from 'src/app/models/Transaction'; 
import { Category } from 'src/app/models/category'; 
import { TransactionType } from 'src/app/models/transactionType';
import { SearchTransaction } from 'src/app/models/searchTransaction';
import { DatePipe } from '@angular/common';

declare var $: any;

@Component({
  selector: 'app-transaction-show',
  templateUrl: './transaction-show.component.html',
  styleUrls: ['./transaction-show.component.css']
})
export class TransactionShowComponent implements OnInit {

  constructor(
    private service:SharedService,
    private datePipe: DatePipe
  ) { }

  TransactionList:Transaction[];
  TransactionTypeList:TransactionType[];
  FromAccountList:Account[];
  CategoryList:Category[];
  searchType:string;
  TransactionTypeID:string;
  TransactionTypeName:string;
  WYear:number;
  WeekNo:number;
  WStartDate:any;
  WEndDate:any;
  MYear:number;
  MMonth:number;
  Year:number;
  StartDate:Date;
  EndDate:Date;
  FromAccountID:string;
  CategoryID:string;
  Min:number;
  Max:number;
  transaction:Transaction;
  ModalTitle:string;
  activateAddEditTransaction:boolean;
  searchNow:boolean = false;

  ngOnInit() {
    const component = this;

    component.searchNow = false;
    component.refreshTransactionTypeList();
    component.refreshFromAccountList();

    $('#transactionModal').on('hidden.bs.modal', function () {
      component.activateAddEditTransaction = false;
      component.transaction = new Transaction();

      component.TransactionList = [];
    });
  }

  onSearchTypeSelected(searchType:string) {
    this.searchType = searchType;
  }

  refreshTransactionTypeList(transationTypeID?:string) {
    this.service.getTransactionTypeList().subscribe(response => {
      if(response.status == 200) {
        var apiResponse = new APIResponse<TransactionType[]>(response.body);

        this.TransactionTypeList = apiResponse.Content;

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

  onTransactionTypeSelected(event:Event) {
    let selectedOptions = event.target['options'];
    let selectedIndex = selectedOptions.selectedIndex;
    let selectElementText = selectedOptions[selectedIndex].text;
    
    this.TransactionTypeID = this.TransactionTypeID;
    this.TransactionTypeName = selectElementText;

    this.refreshCategoryList(this.TransactionTypeID);
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

  refreshFromAccountList(fromAccountID?:string) {
    this.service.getAccountList().subscribe(response => {
      if(response.status == 200) {
        var apiResponse = new APIResponse<Account[]>(response.body);

        this.FromAccountList = apiResponse.Content;

        if(fromAccountID) {
          this.FromAccountID = fromAccountID;
        }
      } else {
        alert('Error! Cannot get from account list.');
      }
    }, error => {
      alert(error.message);
    });
  }

  refreshCategoryList(transactionTypeID?:string, categoryID?:string) {
    this.service.getCategoryList(transactionTypeID).subscribe(response => {
      if(response.status == 200) {
        var apiResponse = new APIResponse<Category[]>(response.body);

        this.CategoryList = apiResponse.Content;

        if(categoryID) {
          this.CategoryID = categoryID;
        }
      } else {
        alert('Error! Cannot get category list.');
      }
    }, error => {
      alert(error.message);
    });
  }

  addTransaction() {
    $('#transactionModal').find('input[type="text"]').val('');
    $('#transactionModal').find('select').val('');

    this.transaction = new Transaction({
      TransactionID: '0',
      TransactionTypeID: '',
      FromAccountID: '',
      ToAccountID: '',
      CategoryID: '',
      TransactionDate: '',
      TransactionTime: '',
      Amount: '',
      Note: '',
      TransactionIsEdit: false
    });
    this.ModalTitle = 'Add Transaction';
    this.activateAddEditTransaction = true;
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
          searchTransaction.FromAccountID = this.FromAccountID;
          searchTransaction.CategoryID = this.CategoryID;
          searchTransaction.Min = this.Min;
          searchTransaction.Max = this.Max;
  
          var tt = '', fa = '', ca = '', sd = '', ed = '', strMin = '', strMax = '';
  
          if(searchTransaction.TransactionTypeID) {
            tt = searchTransaction.TransactionTypeID;
          }
  
          if(searchTransaction.FromAccountID) {
            fa = searchTransaction.FromAccountID;
          }
  
          if(searchTransaction.CategoryID) {
            ca = searchTransaction.CategoryID;
          }
  
          if(searchTransaction.StartDate) {
            sd = this.datePipe.transform(searchTransaction.StartDate, 'yyyy-MM-dd');
          }
          
          if(searchTransaction.EndDate) {
            ed = this.datePipe.transform(searchTransaction.EndDate, 'yyyy-MM-dd');
          }
          
          if(searchTransaction.Min) {
            strMin = searchTransaction.Min.toString();
          }
          
          if(searchTransaction.Max) {
            strMax = searchTransaction.Max.toString();
          }
  
          this.service.getTransactionListByDate(
            sd,
            ed,
            tt,
            fa,
            ca,
            strMin,
            strMax
          ).subscribe(response => {
            var apiResponse = new APIResponse<Transaction[]>(response.body);
  
            if(apiResponse.StatusCode == 200) {
              this.TransactionList = apiResponse.Content;
  
              if(this.TransactionList.length == 0) {
                alert('Transaction not found by selected search criteria.')
              }
            } else {
              alert('Transaction not found.');
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
          searchTransaction.FromAccountID = this.FromAccountID;
          searchTransaction.CategoryID = this.CategoryID;
          searchTransaction.Min = this.Min;
          searchTransaction.Max = this.Max;
  
          var tt = '', fa = '', ca = '', yr = '', mth = '', strMin = '', strMax = '';
  
          if(searchTransaction.TransactionTypeID) {
            tt = searchTransaction.TransactionTypeID;
          }
  
          if(searchTransaction.FromAccountID) {
            fa = searchTransaction.FromAccountID;
          }
  
          if(searchTransaction.CategoryID) {
            ca = searchTransaction.CategoryID;
          }
  
          if(searchTransaction.Year) {
            yr = searchTransaction.Year.toString();
          }
          
          if(searchTransaction.Month) {
            mth = searchTransaction.Month.toString();
          }
          
          if(searchTransaction.Min) {
            strMin = searchTransaction.Min.toString();
          }
          
          if(searchTransaction.Max) {
            strMax = searchTransaction.Max.toString();
          }
  
          this.service.getTransactionListByMonth(
            yr,
            mth,
            tt,
            fa,
            ca,
            strMin,
            strMax
          ).subscribe(response => {
            var apiResponse = new APIResponse<Transaction[]>(response.body);
  
            if(apiResponse.StatusCode == 200) {
              this.TransactionList = apiResponse.Content;
  
              if(this.TransactionList.length == 0) {
                alert('Transaction not found by selected search criteria.')
              }
            } else {
              alert('Transaction not found.');
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
          searchTransaction.FromAccountID = this.FromAccountID;
          searchTransaction.CategoryID = this.CategoryID;
          searchTransaction.Min = this.Min;
          searchTransaction.Max = this.Max;
  
          var tt = '', fa = '', ca = '', yr = '', strMin = '', strMax = '';
  
          if(searchTransaction.TransactionTypeID) {
            tt = searchTransaction.TransactionTypeID;
          }
  
          if(searchTransaction.FromAccountID) {
            fa = searchTransaction.FromAccountID;
          }
  
          if(searchTransaction.CategoryID) {
            ca = searchTransaction.CategoryID;
          }
  
          if(searchTransaction.Year) {
            yr = searchTransaction.Year.toString();
          }
  
          if(searchTransaction.Min) {
            strMin = searchTransaction.Min.toString();
          }
          
          if(searchTransaction.Max) {
            strMax = searchTransaction.Max.toString();
          }
  
          this.service.getTransactionListByYear(
            yr,
            tt,
            fa,
            ca,
            strMin,
            strMax
          ).subscribe(response => {
            var apiResponse = new APIResponse<Transaction[]>(response.body);
  
            if(apiResponse.StatusCode == 200) {
              this.TransactionList = apiResponse.Content;
  
              if(this.TransactionList.length == 0) {
                alert('Transaction not found by selected search criteria.')
              }
            } else {
              alert('Transaction not found.');
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
          searchTransaction.FromAccountID = this.FromAccountID;
          searchTransaction.CategoryID = this.CategoryID;
          searchTransaction.Min = this.Min;
          searchTransaction.Max = this.Max;
  
          var tt = '', fa = '', ca = '', sd = '', ed = '', strMin = '', strMax = '';
  
          if(searchTransaction.TransactionTypeID) {
            tt = searchTransaction.TransactionTypeID;
          }
  
          if(searchTransaction.FromAccountID) {
            fa = searchTransaction.FromAccountID;
          }
  
          if(searchTransaction.CategoryID) {
            ca = searchTransaction.CategoryID;
          }
  
          if(searchTransaction.StartDate) {
            sd = this.datePipe.transform(searchTransaction.StartDate, 'yyyy-MM-dd');
          }
          
          if(searchTransaction.EndDate) {
            ed = this.datePipe.transform(searchTransaction.EndDate, 'yyyy-MM-dd');
          }
          
          if(searchTransaction.Min) {
            strMin = searchTransaction.Min.toString();
          }
          
          if(searchTransaction.Max) {
            strMax = searchTransaction.Max.toString();
          }
  
          this.service.getTransactionListByDate(
            sd,
            ed,
            tt,
            fa,
            ca,
            strMin,
            strMax
          ).subscribe(response => {
            var apiResponse = new APIResponse<Transaction[]>(response.body);
  
            if(apiResponse.StatusCode == 200) {
              this.TransactionList = apiResponse.Content;
  
              if(this.TransactionList.length == 0) {
                alert('Transaction not found by selected search criteria.')
              }
            } else {
              alert('Transaction not found.');
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
  }

  editTransaction(transaction:Transaction) {
    transaction.TransactionIsEdit = true;
    this.transaction = transaction;
    this.ModalTitle = 'Edit Transaction';
    this.activateAddEditTransaction = true;
  }

  deleteTransaction(transaction:Transaction) {
    $('#DivOverlay').show();

    if(confirm('Are you sure want to delete?')) {
      this.service.deleteTransaction(transaction.TransactionID).subscribe(response => {
        var apiResponse = new APIResponse<Transaction>(response);
  
        if(apiResponse.StatusCode == 200) {
          this.transaction = new Transaction();
  
          alert("Successful deleted new Transaction.");

          $('#DivOverlay').hide();

          this.TransactionList = [];
        }
      }, error => {
        alert(error.message);

        $('#DivOverlay').hide();
      })
    } else {
      $('#DivOverlay').hide();
    }
  }

  closeModal() {
    this.activateAddEditTransaction = false;
    this.transaction = new Transaction();
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
