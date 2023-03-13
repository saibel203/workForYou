const logoutButton = document.getElementById('header-logout-button') as HTMLButtonElement | null;

logoutButton.addEventListener('click', () => {
    localStorage.clear();
});