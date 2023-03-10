'use strict';

const connection = new signalR
    .HubConnectionBuilder()
    .withUrl('/hubs/chatHub')
    .build();

const sendMessageForm = document.getElementById('send-message-form');
const messageInput = document.getElementById('message-input');
const messageIdInput = document.getElementById('message-id-input').value;
const currentUsername = document.getElementById('current-username').value;

const ownClassName = 'chat-details-wrapper__body-message message-own';
const notOwnClassName = 'chat-details-wrapper__body-message message-not-my';

connection.on('ReceiveMessage', data => {
    let containerElement = document.getElementById('message-list');

    let containerInnerElement = document.createElement('div');

    if (currentUsername === data.name)
        containerInnerElement.setAttribute('class', ownClassName);
    else
        containerInnerElement.setAttribute('class', notOwnClassName);

    let containerText = document.createElement('p');
    containerText.appendChild(document.createTextNode(data.content));

    let containerOwner = document.createElement('span');
    containerOwner.appendChild(document.createTextNode(data.name));
    containerText.appendChild(containerOwner);

    let containerSendTime = document.createElement('span');
    containerSendTime.appendChild(document.createTextNode(data.timestamp));

    containerInnerElement.appendChild(containerText);
    containerInnerElement.appendChild(containerSendTime);

    containerElement.appendChild(containerInnerElement);
});

let sendMessage = e => {
    e.preventDefault();

    let data = new FormData(e.target);
    const url = '/Chat/SendMessage';

    messageInput.value = '';

    axios.post(url, data)
        .catch(error => {
            console.error('Error send!', error);
        });
};

connection.start()
    .then(() => {
        connection.invoke('joinToRoom', messageIdInput)
            .then();
    })
    .catch(error => {
        console.error(error);
    });

window.addEventListener('onunload', () => {
    connection.invoke('leaveRoom', messageIdInput)
        .then();
});

window.addEventListener('load', () => {
    window.scrollTo(0, document.body.scrollHeight);
});

sendMessageForm.addEventListener('submit', e => {
    e.preventDefault();
    sendMessage(e);
});
