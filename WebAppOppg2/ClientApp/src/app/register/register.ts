import { Component} from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { User } from "../User";

//Linker ts filen til login.html
@Component({ templateUrl: 'register.html' })
export class Register {
    form: FormGroup;

    skjema: FormGroup;

    //validerings objekt som setter kravene for valideringen av input feltene
    validering = {
        id: [""],
        firstName: [
            null, Validators.compose([Validators.required, Validators.pattern("[a-zA-ZøæåØÆÅ\\-. ]{2,30}")])
        ],
        lastName: [
            null, Validators.compose([Validators.required, Validators.pattern("[a-zA-ZøæåØÆÅ\\-. ]{2,30}")])
        ],
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
    }

    onSubmit() {
        //Kaller saveUser() funksjonen
        this.saveUser();
    }

    saveUser() {
        //Lagerer input verdiene i const saveUser
        const saveUser = new User();

        saveUser.firstName = this.skjema.value.firstName;
        saveUser.lastName = this.skjema.value.lastName;
        saveUser.username = this.skjema.value.username;
        saveUser.password = this.skjema.value.password;

        //Sender et API POST REQUEST
        this.http.post("api/user/", saveUser)
            .subscribe(retur => {
                //Sender oss til login siden
                this.router.navigate(['/login']);
            },
                error => console.log(error)
        );
    };
}

