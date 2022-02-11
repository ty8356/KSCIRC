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
        // this.baseUrl = `http://localhost:5000/genes`;
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

    downloadAllFile() {

        let url = this.baseUrl + `/download-all`;
        
        return this.http.get(url, { responseType: 'blob' });
        
    }

    downloadAdvancedSearch(min: number, max: number) {

        let url = this.baseUrl + `/download-advanced?min=` + min + `&max=` + max;
        
        return this.http.get(url, { responseType: 'blob' });
        
    }
}