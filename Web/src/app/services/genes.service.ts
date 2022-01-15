import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs';
import { Gene } from '../models/gene';

@Injectable({
providedIn: 'any'
})

export class GenesService {

    constructor(
        private http: HttpClient
    ) {
        
    }

    searchGenes(name: string): Observable<Gene[]> {

        let url = `http://localhost:5000/api/genes`;
        if (name != "" && name != null && name != 'undefined') {
            url += '?name=' + name;
        }

        console.log(url);

        return this.http.get<Gene[]>(url);
    }
}