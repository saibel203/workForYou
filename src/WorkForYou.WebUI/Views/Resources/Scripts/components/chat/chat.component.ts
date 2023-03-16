import {confirmMessage, errorMessage, successMessage} from "../../services/notification.service.js";
import {removeChat} from "../../services/chat.service.js";
import {IResponseMessage} from "../../models/interfaces/IResponseMessage.interface.js";
import {IResponseError} from "../../models/interfaces/IResponseError.interface.js";

const removeChatButtons = document.querySelectorAll('.remove-chat-button') as NodeListOf<HTMLElement> | null;
const chatRoomIdNumber = document.querySelectorAll('.chat-room-id') as NodeListOf<HTMLInputElement> | null;

for (let i = 0; i < removeChatButtons.length; i++) {
    const path = '/removeChat';
    removeChatButtons[i].addEventListener('click', () => {
        confirmMessage('You sure?')
            .then((result) => {
                if (result !== null) {
                    removeChat(path, +chatRoomIdNumber[i].value)
                        .then((response: IResponseMessage) => {
                            successMessage(response.message.value);
                            setTimeout(() => {
                                location.reload();
                            }, 1500);
                        })
                        .catch((error: IResponseError) => {
                            errorMessage(`${error.errorCode} error: ${error.errorMessage}`);
                        });   
                }
            });
    });
}