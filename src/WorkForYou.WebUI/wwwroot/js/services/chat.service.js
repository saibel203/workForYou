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
import { ResponseError } from "../models/ResponseError.js";
export function removeChat(path, roomId) {
    return __awaiter(this, void 0, void 0, function* () {
        const fullPath = environments.webAPIProject + '/api/chat' + path + '/' + roomId;
        const token = localStorage.getItem('token');
        const method = 'DELETE';
        const headers = new Headers();
        headers.append('Accept', 'application/json');
        headers.append('Content-Type', 'application/json');
        headers.append('Access-Control-Allow-Origin', environments.webAPIProject);
        headers.append('Authorization', 'Bearer ' + token);
        const response = yield fetch(fullPath, {
            method: method,
            headers: headers
        });
        const responseResult = yield response.json();
        if (response.ok)
            return responseResult;
        throw new ResponseError(responseResult);
    });
}
