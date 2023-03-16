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
export function confirmMessage(questionMessage) {
    // @ts-ignore
    return Swal.fire({
        title: 'Are you sure?',
        text: questionMessage,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete chat!'
    }).then((result) => {
        if (!result.isConfirmed) {
            return null;
        }
    });
}
