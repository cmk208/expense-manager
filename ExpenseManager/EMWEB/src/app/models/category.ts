export class Category {
    CategoryID: string;
    TransactionTypeID: string;
    TransactionTypeName: string;
    CategoryName: string;
    Sequence: number;
    CreatedDate: Date;

    constructor(object?:any) {
        if(object) {
            Object.assign(this, object)
        }
    }
}  