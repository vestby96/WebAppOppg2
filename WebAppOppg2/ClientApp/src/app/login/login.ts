//Kan fjerne det meste av dette
//Jobbet med å sette opp en fast bruker dere kan finne en del av det i User.cs
//er mer eller mindre ferdig men det men må få kjørt PostController.cs sin LoggInn funksjon her når knappen login trykkes.
//kan bruke mye av koden jeg har i registrer.ts

import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({ templateUrl: 'login.html' })
export class Login implements OnInit {
    form: FormGroup;
    loading = false;
    submitted = false;

    constructor(
        private formBuilder: FormBuilder,
    ) { }

    ngOnInit() {
        this.form = this.formBuilder.group({
            username: ['', Validators.required],
            password: ['', Validators.required]
        });
    }

}