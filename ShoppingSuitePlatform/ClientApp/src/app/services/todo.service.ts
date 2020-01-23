import { Injectable } from "@angular/core";
import { TodoEntry } from "../domainTypes/todoEntry";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable()
export class TodoService {

    constructor(private http: HttpClient) { }

    public getTodo = (id: number): Observable<TodoEntry> => this.http.get<TodoEntry>('https://jsonplaceholder.typicode.com/todos/' + id);
}