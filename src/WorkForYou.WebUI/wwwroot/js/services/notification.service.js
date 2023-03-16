export function successMessage(message) {
    // @ts-ignore
    Swal.fire('Successfully!', message, 'success');
}
export function errorMessage(message) {
    // @ts-ignore
    Swal.fire({
        icon: 'error',
        title: 'Oops...',
        text: message,
        footer: '<a href="">Why do I have this issue?</a>'
    });
}
