const passwordField = document.querySelector('.contact-form__input input[type="password"]') as HTMLInputElement | null;
const showHideIcon = document.querySelector('.contact-form__input span i') as HTMLElement;

showHideIcon.addEventListener('click', () => {
    if (passwordField.type === 'password') {
        passwordField.setAttribute('type', 'text');
        showHideIcon.setAttribute('class', 'fa-solid fa-eye-slash');
    } else {
        passwordField.setAttribute('type', 'password');
        showHideIcon.setAttribute('class', 'fa-solid fa-eye');
    }
});