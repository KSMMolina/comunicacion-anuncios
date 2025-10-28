import { environment } from './../../env/Environment';
import { inject, Injectable } from "@angular/core";
import { IUser } from "../../domain/user";
import { Observable } from "rxjs";
import { IGeneric } from "../../Application/Models/generic.interface";
import { UserGateway } from "../../Application/Gateway/user-gateway";
import { HttpService } from '../../Application/services/http.service';

@Injectable()
export class UserGatewayImplService implements UserGateway {
    private httpService = inject(HttpService);

    login(body: IUser.LoginRequest): Observable<IGeneric.Response<string>> {
        return this.httpService.post<IGeneric.Response<string>>(`${environment.apiUrl}/login`, body);
    }
}