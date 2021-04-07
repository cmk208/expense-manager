export class AccountType {
    AccountTypeID: string;
    AccountTypeName: string;
    
    constructor(object?:any) {
        if(object) {
            Object.assign(this, object)
        }
    }
}  