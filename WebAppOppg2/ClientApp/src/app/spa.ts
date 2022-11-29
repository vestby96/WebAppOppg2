import { Component, OnInit } from "@angular/core";
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Post } from "./Post";

@Component({
    selector: "app-root",
    templateUrl: "SPA.html"
})
export class SPA implements OnInit{
                                    // variabler
    showDetails: boolean;           // true når detaljer om en post skal vises
    showSchemaRegister: boolean;    // true når input-skjema skal vises
    showSchemaEdit: boolean;
    showTable: boolean;             // true når tabellen skal vises
    loading: boolean;               // true når siden laster
    toggleSort: boolean = true;     // toggle mellom sorterings retninger
    allPosts: Array<Post>;          // array med alle poster
    schemaRegister: FormGroup;              // skjema for utfylling av post
    singlePost: Post;               // brukes for å hente ut en post
    _headers: HttpHeaders;

    validation = {
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

    constructor(private _http: HttpClient, private fb: FormBuilder) {
        this.schemaRegister = fb.group(this.validation);
        this._headers = new HttpHeaders({ 
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + sessionStorage.getItem('Token')
        });
    }

    ngOnInit() {
        this.loading = true;
        this.getAllPosts();
        this.showTable = true;
    }

    getAllPosts() {
        this._http.get<Post[]>("api/post/", { headers: this._headers })
            .subscribe(posts => {
                this.allPosts = posts;
                this.loading = false;
            },
                error => console.log(error),
                () => console.log("done get-api/post")
            );
    };

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
        this.schemaRegister.setValue({
            id: "",
            datePosted: "",
            dateOccured: "",
            country: "",
            city: "",
            address: "",
            shape: "",
            summary: ""
        });
        this.schemaRegister.markAsPristine();
        this.showTable = false;
        this.showDetails = false;
        this.showSchemaRegister = true;
        this.showSchemaEdit = false;
    }

    backToList() {
        this.showTable = true;
        this.showDetails = false;
        this.showSchemaRegister = false;
        this.showSchemaEdit = false;
    }

    savePost() {
        const savedPost = new Post();

        var date = new Date();
        var str: string;
        str = date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate();

        savedPost.datePosted = str;
        savedPost.dateOccured = this.schemaRegister.value.dateOccured;
        savedPost.country = this.schemaRegister.value.country;
        savedPost.city = this.schemaRegister.value.city;
        savedPost.address = this.schemaRegister.value.address;
        savedPost.shape = this.schemaRegister.value.shape;
        savedPost.summary = this.schemaRegister.value.summary;

        this._http.post("api/post", savedPost, { headers: this._headers })
            .subscribe(retur => {
                this.getAllPosts();
                this.showTable = true;
                this.showDetails = false;
                this.showSchemaRegister = false;
                this.showSchemaEdit = false;
            },
                error => console.log(error)
            );
    };

    deletePost(id: number) {
        this._http.delete("api/post/" + id, { headers: this._headers })
            .subscribe(retur => {
                this.getAllPosts();
                this.showTable = true;
                this.showDetails = false;
                this.showSchemaRegister = false;
                this.showSchemaEdit = false;
            },
                error => console.log(error)
            );
    };

    editPost(id: number) {
        this._http.get<Post>("api/post/" + id, { headers: this._headers })
            .subscribe(
                post => {
                    this.schemaRegister.patchValue({ id: post.id });
                    this.schemaRegister.patchValue({ datePosted: post.datePosted });
                    this.schemaRegister.patchValue({ dateOccured: post.dateOccured });
                    this.schemaRegister.patchValue({ country: post.country });
                    this.schemaRegister.patchValue({ city: post.city });
                    this.schemaRegister.patchValue({ address: post.address });
                    this.schemaRegister.patchValue({ shape: post.shape });
                    this.schemaRegister.patchValue({ summary: post.summary });
                },
                error => console.log(error)
        );
        this.showTable = false;
        this.showDetails = false;
        this.showSchemaRegister = false;
        this.showSchemaEdit = true;
    }

    editAPost() {
        const editPost = new Post();

        editPost.id = this.schemaRegister.value.id;
        editPost.datePosted = this.schemaRegister.value.datePosted;
        editPost.dateOccured = this.schemaRegister.value.dateOccured;
        editPost.country = this.schemaRegister.value.country;
        editPost.city = this.schemaRegister.value.city;
        editPost.address = this.schemaRegister.value.address;
        editPost.shape = this.schemaRegister.value.shape;
        editPost.summary = this.schemaRegister.value.summary;

        this._http.put("api/post/", editPost, { headers: this._headers })
            .subscribe(
                retur => {
                    this.getAllPosts();
                    this.showTable = true;
                    this.showDetails = false;
                    this.showSchemaRegister = false;
                    this.showSchemaEdit = false;
                },
                error => console.log(error)
            );
    }

    postDetails(id: number) {
        this._http.get<Post>("api/post/" + id, { headers: this._headers })
            .subscribe(post => {
                this.singlePost = post;
                this.loading = false;
                this.showTable = false;
                this.showDetails = true;
                this.showSchemaRegister = false;
                this.showSchemaEdit = false;

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
        let sortedArray = this.allPosts;
        if (this.toggleSort) {
            sortedArray.sort((a, b) => (a.dateOccured < b.dateOccured) ? -1 : 1);
            this.toggleSort = false;
        }
        else {
            sortedArray.sort((a, b) => (a.dateOccured > b.dateOccured) ? -1 : 1);
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
