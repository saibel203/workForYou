import {environments} from "../constants/environments.js";

export async function sendChatMessage(path: string, data: object) {
    const fullPath: string = environments.webUIProject + '/chat' + path;
    const token: string = localStorage.getItem('token');
    const method: string = 'POST';

    const headers: HeadersInit = new Headers();
    headers.append('Accept', 'application/json');
    headers.append('Content-Type', 'application/json');
    headers.append('Access-Control-Allow-Origin', environments.webAPIProject);
    headers.append('Authorization', 'Bearer ' + token);

    const body: BodyInit = JSON.stringify(data);

    const response = await fetch(fullPath, {
        method: method,
        headers: headers,
        body: body
    });

    return response.status;
}