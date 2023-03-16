export class ResponseError {
    constructor(response) {
        this.errorCode = response.errorCode;
        this.errorMessage = response.errorMessage;
        this.errorDetails = response.errorDetails;
    }
}
