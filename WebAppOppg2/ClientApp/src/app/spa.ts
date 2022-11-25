import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Post } from "./Post";

@Component({
    selector: "app-root",
    templateUrl: "SPA.html"
})
export class SPA {

    showSchemaRegister: boolean;
    showArray: boolean;
    allPosts: Array<Post>;
    schema: FormGroup;
    loading: boolean;

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
        this.schema = fb.group(this.validation);
    }

    ngOnInit() {
        this.loading = true;
        this.getAllPosts();
        this.showArray = true;
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
    };

    submit() {
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
        this.showArray = false;
        this.showSchemaRegister = true;
    }

    backToList() {
        this.showArray = true;
    }

    savePost() {
        const savedPost = new Post();
        // sjekker om datoen er tom
        if (!this.schema.value.datePosted) {
            this.schema.value.datePosted = new Date();
        }

        savedPost.datePosted = this.schema.value.datePosted;
        savedPost.dateOccured = this.schema.value.dateOccured;
        savedPost.country = this.schema.value.country;
        savedPost.city = this.schema.value.city;
        savedPost.address = this.schema.value.address;
        savedPost.shape = this.schema.value.shape;
        savedPost.summary = this.schema.value.summary;

        this._http.post("api/post", savedPost)
            .subscribe(retur => {
                this.getAllPosts();
                this.showSchemaRegister = false;
                this.showArray = true;
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
        this.showSchemaRegister = false;
        this.showArray = false;
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
                    this.showArray = true;
                },
                error => console.log(error)
            );
    }
}
