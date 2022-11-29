import { Component, OnInit } from "@angular/core";
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { Token } from "../Token";

@Component({ templateUrl: 'login.html' })
export class Login {
    form: FormGroup;

    skjema: FormGroup;

    validering = {
        username: [
            null, Validators.compose([Validators.required, Validators.pattern("[0-9a-zA-ZøæåØÆÅ\\-. ]{2,30}")])
        ],
        password: [
            null, Validators.compose([Validators.required, Validators.pattern("[0-9a-zA-ZøæåØÆÅ\\-. ]{6,30}")])
        ]
    }

    constructor(private http: HttpClient, private fb: FormBuilder, private router: Router) {
        this.skjema = fb.group(this.validering);
        this.skjema.patchValue({ username: localStorage.getItem('Username') });
        sessionStorage.clear();
    }

    onSubmit() {
        localStorage.setItem('Username', this.skjema.value.username)
        this.Token();
    }

    Token() {
        let queryParams = new HttpParams();
        queryParams = queryParams.append("username", this.skjema.value.username);
        queryParams = queryParams.append("password", this.skjema.value.password);

        this.http.get<Token>("api/user/", { params: queryParams })
            .subscribe(retur => {
                console.log(retur)
                sessionStorage.setItem('Token', retur.token)
                this.router.navigate(['/spa']);
            },
                error =>
                {
                    console.log(error)
                    this.skjema.patchValue({ password: '' });
                }
            );
    };

}