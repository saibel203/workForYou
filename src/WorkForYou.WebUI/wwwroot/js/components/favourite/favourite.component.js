import { addToFavouriteList } from "../../services/favourite.service.js";
const likeForms = document.querySelectorAll('.form-like');
const likeButtons = document.querySelectorAll('.form-like-button');
const likePath = document.querySelectorAll('.like-heart');
const currentPath = window.location.origin + window.location.pathname;
const currentPathElements = currentPath.split('/');
const accountRoleName = 'CandidateAccount';
const accountFavourite = 'FavouriteCandidateList';
const accountRespondedList = 'RespondedList';
const accountAllEmployer = 'AllEmployerVacancies';
const path = currentPathElements.indexOf(accountRoleName) > -1 || currentPathElements.indexOf(accountFavourite) > -1
    || currentPathElements.indexOf(accountRespondedList) > -1
    || currentPathElements.indexOf(accountAllEmployer) > -1
    ? '/newVacancy'
    : '/newCandidate';
for (let i = 0; i < likeForms.length; i++) {
    likeButtons[i].addEventListener('click', () => {
        var _a;
        likePath[i].classList.toggle('is-active');
        const formIdValue = (_a = likeForms[i]['id']) === null || _a === void 0 ? void 0 : _a.value;
        const data = {
            id: formIdValue
        };
        addToFavouriteList(path, data)
            .then()
            .catch(error => {
            console.log(error);
        });
    });
}
