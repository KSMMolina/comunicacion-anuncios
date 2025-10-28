import { inject, Injectable } from '@angular/core';
import { UserGateway } from '../Gateway/user-gateway';
import { IUser } from '../../domain/user';

@Injectable({ providedIn: 'root' })
export class AuthUseCases {
  public gateway = inject(UserGateway);

  public login(credentials: IUser.LoginRequest) {
    return this.gateway.login(credentials);
  }
  
}