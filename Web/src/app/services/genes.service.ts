import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs';
import { Gene } from '../models/gene';
import { StatValue } from '../models/statValue';

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

        return this.http.get<Gene[]>(url);

    }

    getStatValuesByGene(name: string): Observable<StatValue[]> {

        let url = `http://localhost:5000/api/genes/` + name + `/stat-values`;
        
        return this.http.get<StatValue[]>(url);
        
    }
}