const imageForm = document.getElementById('image-form');
const imageFormFile = document.getElementById('image-form-file');
const imageFormButton = document.getElementById('image-form-send');
imageFormButton.addEventListener('click', () => {
    imageFormFile.click();
});
imageFormFile.addEventListener('change', () => {
    imageForm.submit();
});
