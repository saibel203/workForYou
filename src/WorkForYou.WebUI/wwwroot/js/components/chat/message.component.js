import { sendChatMessage } from "../../services/send-message.service.js";
// @ts-ignore
const connection = new signalR
    .HubConnectionBuilder()
    .withUrl('/hubs/chatHub')
    .build();
const sendMessageForm = document.getElementById('send-message-form');
const messageInput = document.getElementById('message-input');
const messageIdInput = document.getElementById('message-id-input');
const currentUsername = document.getElementById('current-username');
const ownClassName = 'chat-details-wrapper__body-message message-own';
const notOwnClassName = 'chat-details-wrapper__body-message message-not-my';
function sendMessage(e) {
    e.preventDefault();
    const path = '/sendMessage';
    const data = {
        message: messageInput.value,
        roomId: +messageIdInput.value
    };
    messageInput.value = '';
    sendChatMessage(path, data)
        .catch(error => {
        console.error('An error occurred while trying to send the message: ' + error);
    });
}
connection.on('ReceiveMessage', data => {
    const containerElement = document.getElementById('message-list');
    const containerInnerElement = document.createElement('div');
    if (currentUsername.value === data.name)
        containerInnerElement.setAttribute('class', ownClassName);
    else
        containerInnerElement.setAttribute('class', notOwnClassName);
    const containerText = document.createElement('p');
    containerText.appendChild(document.createTextNode(data.content));
    const containerOwner = document.createElement('span');
    containerOwner.appendChild(document.createTextNode(data.name));
    containerText.appendChild(containerOwner);
    const containerSendTime = document.createElement('span');
    containerSendTime.appendChild(document.createTextNode(data.timestamp));
    containerInnerElement.appendChild(containerText);
    containerInnerElement.appendChild(containerSendTime);
    containerElement.appendChild(containerInnerElement);
});
connection.start()
    .then(() => {
    connection.invoke('joinToRoom', messageIdInput.value)
        .then();
})
    .catch(error => {
    console.error(error);
});
window.addEventListener('onunload', () => {
    connection.invoke('leaveRoom', messageIdInput.value)
        .then();
});
window.addEventListener('load', () => {
    window.scrollTo(0, document.body.scrollHeight);
});
sendMessageForm.addEventListener('submit', e => {
    e.preventDefault();
    sendMessage(e);
});
