export class APIResponse<T> {

    StatusCode: number;
    StatusRemark: string;
    Content: T;
    
    constructor(object) {
        Object.assign(this, object)
    }
  }
  