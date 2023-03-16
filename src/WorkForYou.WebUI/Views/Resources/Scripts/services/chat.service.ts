import {environments} from "../constants/environments.js";
import {ResponseError} from "../models/ResponseError.js";

export async function removeChat(path: string, roomId: number) {
    const fullPath: string = environments.webAPIProject + '/api/chat' + path + '/' + roomId;
    const token: string = localStorage.getItem('token');
    const method: string = 'DELETE';

    const headers: HeadersInit = new Headers();
    headers.append('Accept', 'application/json');
    headers.append('Content-Type', 'application/json');
    headers.append('Access-Control-Allow-Origin', environments.webAPIProject);
    headers.append('Authorization', 'Bearer ' + token);
    
    const response = await fetch(fullPath, {
        method: method,
        headers: headers
    });

    const responseResult = await response.json();

    if (response.ok)
        return responseResult;

    throw new ResponseError(responseResult);
}