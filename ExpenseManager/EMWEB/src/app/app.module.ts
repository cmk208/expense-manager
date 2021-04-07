import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CategoryComponent } from './category/category.component';
import { AccountComponent } from './account/account.component';
import { SharedService } from './shared.service'

import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CategoryShowComponent } from './category/category-show/category-show.component';
import { CategoryAddEditComponent } from './category/category-add-edit/category-add-edit.component';
import { AccountShowComponent } from './account/account-show/account-show.component';
import { AccountAddEditComponent } from './account/account-add-edit/account-add-edit.component';
import { TransactionComponent } from './transaction/transaction.component';
import { TransactionShowComponent } from './transaction/transaction-show/transaction-show.component';
import { TransactionAddEditComponent } from './transaction/transaction-add-edit/transaction-add-edit.component';
import { ChartsModule, ThemeService } from 'ng2-charts';
import { DatePipe } from '@angular/common';
import { ReportComponent } from './report/report.component';
import { ReportShowComponent } from './report/report-show/report-show.component';
import { ReportDetailShowComponent } from './report/report-detail-show/report-detail-show.component';

@NgModule({
  declarations: [
    AppComponent,
    CategoryComponent,
    AccountComponent,
    CategoryShowComponent,
    CategoryAddEditComponent,
    AccountShowComponent,
    AccountAddEditComponent,
    TransactionComponent,
    TransactionShowComponent,
    TransactionAddEditComponent,
    ReportComponent,
    ReportShowComponent,
    ReportDetailShowComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    ChartsModule
  ],
  providers: [SharedService, DatePipe, ThemeService],
  bootstrap: [AppComponent]
})
export class AppModule { }
