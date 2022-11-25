import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { SPA } from './spa';
import { Login } from './login/login';
import { Register } from './register/register';
import { AppRoutingModule } from './app-routing.module';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';

@NgModule({
    imports: [
        ReactiveFormsModule,
        HttpClientModule,
        AppRoutingModule,
        CommonModule,
        BrowserModule.withServerTransition({appId: 'ng-cli-universal'})
    ],
    declarations: [
        AppComponent,
        SPA,
        Login,
        Register,

    ],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule { }