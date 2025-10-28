import { AuthUseCases } from '@core/Application/UseCases/auth';
import { Component, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { UserLocalService } from '@core/Application/services/user.service';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login {
  private fb = inject(FormBuilder);
  loading = signal(false);
  errorMessage = signal<string | null>(null);
  public authUseCases = inject(AuthUseCases);
  private router = inject(Router);
  private userLocalService = inject(UserLocalService);

  form = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
  });

  year = new Date().getFullYear();

  submit() {
    this.errorMessage.set(null);
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.loading.set(true);

    const raw = this.form.getRawValue();
    const email = raw.email as string; // form invalid ya retornó antes
    const password = raw.password as string;
    console.log('[Login] intentando login', { email, password });

    this.authUseCases.login({ email, password }).subscribe({
      next: (response) => {
        this.loading.set(false);
        const token = response?.data;
        localStorage.setItem('access_token', token);
        const decoded: any = jwtDecode(token);
        this.userLocalService.user.set(decoded);
        this.router.navigate(['/dashboard']);
      },
      error: (error) => {
        this.loading.set(false);
        this.errorMessage.set(error?.message || 'Error al iniciar sesión');
      },
    });
  }

  fieldInvalid(field: string): boolean {
    const control = this.form.get(field);
    return !!(control && control.invalid && control.touched);
  }
}
