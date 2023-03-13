const logoutButton = document.getElementById('header-logout-button');
logoutButton.addEventListener('click', () => {
    localStorage.clear();
});
