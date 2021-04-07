export class SearchTransaction {
    TransactionTypeID: string;
    StartDate: Date;
    EndDate: Date;
    Year: number;
    Month: number;
    FromAccountID: string;
    CategoryID: string;
    Min: number;
    Max: number;
    
    constructor(object?:any) {
        if(object) {
            Object.assign(this, object)
        }
    }
}  