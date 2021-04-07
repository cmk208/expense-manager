import { Time } from "@angular/common";

export class Transaction {
    TransactionID: string;
    FromTransactionID: string;
    TransactionTypeID: string;
    TransactionTypeName: string;
    FromAccountID: string;
    FromAccountName: string;
    ToAccountID: string;
    ToAccountName: string;
    CategoryID: string;
    CategoryName: string;
    SubCategoryID: string;
    SubCategoryName: string;
    TransactionDate: Date;
    TransactionTime: Time;
    Operator: boolean;
    Amount: number;
    Note: string;
    CreatedDate: Date;
    TransactionIsEdit: boolean;

    constructor(object?:any) {
        if(object) {
            Object.assign(this, object)
        }
    }
}  