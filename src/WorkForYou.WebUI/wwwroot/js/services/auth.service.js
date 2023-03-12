var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
import { environments } from "../constants/environments.js";
export function login(path, data) {
    return __awaiter(this, void 0, void 0, function* () {
        const fullPath = environments.webAPIProject + '/api/auth' + path;
        const body = JSON.stringify(data);
        const headers = new Headers();
        const method = 'POST';
        headers.append('Accept', 'application/json');
        headers.append('Content-Type', 'application/json');
        headers.append('Access-Control-Allow-Origin', environments.webAPIProject);
        const response = yield fetch(fullPath, {
            method: method,
            headers: headers,
            body: body
        });
        return response.json();
    });
}
