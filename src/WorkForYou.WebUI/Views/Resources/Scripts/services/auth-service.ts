import {environment} from "../environments/environment.js";

const baseUrl: string = environment.webApiApplicationUrl + '/api/auth';
export async function login(path: string, data: object) {
    const fullPath: string = baseUrl + path;
    const method: string = 'POST';
    const cors: RequestMode = 'no-cors';
    
    const headers: HeadersInit = new Headers();
    headers.append('Content-Type', 'application/json');
    headers.append('Access-Control-Allow-Origin', '*');
    headers.append('Access-Control-Allow-Methods', '*');
    headers.append('Access-Control-Allow-Headers', '*');
    
    const body: BodyInit = JSON.stringify(data);
    
    let response = await fetch(fullPath, {
        method: method,
        mode: cors,
        headers: headers,
        body: body
    });
    
    if (response.ok)
        return response.json();
    else 
        return response.text()
            .then(error => new Error(error));
}