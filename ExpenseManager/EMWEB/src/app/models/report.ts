import { Transaction } from "./Transaction";

export class Report {
    CategoryID: string;
    CategoryName: string;
    Percentage: number;
    Amount: number;
    Transactions: Transaction[];

    constructor(object?:any) {
        if(object) {
            Object.assign(this, object)
        }
    }
}  