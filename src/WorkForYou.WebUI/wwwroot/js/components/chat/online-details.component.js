// @ts-ignore
const connection = new signalR.HubConnectionBuilder()
    .withUrl('/hubs/onlineUsersHub')
    .build();
const oppositeUsername = document.querySelector('.chat-details-wrapper__header-info input[type="hidden"]');
const onlineMarker = document.querySelector('.chat-details-wrapper__header-info span');
connection.start()
    .then()
    .catch(error => {
    console.error(error);
});
connection.on('updateOnlineUsers', (users) => {
    users.forEach((user) => {
        if (oppositeUsername.value === user)
            onlineMarker.classList.add('user-online');
    });
});
