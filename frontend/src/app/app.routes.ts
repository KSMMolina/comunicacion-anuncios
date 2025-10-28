import { Routes } from '@angular/router';
import { LoginGuard } from '../../Core/Application/guards/login.guard';
import { AuthGuard } from '../../Core/Application/guards/auth.guard';

export const routes: Routes = [
    {
        path: '',
        pathMatch: 'full',
        redirectTo: 'login'
    },
    {
        path: 'login',
        canActivate: [LoginGuard],
        loadComponent: () => import('./modules/login/login').then(c => c.Login),
        title: 'Login'
    },
    {
        path: 'dashboard',
        // canActivate: [AuthGuard],
        loadComponent: () => import('./modules/Dashboard/dashboard.component').then(c => c.DashboardComponent),
        title: 'Dashboard'
    },
    {
        path: '**',
        redirectTo: 'login'
    }
];
