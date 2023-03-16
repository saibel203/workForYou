import { confirmMessage, errorMessage, successMessage } from "../../services/notification.service.js";
import { removeChat } from "../../services/chat.service.js";
const removeChatButtons = document.querySelectorAll('.remove-chat-button');
const chatRoomIdNumber = document.querySelectorAll('.chat-room-id');
for (let i = 0; i < removeChatButtons.length; i++) {
    const path = '/removeChat';
    removeChatButtons[i].addEventListener('click', () => {
        confirmMessage('You sure?')
            .then((result) => {
            if (result !== null) {
                removeChat(path, +chatRoomIdNumber[i].value)
                    .then((response) => {
                    successMessage(response.message.value);
                    setTimeout(() => {
                        location.reload();
                    }, 1500);
                })
                    .catch((error) => {
                    errorMessage(`${error.errorCode} error: ${error.errorMessage}`);
                });
            }
        });
    });
}
