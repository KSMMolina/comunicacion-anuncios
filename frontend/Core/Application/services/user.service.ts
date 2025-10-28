import { computed, inject, Injectable, signal } from '@angular/core';
import { IUser } from '../../domain/user';

@Injectable({
  providedIn: "root",
})
export class UserLocalService {
    public user = signal<IUser.Token | null>(null);

    public isAdmin = computed(
        () => this.user()?.role === 'admin'
    );
}