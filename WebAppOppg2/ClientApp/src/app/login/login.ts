import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { User } from "../User";

@Component({ templateUrl: 'login.html' })
export class Login implements OnInit {
    schemaLogin: FormGroup;
    loading = false;
    submitted = false;

    constructor(private fb: FormBuilder, private _http: HttpClient) {

    }

    ngOnInit() {
        this.schemaLogin = this.fb.group({
            username: ['', Validators.required],
            password: ['', Validators.required]
        });
    }

    onSubmit() {
        this.loggInn();
    }

    loggInn() {
        const username = document.getElementById("username") as HTMLInputElement;
        const brukernavnOK = this.validerBrukernavn(username.value);
        const password = document.getElementById("password") as HTMLInputElement;
        const passordOK = this.validerPassord(password.value);

        if (brukernavnOK && passordOK) {
            const bruker: User = {
                id: null,
                firstName: null,
                lastName: null,
                username: username.value,
                password: password.value
            };
            console.log(bruker);

            var loginOK: boolean = false;

            this._http.get("api/user" + bruker)
                .subscribe(retur => {
                    loginOK = true;
                    console.log("Login: "+loginOK);
                },
                    error => console.log(error),
                    () => console.log("done get-api/user" + bruker)
            );

            
        }
    }

    validerBrukernavn(brukernavn) {
        const regexp = /^[a-zA-ZæøåÆØÅ\.\ \-]{2,20}$/;
        const ok = regexp.test(brukernavn);
        if (!ok) {
            console.log("brukernavn ikke validert");
            return false;
        }
        else {
            console.log("brukernavn validert");
            return true;
        }
    }

    validerPassord(passord) {
        const regexp = /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$/;
        const ok = regexp.test(passord);
        if (!ok) {
            console.log("passord ikke validert");
            return false;
        }
        else {
            console.log("passord validert");
            return true;
        }
    }
}