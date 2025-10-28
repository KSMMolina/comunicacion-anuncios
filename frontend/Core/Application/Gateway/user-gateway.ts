import { Observable } from "rxjs";
import { IGeneric } from "../Models/generic.interface";
import { IUser } from "../../domain/user";

export abstract class UserGateway {
    
    abstract login(body: IUser.LoginRequest): Observable<IGeneric.Response<string>>;
    
}