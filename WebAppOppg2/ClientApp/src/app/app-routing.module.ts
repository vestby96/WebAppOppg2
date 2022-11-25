import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Login } from './login/login';
import { Register } from './register/register';
import { SPA } from './spa';


const appRoots: Routes = [
    { path: 'login', component: Login },
    { path: 'register', component: Register },
    { path: 'spa', component: SPA, },
    { path: '', redirectTo: 'login', pathMatch: 'full' }
]

@NgModule({
    imports: [
        RouterModule.forRoot(appRoots)
    ],
    exports: [
        RouterModule
    ]
})
export class AppRoutingModule { }
