import { Component, OnInit } from '@angular/core';
import { Gene } from '../models/gene';
import { GenesService } from '../services/genes.service';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {

  searchTerm: string = "";
  genes: Gene[] = null as any;

  constructor(
    public genesService: GenesService
  ) { }

  ngOnInit(): void {
  }

  onEnter() {
    console.log('You hit enter! ' + this.searchTerm);

    this.genesService.searchGenes(this.searchTerm)
      .subscribe(searchedGenes => {
        console.log(searchedGenes[0]);
        this.genes = searchedGenes;
        console.log(this.genes[0]);
      });
  }

}
