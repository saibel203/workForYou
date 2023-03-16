// @ts-ignore
const connection = new signalR.HubConnectionBuilder()
    .withUrl('/hubs/onlineUsersHub')
    .build();

const oppositeUsernames = document.querySelectorAll('.opposite-username') as NodeListOf<HTMLInputElement> | null;
const isUserOnlineIcon = document.querySelectorAll('.chat-wrapper__chats-chat-main span') as NodeListOf<HTMLElement> | null;

connection.start()
    .then()
    .catch(error => {
    console.error(error);
    });
connection.on('updateOnlineUsers', (users) => {
    users.forEach((user) => {
        for (let i = 0; i < oppositeUsernames.length; i++) {
            if (user === oppositeUsernames[i].value)
                isUserOnlineIcon[i].classList.add('user-online');
        }
    });
});