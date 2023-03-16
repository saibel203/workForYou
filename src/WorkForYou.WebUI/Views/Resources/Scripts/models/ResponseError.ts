import {IResponseError} from "./interfaces/IResponseError.interface.js";

export class ResponseError implements IResponseError {
    errorCode: number;
    errorMessage: string;
    errorDetails?: string;
    
    constructor(response: IResponseError) {
        this.errorCode = response.errorCode;
        this.errorMessage = response.errorMessage;
        this.errorDetails = response.errorDetails;
    }
}