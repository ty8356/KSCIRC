import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { BehaviorSubject, Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { Gene } from '../models/gene';
import { GenesService } from '../services/genes.service';
import { LegendPosition } from '@swimlane/ngx-charts';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {

  // BEGIN GRAPH \\
  allData: any[] = [];
  enrichmentData: any[] = [];
  interactionData: any[] = [];
  ipData: any[] = [];
  inData: any[] = [];

  view: [number, number] = [800, 300];
  xAxisTicks: any[] = [ "2", "10", "42" ]

  // options
  lineLegend: boolean = true;
  barLegend: boolean = false;
  legendPosition: LegendPosition = LegendPosition.Below;
  showLabels: boolean = true;
  animations: boolean = true;
  showYAxisLabel: boolean = true;
  showXAxisLabel: boolean = true;
  xAxisLabel: string = 'DPI';
  yAxisLabel: string = 'L2 FC';
  timeline: boolean = false;
  gradient = false;
  showYAxis: boolean = true;
  showXAxis: boolean = true;

  // END GRAPH \\

  geneSearchControl = new FormControl();
  options: string[] = [ ];
  filteredOptions: Observable<string[]>;
  searchTerm: string = "";
  selectedGene: Gene = new Gene;

  constructor(
    public genesService: GenesService
  ) { }

  ngOnInit(): void {
    this.filteredOptions = this.geneSearchControl.valueChanges.pipe(
      startWith(''),
      map(value => this._filter(value)),
    );

    this.searchTerm = "Gapdh";
    this.onKeyUp();
  }

  private _filter(value: string): string[] {
    const filterValue = value.toLowerCase();

    return this.options.filter(option => option.toLowerCase().includes(filterValue));
  }

  onKeyUp() {
    if (this.searchTerm.length >= 2) {
      this.genesService.searchGenes(this.searchTerm)
        .subscribe(searchedGenes => {
          this.options = searchedGenes.map(x => (x.Name as string));

          if (searchedGenes.length < 10) {
            searchedGenes.forEach(x => {
              if (x.Name?.toUpperCase() === this.searchTerm.toUpperCase()) {
                this.setSelectedGene(x);
                this.populateGraph();
              }
            });
          }
          
          this.filteredOptions = this.geneSearchControl.valueChanges.pipe(
            startWith(''),
            map(value => this._filter(value)),
          );
        });
    } 
  }

  onOptionClick() {
    this.populateGraph();
    this.onKeyUp();
  }

  onEnter() {
    this.populateGraph();
    this.onKeyUp();
  }

  private setSelectedGene(gene: Gene) {
    this.selectedGene = gene;
  }

  private populateGraph() {
    this.genesService.getStatValuesByGene(this.searchTerm)
      .subscribe(statValues => {

        let enrichment: any[] = [];
        let interaction: any[] = [];
        let input: any[] = [];
        let ip: any[] = [];

        statValues.forEach(x => {
          enrichment.push({
            "name": x.DaysPostInjury,
            "value": x.EnrichmentValue
          });
          interaction.push({
            "name": x.DaysPostInjury,
            "value": x.InteractionValue
          });
          input.push({
            "name": x.DaysPostInjury,
            "value": x.InputValue
          });
          ip.push({
            "name": x.DaysPostInjury,
            "value": x.ImmunoprecipitateValue
          });
        });

        this.allData = [
          {
            "name": "Enrichment",
            "series": enrichment
          },
          {
            "name": "Interaction",
            "series": interaction
          },
          {
            "name": "IN",
            "series": input
          },
          {
            "name": "IP",
            "series": ip
          }
        ];

        this.enrichmentData = enrichment.sort((a, b) => (a.name > b.name) ? 1 : -1);
        this.interactionData = interaction.sort((a, b) => (a.name > b.name) ? 1 : -1);
        this.ipData = ip.sort((a, b) => (a.name > b.name) ? 1 : -1);
        this.inData = input.sort((a, b) => (a.name > b.name) ? 1 : -1);

      });
  }

}
