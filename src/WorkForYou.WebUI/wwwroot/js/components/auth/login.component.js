import { login } from "../../services/auth.service.js";
const loginForm = document.getElementById('login-form');
const loginFormButton = document.getElementById('login-form-button');
loginFormButton.addEventListener('click', e => {
    e.preventDefault();
    const emailInput = document.getElementById('user-email');
    const passwordInput = document.getElementById('user-password');
    const path = '/login';
    const data = {
        email: emailInput === null || emailInput === void 0 ? void 0 : emailInput.value,
        password: passwordInput === null || passwordInput === void 0 ? void 0 : passwordInput.value
    };
    login(path, data)
        .then(result => {
        localStorage.setItem('token', result === null || result === void 0 ? void 0 : result.token);
        loginForm.submit();
    })
        .catch(error => {
        console.error('Error login ' + error);
    });
});
