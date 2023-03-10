var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
import { environment } from "../environments/environment.js";
const baseUrl = environment.webApiApplicationUrl + '/api/responded';
export function respondToVacancy(path, data) {
    return __awaiter(this, void 0, void 0, function* () {
        const token = localStorage.getItem('token');
        const fullPath = baseUrl + path;
        const method = 'POST';
        const cors = 'cors';
        const bodyData = { 'id': data };
        const headers = new Headers();
        headers.append('Content-Type', 'application/json');
        headers.append('Access-Control-Allow-Origin', '*');
        headers.append('Authorization', 'Bearer ' + token);
        const body = JSON.stringify(bodyData);
        console.log(body);
        console.log(fullPath);
        const response = yield fetch(fullPath, {
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
    });
}
//# sourceMappingURL=responded-service.js.map