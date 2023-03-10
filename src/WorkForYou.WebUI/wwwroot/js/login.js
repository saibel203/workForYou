import { login } from "./services/auth-service.js";
const formButton = document.getElementById('login-form-button');
formButton.addEventListener('click', () => {
    const emailInput = document.getElementById('user-email');
    const passwordInput = document.getElementById('user-password');
    const path = '/login';
    const data = {
        email: emailInput === null || emailInput === void 0 ? void 0 : emailInput.value,
        password: passwordInput === null || passwordInput === void 0 ? void 0 : passwordInput.value,
    };
    login(path, data)
        .then(result => {
        alert(`${result} SUCCESS branch`);
        localStorage.setItem('token', result['token']);
    })
        .catch(error => {
        alert(`${error}. An error occurred. Try logging out and logging in again.`);
    });
});
//# sourceMappingURL=login.js.map