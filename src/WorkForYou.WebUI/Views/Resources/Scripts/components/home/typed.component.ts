// @ts-ignore 
const designers = typedTranslations.designers;

// @ts-ignore
const devOps = typedTranslations.designers;

// @ts-ignore
const testers = typedTranslations.designers;

// @ts-ignore
const pm = typedTranslations.designers;

// @ts-ignore
const all = typedTranslations.designers;

const typedStrings = [
    designers, devOps, testers, pm, all
];

const speed = 100;

// @ts-ignore
let typed = new Typed(".auto-type", {
    strings: typedStrings,
    typeSpeed: speed,
    backSpeed: speed - 30,
    loop: true
});