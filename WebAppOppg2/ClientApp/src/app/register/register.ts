import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { User } from "../User";

@Component({ templateUrl: 'register.html' })
export class Register {
    form: FormGroup;

    skjema: FormGroup;

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
        this.skjema = fb.group(this.validering);
    }

    onSubmit() {
        this.saveUser();
    }

    saveUser() {
        const saveUser = new User();

        saveUser.firstName = this.skjema.value.firstName;
        saveUser.lastName = this.skjema.value.lastName;
        saveUser.username = this.skjema.value.username;
        saveUser.password = this.skjema.value.password;

        this.http.post("api/user/", saveUser)
            .subscribe(retur => {
                this.router.navigate(['/login']);
            },
                error => console.log(error)
        );
    };
}

