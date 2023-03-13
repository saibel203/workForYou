const toggleButton = document.querySelector('.header__settings-switcher');
const dropdownMenuButton = document.querySelector('.header__settings-button');
const dropdownMenu = document.querySelector('.profile-dropdown');
if (localStorage.getItem('themeColor') === null)
    localStorage.setItem('themeColor', 'light');
if (localStorage.getItem('themeColor') === 'light')
    document.body.classList.remove('dark-mode');
else
    document.body.classList.add('dark-mode');
toggleButton.addEventListener('click', () => {
    if (!document.body.classList.contains('dark-mode')) {
        document.body.classList.add('dark-mode');
        localStorage.setItem('themeColor', 'dark');
    }
    else {
        document.body.classList.remove('dark-mode');
        localStorage.setItem('themeColor', 'light');
    }
});
dropdownMenuButton.addEventListener('click', () => {
    dropdownMenu.classList.toggle('show');
});
