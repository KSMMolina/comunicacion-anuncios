import { inject, Injectable } from '@angular/core';
import { PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { Router } from '@angular/router';
import { UserLocalService } from '../services/user.service';
import { isTokenExpired } from '../services/helper.service';
import { jwtDecode } from 'jwt-decode';

@Injectable({ providedIn: 'root' })
export class AuthGuard {
  private userLocalService = inject(UserLocalService);
  private router = inject(Router);
  private platformId = inject(PLATFORM_ID);

  canActivate(): boolean {
    // Evitar acceso a window/localStorage en SSR
    const isBrowser = isPlatformBrowser(this.platformId);
    if (!isBrowser) {
      // En SSR no sabemos el estado de localStorage; asumimos no autenticado.
      return false;
    }

    const token = localStorage.getItem('access_token');

    if (!token) {
      this.router.navigate(['/login']);
      return false;
    }

    try {
      const decoded: any = jwtDecode(token);
      if (isTokenExpired(decoded)) {
        localStorage.removeItem('access_token');
        this.router.navigate(['/login']);
        return false;
      }
      // Guardar usuario si aún no está
      if (!this.userLocalService.user()) this.userLocalService.user.set(decoded);
      return true;
    } catch (e) {
      localStorage.removeItem('access_token');
      this.router.navigate(['/login']);
      return false;
    }
  }
}
