﻿import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Post } from "./Post";

@Component({
    selector: "app-root",
    templateUrl: "SPA.html"
})

export class SPA implements OnInit {
                                    // variabler
    showDetails: boolean;           // true når detaljer om en post skal vises
    showSchemaRegister: boolean;    // true når input-skjema skal vises
    showTable: boolean;             // true når tabellen skal vises
    loading: boolean;               // true når siden laster
    toggleSort: boolean = true;     // toggle mellom sorterings retninger
    allPosts: Array<Post>;          // array med alle poster
    schema: FormGroup;              // skjema for utfylling av post
    singlePost: Post;               // brukes for å hente ut en post

    validation = { // inputvalidering av post
        id: [""],
        datePosted: [""],
        dateOccured: [""],
        country: [
            null, Validators.compose([Validators.required, Validators.pattern("[a-zA-ZøæåØÆÅ\\-. ]{2,30}")])
        ],
        city: [
            null, Validators.compose([Validators.required, Validators.pattern("[a-zA-ZøæåØÆÅ\\-. ]{2,30}")])
        ],
        address: [
            null, Validators.compose([Validators.required, Validators.pattern("[0-9a-zA-ZøæåØÆÅ\\-. ]{2,30}")])
        ],
        shape: [
            null, Validators.compose([Validators.required, Validators.pattern("[a-zA-ZøæåØÆÅ\\-. ]{2,30}")])
        ],
        summary: [
            null, Validators.compose([Validators.required, Validators.pattern("[0-9a-zA-ZøæåØÆÅ\\-. ]{2,300}")])
        ]
    }
    
    constructor(private _http: HttpClient, private fb: FormBuilder) { // konstruktør, oppretter HttpClient og Formbuilder
        this.schema = fb.group(this.validation);
    }

    ngOnInit() { // 
        this.loading = true;
        this.getAllPosts();
        this.showTable = true;
        this.showDetails = false;
        this.showSchemaRegister = false;
    }

    getAllPosts() {
        this._http.get<Post[]>("api/post/")
            .subscribe(posts => {
                this.allPosts = posts;
                this.loading = false;
            },
                error => console.log(error),
                () => console.log("done get-api/post")
        );
    }

    onSubmit() {
        if (this.showSchemaRegister) {
            this.savePost();
        }
        else {
            this.editAPost();
        }
    }

    registerPost() {
        // må resette verdiene i skjema dersom skjema har blitt brukt til endringer
        this.schema.setValue({
            id: "",
            datePosted: "",
            dateOccured: "",
            country: "",
            city: "",
            address: "",
            shape: "",
            summary: ""
        });
        this.schema.markAsPristine();
        this.showTable = false;
        this.showDetails = false;
        this.showSchemaRegister = true;
    }

    backToList() {
        this.showTable = true;
        this.showDetails = false;
        this.showSchemaRegister = false;
    }

    savePost() {
        const savedPost = new Post();

        var date = new Date();
        var str: string;
        str = date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate();
        
        savedPost.datePosted = str;
        savedPost.dateOccured = this.schema.value.dateOccured;
        savedPost.country = this.schema.value.country;
        savedPost.city = this.schema.value.city;
        savedPost.address = this.schema.value.address;
        savedPost.shape = this.schema.value.shape;
        savedPost.summary = this.schema.value.summary;

        this._http.post("api/post", savedPost)
            .subscribe(retur => {
                this.getAllPosts();
                this.showTable = true;
                this.showDetails = false;
                this.showSchemaRegister = false;
            },
                error => console.log(error)
            );
    };

    deletePost(id: number) {
        this._http.delete("api/post/" + id)
            .subscribe(retur => {
                this.getAllPosts();
            },
                error => console.log(error)
            );
    };

    editPost(id: number) {
        this._http.get<Post>("api/post/" + id)
            .subscribe(
                post => {
                    this.schema.patchValue({ id: post.id });
                    this.schema.patchValue({ datePosted: post.datePosted });
                    this.schema.patchValue({ dateOccured: post.dateOccured });
                    this.schema.patchValue({ country: post.country });
                    this.schema.patchValue({ city: post.city });
                    this.schema.patchValue({ address: post.address });
                    this.schema.patchValue({ shape: post.shape });
                    this.schema.patchValue({ summary: post.summary });
                },
                error => console.log(error)
        );

        this.showTable = false;
        this.showDetails = false;
        this.showSchemaRegister = true;
        console.log(id);
    }

    editAPost() {
        const editPost = new Post();
        editPost.id = this.schema.value.id;
        editPost.datePosted = this.schema.value.datePosted;
        editPost.dateOccured = this.schema.value.dateOccured;
        editPost.country = this.schema.value.country;
        editPost.city = this.schema.value.city;
        editPost.address = this.schema.value.address;
        editPost.shape = this.schema.value.shape;
        editPost.summary = this.schema.value.summary;

        this._http.put("api/post/", editPost)
            .subscribe(
                retur => {
                    this.getAllPosts();
                    this.showTable = true;
                    this.showDetails = false;
                    this.showSchemaRegister = false;
                },
                error => console.log(error)
            );
    }

    postDetails(id: number) {
        this._http.get<Post>("api/post/" + id)
            .subscribe(post => {
                this.singlePost = post;
                this.loading = false;
                this.showTable = false;
                this.showDetails = true;
                this.showSchemaRegister = false;

            },
                error => console.log(error),
                () => console.log("done get-api/post" + id)
            );
    }

    sortByDatePosted() {
        var sortedArray = this.allPosts;
        if (this.toggleSort) {
            sortedArray.sort((a, b) => (a.datePosted < b.datePosted) ? -1 : 1);
            this.toggleSort = false;
        }
        else {
            sortedArray.sort((a, b) => (a.datePosted > b.datePosted) ? -1 : 1);
            this.toggleSort = true;
        }
    }

    sortByDateOccured() {
        let sortedArray;
        if (this.toggleSort) {
            sortedArray = this.allPosts.sort((a, b) => (a.dateOccured < b.dateOccured) ? -1 : 1);
            this.toggleSort = false;
        }
        else {
            sortedArray = this.allPosts.sort((a, b) => (a.dateOccured > b.dateOccured) ? -1 : 1);
            this.toggleSort = true;
        }
    }

    searchArray() {
        var filter, value, row, cols, filtered, i, j, table; // vaiabler
        filter = document.getElementById("searchbar") as HTMLInputElement; // henter HTML-input
        value = filter.value.toUpperCase(); // henter verdien fra input

        table = document.getElementById("display") as HTMLTableElement; // henter HTML-tabellen
        row = table.getElementsByTagName("tr") as HTMLTableRowElement; // henter radene fra tabellen

        for (i = 1; i < row.length; i++) { // løkke som går gjennom alle radene i tabelen
            filtered = false; // variabel som brukes til å markere om en rad skal vises eller ikke
            cols = row[i].getElementsByTagName("td"); // henter alle kolonnene i en spesifikk rad
            for (j = 0; j < (cols.length - 3); j++) { // løkke som går gjennom alle kolonnene i raden
                if (cols[j]) { // dersom kolonnen ikke er tom kjører denne
                    if (cols[j].innerHTML.toUpperCase().indexOf(value) > -1) { // sjekker innholdet i cellen om det matcher med søket
                        filtered = true; // dersom det er en match blir filtered satt til true
                    }
                }
            }
            if (filtered === true) { // dersom filtered er lik true blir raden vist
                row[i].style.display = '';
            }
            else { // dersom filtered er lik false blir raden ikke vist
                row[i].style.display = 'none';
            }
        }
    }
}
