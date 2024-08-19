import {
  HTTP_INTERCEPTORS,
  provideHttpClient,
  withInterceptors,
  withInterceptorsFromDi,
} from '@angular/common/http';
import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { provideRouter } from '@angular/router';
import { provideToastr } from 'ngx-toastr';
import { jwtInterceptor } from './_interceptors/jwt.interceptor';
import { routes } from './app.routes';
import { errorInterceptor } from './_interceptors/error.interceptor';
import { loadingInterceptor } from './_interceptors/loading.interceptor';
import { NgxSpinnerModule } from 'ngx-spinner';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';


export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideToastr({
      positionClass: 'toast-bottom-right',
    }),
    importProvidersFrom(
      BrowserModule,
      FormsModule,
      NgxSpinnerModule,
      BrowserAnimationsModule
    ),
    provideHttpClient(
      withInterceptors([errorInterceptor, jwtInterceptor, loadingInterceptor]),
      withInterceptorsFromDi()
    ),
  ],
};
