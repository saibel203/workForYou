const tagsContainerEl = document.querySelector('.create-form__item-keywords');
const tagsInput = document.querySelector('.create-form__item-keywords input');
const createForm = document.querySelector('.create-form form');
const keywordsInput = document.querySelector('.keywords-input-item');


let tags = [];

createForm.addEventListener('keypress', e => {
    let key = e.charCode || e.keyCode || 0;
    if (key === 13)
        e.preventDefault();
});

let deleteTag = (e) => {
    let parentNode = e.target.parentNode;
    let parentNodeText = parentNode.innerText;
    let tagIndex = tags.indexOf(parentNodeText);
    tags.splice(tagIndex, 1);
    parentNode.remove();
};

let pushTag = (title) => {
    let li = document.createElement('li');
    let icon = document.createElement('i');
    li.className = 'tag';
    icon.addEventListener('click', deleteTag);
    icon.className = 'fa-solid fa-xmark';
    li.innerText = title;
    li.appendChild(icon);
    tagsContainerEl.insertBefore(li, tagsInput);
};

let addTagsInitialization = () => {
    tags.forEach(tag => {
        pushTag(tag);
    });
};

addTagsInitialization();

const addTags = (e) => {
    if (e.key === 'Enter') {
        let title = e.target.value.replace(/\s+/g, " ");

        if (!title) return;

        if (!tags.includes(title)) {
            pushTag(title);
            tags.push(title);
            tagsInput.value = '';
            if (keywordsInput.value.length === 0)
                keywordsInput.value += title;
            else
                keywordsInput.value += ', ' + title;
        } else {
            e.target.value = "";
        }
    }
};

tagsInput.addEventListener('keyup', addTags);

window.addEventListener('beforeunload', () => {
    localStorage.setItem('VacancyKeywords', tags.join(', '));
});

window.addEventListener('load', () => {
    if (localStorage.getItem('VacancyKeywords').length !== 0) {
        const newTags = localStorage.getItem('VacancyKeywords').split(', ');

        keywordsInput.value = localStorage.getItem('VacancyKeywords');

        for (let tag of newTags)
            tags.push(tag);

        addTagsInitialization();
        
        localStorage.removeItem('VacancyKeywords');
        tags = [];
    }
});