import { Component, OnInit, Input } from '@angular/core';
import { SharedService } from 'src/app/shared.service';
import { APIResponse } from 'src/app/models/ApiResponse';
import { Category } from 'src/app/models/Category'; 
import { TransactionType } from 'src/app/models/TransactionType'; 

declare var $: any;

@Component({
  selector: 'app-category-add-edit',
  templateUrl: './category-add-edit.component.html',
  styleUrls: ['./category-add-edit.component.css']
})
export class CategoryAddEditComponent implements OnInit {

  constructor(
    private service:SharedService
  ) { }

  @Input() category:Category;
  TransactionTypeList:TransactionType[];
  CategoryID:string;
  TransactionTypeID:string;
  CategoryName:string;
  Sequence:number;
  submit:boolean=false;

  ngOnInit() {
    this.submit = false;

    this.refreshTransactionTypeList(this.category.TransactionTypeID);

    this.CategoryID = this.category.CategoryID;
    this.CategoryName = this.category.CategoryName;
    this.Sequence = this.category.Sequence;
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

  addCategory() {
    $('#DivOverlay').show();

    this.category.TransactionTypeID = this.TransactionTypeID;
    this.category.CategoryName = this.CategoryName;
    this.category.Sequence = this.Sequence;
    this.submit = true;

    if((this.TransactionTypeID != null && this.TransactionTypeID != '') &&
       (this.CategoryName != null && this.CategoryName != '') &&
        this.Sequence != null) {
      this.service.addCategory(this.category).subscribe(response => {
        var apiResponse = new APIResponse<Category>(response);
  
        if(apiResponse.StatusCode == 200) {
          this.category = new Category();
  
          alert("Successful added new Category.");
  
          $('#categoryModal').modal('hide');
          $('#DivOverlay').hide();
  
          this.submit = false;
        }
      }, error => {
        alert(error.message);
        
        $('#DivOverlay').hide();

        this.submit = false;
      });
    } else {
      $('#DivOverlay').hide();
    }
  }

  editCategory() {
    $('#DivOverlay').show();

    this.category.CategoryID = this.CategoryID;
    this.category.TransactionTypeID = this.TransactionTypeID;
    this.category.CategoryName = this.CategoryName;
    this.category.Sequence = this.Sequence;
    this.submit = true;

    if((this.CategoryID != null && this.CategoryID != '') &&
       (this.TransactionTypeID != null && this.TransactionTypeID != '') &&
       (this.CategoryName != null && this.CategoryName != '') &&
        this.Sequence != null) {
      this.service.editCategory(this.category).subscribe(response => {
        var apiResponse = new APIResponse<Category>(response);
  
        if(apiResponse.StatusCode == 200) {
          this.category = new Category();
  
          alert("Successful edited new Category.");
  
          $('#categoryModal').modal('hide');
          $('#DivOverlay').hide();

          this.submit = false;
        }
      }, error => {
        alert(error.message);

        $('#DivOverlay').hide();

        this.submit = false;
      });
    } else {
      $('#DivOverlay').hide();
    }
  }
}
