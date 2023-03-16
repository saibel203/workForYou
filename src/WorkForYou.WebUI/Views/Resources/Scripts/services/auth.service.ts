import {ILoginResponse} from "../models/interfaces/ILoginResponse.interface.js";
import {environments} from "../constants/environments.js";

export async function login(path: string, data: object): Promise<ILoginResponse> {
    const fullPath: string = environments.webAPIProject + '/api/auth' + path;
    const body: BodyInit = JSON.stringify(data);
    const headers: HeadersInit = new Headers();
    const method: string = 'POST';

    headers.append('Accept', 'application/json');
    headers.append('Content-Type', 'application/json');
    headers.append('Access-Control-Allow-Origin', environments.webAPIProject);
    
    const response = await fetch(fullPath, {
        method: method,
        headers: headers,
        body: body
    });
    
    return response.json();
}