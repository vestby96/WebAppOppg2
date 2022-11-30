import { Component} from "@angular/core";
import { HttpClient, HttpParams } from '@angular/common/http';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { Token } from "../Token";

//Linker ts filen til login.html
@Component({ templateUrl: 'login.html' })
export class Login {
    form: FormGroup;

    skjema: FormGroup;

    //validerings objekt som setter kravene for valideringen av username og password feltene
    validering = {
        username: [
            null, Validators.compose([Validators.required, Validators.pattern("[0-9a-zA-ZøæåØÆÅ\\-. ]{2,30}")])
        ],
        password: [
            null, Validators.compose([Validators.required, Validators.pattern("[0-9a-zA-ZøæåØÆÅ\\-. ]{6,30}")])
        ]
    }

    constructor(private http: HttpClient, private fb: FormBuilder, private router: Router) {
        //Validerer innholdet i input filene mot kravene i validerings objektet
        this.skjema = fb.group(this.validering);
        //Henter username fra local storage
        this.skjema.patchValue({ username: localStorage.getItem('Username') });
        //Tømmer sessionStorage for tokens
        sessionStorage.clear();
    }

    onSubmit() {
        //Lagrer username i local storage
        localStorage.setItem('Username', this.skjema.value.username)
        //Kaller Token() funksjonen
        this.Token();
    }

    Token() {
        //Lagrer username og password som queryParams
        let queryParams = new HttpParams();
        queryParams = queryParams.append("username", this.skjema.value.username);
        queryParams = queryParams.append("password", this.skjema.value.password);

        //Sender et API GET REQUEST
        this.http.get<Token>("api/user/", { params: queryParams })
            .subscribe(retur => {
                console.log(retur)
                //Lagerer token i session storage
                sessionStorage.setItem('Token', retur.token)
                //Sender oss til spa siden
                this.router.navigate(['/spa']);
            },
                error => {
                    console.log(error)
                    //Hvis feil tøm password input feltet
                    this.skjema.patchValue({ password: '' });
                }
            );
    };

}