import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs';
import { Gene } from '../models/gene';
import { StatValue } from '../models/statValue';
import { environment } from 'src/environments/environment';

@Injectable({
providedIn: 'any'
})

export class GenesService {

    baseUrl: string = "";

    constructor(
        private http: HttpClient
    ) {
        this.baseUrl = `api/genes`;
    }

    searchGenes(name: string): Observable<Gene[]> {

        let url = this.baseUrl;
        if (name != "" && name != null && name != 'undefined') {
            url += '?name=' + name;
        }

        return this.http.get<Gene[]>(url);

    }

    getStatValuesByGene(name: string): Observable<StatValue[]> {

        let url = this.baseUrl + `/` + name + `/stat-values`;
        
        return this.http.get<StatValue[]>(url);
        
    }
}