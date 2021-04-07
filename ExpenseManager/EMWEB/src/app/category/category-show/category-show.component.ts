import { Component, OnInit } from '@angular/core';
import { SharedService } from 'src/app/shared.service';
import { APIResponse } from 'src/app/models/ApiResponse'; 
import { Category } from 'src/app/models/Category'; 

declare var $: any;

@Component({
  selector: 'app-category-show',
  templateUrl: './category-show.component.html',
  styleUrls: ['./category-show.component.css']
})
export class CategoryShowComponent implements OnInit {

  constructor(
    private service:SharedService
  ) { }

  CategoryList:Category[];
  category:Category;
  ModalTitle:string;
  activateAddEditCategory:boolean;

  ngOnInit() {
    const component = this;

    component.refreshCategoryList('');

    $('#categoryModal').on('hidden.bs.modal', function () {
      component.activateAddEditCategory = false;
      component.category = new Category();
      component.refreshCategoryList('');
    });
  }

  refreshCategoryList(transactionTypeID?:string) {
    this.service.getCategoryList(transactionTypeID).subscribe(response => {
      if(response.status == 200) {
        var apiResponse = new APIResponse<Category[]>(response.body);

        this.CategoryList = apiResponse.Content;
      } else {
        alert('Error! Cannot get category list.');
      }
    }, error => {
      alert(error.message);
    });
  }

  addCategory() {
    $('#categoryModal').find('input[type="text"]').val('');
    $('#categoryModal').find('select').val('');

    this.category = new Category({
      CategoryID: '0',
      TransactionTypeID: '',
      CategoryName: ''
    });
    this.ModalTitle = 'Add Category';
    this.activateAddEditCategory = true;
  }

  editCategory(category:Category) {
    this.category = category;
    this.ModalTitle = 'Edit Category';
    this.activateAddEditCategory = true;
  }

  deleteCategory(category:Category) {
    $('#DivOverlay').show();

    if(confirm('Are you sure want to delete?')) {
      this.service.deleteCategory(category.CategoryID).subscribe(response => {
        var apiResponse = new APIResponse<Category>(response);
  
        if(apiResponse.StatusCode == 200) {
          this.category = new Category();
  
          alert("Successful deleted new Category.");

          $('#DivOverlay').hide();

          this.refreshCategoryList('');
        }
      }, error => {
        var apiResponse = new APIResponse<boolean>(error.error);

        if(apiResponse) {
          alert(apiResponse.StatusRemark);
        } else {
          alert(error.message);
        }

        $('#DivOverlay').hide();
      })
    } else {
      $('#DivOverlay').hide();
    }
  }

  closeModal() {
    this.activateAddEditCategory = false;
    this.category = new Category();
  }
}
