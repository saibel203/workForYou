import {login} from "./services/auth-service.js";

const formButton = document.getElementById('login-form-button') as HTMLButtonElement | null;

formButton.addEventListener('click', () => {
    const emailInput = document.getElementById('user-email') as HTMLInputElement | null;
    const passwordInput = document.getElementById('user-password') as HTMLInputElement | null;

    const path: string = '/login';

    const data = {
        email: emailInput?.value,
        password: passwordInput?.value,
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