import { respondToVacancy } from "./services/responded-service.js";
const respondVacancyButton = document.getElementById('respond-vacancy-button');
const removeRespondVacancyButton = document.getElementById('remove-respond-vacancy-button');
const respondVacancyId = document.getElementById('respond-vacancy-id');
respondVacancyButton.addEventListener('click', respond);
removeRespondVacancyButton.addEventListener('click', respond);
function respond() {
    const data = +(respondVacancyId === null || respondVacancyId === void 0 ? void 0 : respondVacancyId.value);
    const path = window.getComputedStyle(respondVacancyButton).display === 'block'
        ? '/newResponded'
        : '/removeResponded';
    respondToVacancy(path, data)
        .then()
        .catch(error => {
        console.log(error);
    });
    if (path === '/newResponded') {
        respondVacancyButton.style.display = 'none';
        removeRespondVacancyButton.style.display = 'block';
    }
    else {
        respondVacancyButton.style.display = 'block';
        removeRespondVacancyButton.style.display = 'none';
    }
}
//# sourceMappingURL=respond-vacancy.js.map