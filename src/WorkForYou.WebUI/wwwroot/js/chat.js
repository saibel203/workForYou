'use strict';

const connection = new signalR.HubConnectionBuilder()
    .withUrl('/hubs/chatHub')
    .build();

const sendButton = document.getElementById('sendButton');
const messagesList = document.getElementById('messagesList');
const messageInput = document.getElementById('messageInput').value;
const senderInput = document.getElementById('senderInput').value;
const recipientInput = document.getElementById('recipientInput').value;

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on('ReceiveMessage', (user, message) => {
    const messageWrapper = document.createElement('div');
    messageWrapper.setAttribute('class', 'chat-details-wrapper__body-message message-not-my');
    
    const messageInner = document.createElement('p');
    messageInner.textContent = `${user}: ${message}`;
    
    messageWrapper.appendChild(messageInner);
    messagesList.appendChild(messageWrapper);
});

sendButton.addEventListener('click', e => {
    connection.invoke('SendMessageToGroup', senderInput, recipientInput, messageInput)
        .then(message => {
            console.log(message);
        }).catch(error => {
            return console.error(error.toString());
    });
    
    e.preventDefault();
});
