import {ILocalizationResponse} from "../models/interfaces/ILocalizationResponse.interface.js";

export function successMessage(message: ILocalizationResponse | string): void {
    // @ts-ignore
    Swal.fire(
        'Successfully!',
        message,
        'success'
    );
}

export function errorMessage(message: string): void {
    // @ts-ignore
    Swal.fire({
        icon: 'error',
        title: 'Oops...',
        text: message,
        footer: '<a href="">Why do I have this issue?</a>'
    })
}