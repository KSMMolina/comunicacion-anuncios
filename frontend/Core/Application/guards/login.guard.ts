import { Injectable, inject, PLATFORM_ID } from "@angular/core";
import { Router } from "@angular/router";
import { jwtDecode } from "jwt-decode";
import { UserLocalService } from "../services/user.service";
import { isTokenExpired } from "../services/helper.service";
import { isPlatformBrowser } from "@angular/common";

@Injectable({
  providedIn: "root",
})
export class LoginGuard {
  private userLocalService = inject(UserLocalService);
  private router = inject(Router);
  private platformId = inject(PLATFORM_ID);

  canActivate(): boolean {
    const isBrowser = isPlatformBrowser(this.platformId);
    if (!isBrowser) {
      // En SSR permitimos que renderice la página de login sin lógica de redirección.
      return true;
    }

    const token = localStorage.getItem("access_token");
    let isValid = false;

    if (token) {
      try {
        const decoded: any = jwtDecode(token);
        if (!isTokenExpired(decoded)) {
          this.userLocalService.user.set(decoded);
          isValid = true;
        }
      } catch (e) {
        console.warn('Token inválido en login guard', e);
        // token inválido -> limpiar
        localStorage.removeItem("access_token");
      }
    }

    if (isValid) {
      this.router.navigate(["/dashboard"]);
      return false;
    }

    // No token o expirado/ inválido => permitir acceso a login
    return true;
  }
}
