import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideZoneChangeDetection } from '@angular/core';
import { bootstrapApplication } from '@angular/platform-browser';
import { provideRouter } from '@angular/router';
import { AppComponent } from './app/app.component';
import { APP_ROUTES } from './app/app.routes';
import { authTokenInterceptor } from './app/core/auth/auth-token.interceptor';
import { errorHandlingInterceptor } from './app/core/http/error-handling.interceptor';

bootstrapApplication(AppComponent, {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(APP_ROUTES),
    provideHttpClient(withInterceptors([authTokenInterceptor, errorHandlingInterceptor])),
  ],
}).catch(err => console.error(err));
