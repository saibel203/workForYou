const imageForm = document.getElementById('image-form') as HTMLFormElement | null;
const imageFormFile = document.getElementById('image-form-file') as HTMLInputElement | null;
const imageFormButton = document.getElementById('image-form-send') as HTMLInputElement | null;

imageFormButton.addEventListener('click', () => {
    imageFormFile.click();
});

imageFormFile.addEventListener('change', () => {
    imageForm.submit();
});