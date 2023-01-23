// Text type settings
const typedStrings = ['UX/UI дизайнерів.', 'DevOps.', 'тестувальників.', 'програмістів.',
    'PM.', 'вcix, хто працює в IT.'];
const speed = 100;

let typed = new Typed(".auto-type", {
    strings: typedStrings,
    typeSpeed: speed,
    backSpeed: speed - 30,
    loop: true
});