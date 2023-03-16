import {IResponseMessage} from "../models/interfaces/IResponseMessage.interface.js";
import {IResponseError} from "../models/interfaces/IResponseError.interface.js";
import {environments} from "../constants/environments.js";
import {ResponseError} from "../models/ResponseError.js";

export async function addToFavouriteList(path: string, data: object): Promise<IResponseMessage | IResponseError> {
    const fullPath: string = environments.webAPIProject + '/api/favourite' + path;
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

    const responseResult = await response.json();

    if (response.ok)
        return responseResult;

    throw new ResponseError(responseResult);
}