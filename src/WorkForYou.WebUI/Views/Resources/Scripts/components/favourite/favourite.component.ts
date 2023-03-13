import {addToFavouriteList} from "../../services/favourite.service.js";

const likeForms: any = document.querySelectorAll('.form-like');
const likeButtons = document.querySelectorAll('.form-like-button') as NodeListOf<Element> | null;
const likePath = document.querySelectorAll('.like-heart') as NodeListOf<Element> | null;

const currentPath: string = window.location.origin + window.location.pathname;
const currentPathElements: string[] = currentPath.split('/');
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
        likePath[i].classList.toggle('is-active');

        const formIdValue = likeForms[i]['id']?.value;
        
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
