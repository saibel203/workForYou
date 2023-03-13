// Hamburger menu settings
const hamburgerMenu = document.getElementById('hamburger-menu');
const navbarItems = document.getElementsByClassName('home-navbar__navigation')[0];
hamburgerMenu.addEventListener('click', () => {
    hamburgerMenu.classList.toggle('is-active');
    navbarItems.classList.toggle('show');
});
// Back to top
const amountScrolled = 150;
const btnToTop = document.getElementsByClassName('back-to-top')[0];
window.addEventListener('scroll', () => {
    let distance = window.scrollY;
    if (distance > amountScrolled)
        btnToTop.classList.add('show');
    else
        btnToTop.classList.remove('show');
});
btnToTop.addEventListener('click', () => {
    window.scrollTo({ top: 0, behavior: 'smooth' });
});
