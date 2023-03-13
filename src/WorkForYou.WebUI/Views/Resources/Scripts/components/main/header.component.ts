const toggleButton = document.querySelector('.header__settings-switcher') as HTMLElement | null;
const dropdownMenuButton = document.querySelector('.header__settings-button') as HTMLElement | null;
const dropdownMenu = document.querySelector('.profile-dropdown') as HTMLElement | null;

if (localStorage.getItem('themeColor') ===   null) 
    localStorage.setItem('themeColor', 'light');

if (localStorage.getItem('themeColor') === 'light') 
    document.body.classList.remove('dark-mode');
else 
    document.body.classList.add('dark-mode');

toggleButton.addEventListener('click', () => {
    if (!document.body.classList.contains('dark-mode')) {
        document.body.classList.add('dark-mode');
        localStorage.setItem('themeColor', 'dark');
    } else {
        document.body.classList.remove('dark-mode');
        localStorage.setItem('themeColor', 'light');
    }
});

dropdownMenuButton.addEventListener('click', () => {
    dropdownMenu.classList.toggle('show');
});