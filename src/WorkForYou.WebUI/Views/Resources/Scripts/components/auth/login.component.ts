import {login} from "../../services/auth.service.js";

const loginForm = document.getElementById('login-form') as HTMLFormElement | null;
const loginFormButton = document.getElementById('login-form-button') as HTMLButtonElement | null;

loginFormButton.addEventListener('click', e => {
    e.preventDefault();

    const emailInput = document.getElementById('user-email') as HTMLInputElement | null;
    const passwordInput = document.getElementById('user-password') as HTMLInputElement | null;
    
    const path: string = '/login';
    
    const data = {
        email: emailInput?.value,
        password: passwordInput?.value
    };
    
    login(path, data)
        .then(result => {
            localStorage.setItem('token', result?.token);
            loginForm.submit();
        })
        .catch(error => {
            console.error('Error login ' + error);
        });
});