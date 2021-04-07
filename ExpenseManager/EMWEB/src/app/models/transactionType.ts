export class TransactionType {
    TransactionTypeID: string;
    TransactionTypeName: string;
    
    constructor(object?:any) {
        if(object) {
            Object.assign(this, object)
        }
    }
}  