import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ReportComponent } from './report/report.component';
import { TransactionComponent } from './transaction/transaction.component';
import { AccountComponent } from './account/account.component';
import { CategoryComponent } from './category/category.component';

const routes: Routes = [
  { path: '', component:ReportComponent },
  { path: 'transaction', component:TransactionComponent },
  { path: 'account', component:AccountComponent },
  { path: 'category', component:CategoryComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
