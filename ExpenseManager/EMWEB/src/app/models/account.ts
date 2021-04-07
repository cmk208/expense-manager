export class Account {
    AccountID: string;
    AccountTypeID: string;
    AccountTypeName: string;
    AccountName: string;
    CreatedDate: Date;

    constructor(object?:any) {
        if(object) {
            Object.assign(this, object)
        }
    }
}  