const likeButton: HTMLElement = document.querySelector('.like-heart');
likeButton.addEventListener('click', () => {
    likeButton.classList.toggle('is-active');
});