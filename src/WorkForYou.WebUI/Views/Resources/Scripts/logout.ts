const logoutButton = document.getElementById('header-logout-button') as HTMLLinkElement | null;

logoutButton.addEventListener('click', () => {
    localStorage.clear();
});