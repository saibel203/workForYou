import {environment} from "../environments/environment.js";

const baseUrl: string = environment.webApiApplicationUrl + '/api/responded';

export async function respondToVacancy(path: string, data: number) {
    const token: string = localStorage.getItem('token');
    const fullPath: string = baseUrl + path;
    const method: string = 'POST';
    const cors: RequestMode = 'cors';
    const bodyData = {'id': data};

    const headers: HeadersInit = new Headers();
    headers.append('Content-Type', 'application/json');
    headers.append('Access-Control-Allow-Origin', '*');
    headers.append('Authorization', 'Bearer ' + token);

    const body: BodyInit = JSON.stringify(bodyData);

    console.log(body);
    console.log(fullPath);

    const response = await fetch(fullPath, {
        method: method,
        mode: cors,
        headers: headers,
        body: body
    });

    if (response.ok)
        return response.text();
    else
        return response.text()
            .then(error => new Error(error));
}