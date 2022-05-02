import { Component, OnInit, TemplateRef, HostListener } from '@angular/core';
import { FormControl } from '@angular/forms';
import { BehaviorSubject, Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { Gene } from '../models/gene';
import { GenesService } from '../services/genes.service';
import { LegendPosition } from '@swimlane/ngx-charts';
import { Options } from '@angular-slider/ngx-slider';
import { NgxSpinnerService } from "ngx-spinner";

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {

  innerWidth: number = window.innerWidth;

  // BEGIN GRAPH \\
  allData: any[] = [];
  enrichmentData: any[] = [];
  interactionData: any[] = [];
  ipData: any[] = [];
  inData: any[] = [];

  enrichmentColors: any[] = [];
  interactionColors: any[] = [];
  ipColors: any[] = [];
  inColors: any[] = [];

  view: [number, number] = [this.innerWidth <= 800 ? this.innerWidth - 60 : 800, 300];
  xAxisTicks: any[] = [ "0", "2", "10", "42" ]

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

  minValue: number = -10;
  maxValue: number = 10;
  rangeSliderOptions: Options = {
    floor: -15,
    ceil: 15,
    step: 0.5
  };

  selectedSearchMethod: string = '0';
  searchMethods: any[] = [
    {value: '0', viewValue: 'L2FC Range'},
    {value: '1', viewValue: 'Top 500'}
  ];

  selectedRegulationType: string = '0';
  regulationTypes: any[] = [
    {value: '0', viewValue: 'Upregulated'},
    {value: '1', viewValue: 'Downregulated'}
  ];

  showSigLegend: boolean = false;
  // showReadCounts: boolean = false;

  geneSearchControl = new FormControl();
  options: string[] = [ ];
  filteredOptions: Observable<string[]>;
  searchTerm: string = "";
  selectedGene: Gene = new Gene;

  constructor(
    public genesService: GenesService,
    public spinnerService: NgxSpinnerService
  ) { }

  ngOnInit(): void {
    this.spinnerService.show();
    this.filteredOptions = this.geneSearchControl.valueChanges.pipe(
      startWith(''),
      map(value => this._filter(value)),
    );

    this.searchTerm = "Gapdh";
    this.onKeyUp();

    this.innerWidth = window.innerWidth;
  }

  private _filter(value: string): string[] {
    const filterValue = value.toLowerCase();

    return this.options.filter(option => option.toLowerCase().includes(filterValue));
  }

  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    this.innerWidth = window.innerWidth;
    this.view = [this.innerWidth <= 800 ? this.innerWidth - 60 : 800, 300]
    console.log(this.view);
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

          this.spinnerService.hide();
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

  toggleSigLegend($event: any) {
    if ($event.index == 0) {
      this.showSigLegend = false;
    } else {
      this.showSigLegend = true;
    }
  }

  downloadAll() {
    this.spinnerService.show();

    this.genesService.downloadAllFile().subscribe((data: any) => {
      this.spinnerService.hide();

      const a = document.createElement('a');
      a.setAttribute('style', 'display:none;');
      document.body.appendChild(a);
      a.download = 'Log2FC_Qvalue_Merged_ForAllResults';
      a.href = URL.createObjectURL(data);
      a.target = '_blank';
      a.click();
      document.body.removeChild(a);
    });
  }

  downloadAdvanced() {

    this.spinnerService.show();

    if (this.selectedSearchMethod == '0') { // L2FC RANGE

      this.genesService.downloadAdvancedSearch(this.minValue, this.maxValue).subscribe((data: any) => {
        this.spinnerService.hide();

        const a = document.createElement('a');
        a.setAttribute('style', 'display:none;');
        document.body.appendChild(a);
        a.download = 'ol_gene_exp_search_range.xlsx';
        a.href = URL.createObjectURL(data);
        a.target = '_blank';
        a.click();
        document.body.removeChild(a);
      });

    } 
    else if (this.selectedSearchMethod == '1') { // TOP

      if (this.selectedRegulationType == '0') { // upreg

        this.genesService.downloadTopUpregulated().subscribe((data: any) => {
          this.spinnerService.hide();
  
          const a = document.createElement('a');
          a.setAttribute('style', 'display:none;');
          document.body.appendChild(a);
          a.download = 'ol_gene_exp_search_top500_upregulated.xlsx';
          a.href = URL.createObjectURL(data);
          a.target = '_blank';
          a.click();
          document.body.removeChild(a);
        });

      } 
      else if (this.selectedRegulationType == '1') { // downreg

        this.genesService.downloadTopDownregulated().subscribe((data: any) => {
          this.spinnerService.hide();
  
          const a = document.createElement('a');
          a.setAttribute('style', 'display:none;');
          document.body.appendChild(a);
          a.download = 'ol_gene_exp_search_top500_downregulated.xlsx';
          a.href = URL.createObjectURL(data);
          a.target = '_blank';
          a.click();
          document.body.removeChild(a);
        });

      }

    }
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
            "value": x.EnrichmentValue?.toFixed(3),
            "extra": {
              "qvalue": x.EnrichmentQvalue?.toFixed(3)
            }
          });
          if (x.DaysPostInjury != 0) {
            interaction.push({
              "name": x.DaysPostInjury,
              "value": x.InteractionValue?.toFixed(3),
              "extra": {
                "qvalue": x.InteractionQvalue?.toFixed(3)
              }
            });
            input.push({
              "name": x.DaysPostInjury,
              "value": x.InputValue?.toFixed(3),
              "extra": {
                "qvalue": x.InputQvalue?.toFixed(3),
                "medianReadCount": x.InputMedianReadCount?.toFixed(3)
              }
            });
            ip.push({
              "name": x.DaysPostInjury,
              "value": x.ImmunoprecipitateValue?.toFixed(3),
              "extra": {
                "qvalue": x.ImmunoprecipitateQvalue?.toFixed(3),
                "medianReadCount": x.ImmunoprecipitateMedianReadCount?.toFixed(3)
              }
            });
          }
        });

        this.allData = [
          {
            "name": "OL Enrichment",
            "series": enrichment
          },
          {
            "name": "Change Over Time",
            "series": interaction
          },
          {
            "name": "Total mRNA",
            "series": input
          },
          {
            "name": "OL mRNA",
            "series": ip
          }
        ];

        this.enrichmentData = enrichment.sort((a, b) => (a.name > b.name) ? 1 : -1);
        this.interactionData = interaction.sort((a, b) => (a.name > b.name) ? 1 : -1);
        this.ipData = ip.sort((a, b) => (a.name > b.name) ? 1 : -1);
        this.inData = input.sort((a, b) => (a.name > b.name) ? 1 : -1);

        this.enrichmentColors = [];
        this.enrichmentData.forEach(x => {
          if (x.extra.qvalue <= 0.05) {
            this.enrichmentColors.push({
              "name": x.name.toString(),
              "value": "#AD0000"
            });
          } else {
            this.enrichmentColors.push({
              "name": x.name.toString(),
              "value": "#A9A9A9"
            });
          }
        });

        this.interactionColors = [];
        this.interactionData.forEach(x => {
          if (x.extra.qvalue <= 0.05) {
            this.interactionColors.push({
              "name": x.name.toString(),
              "value": "#AD0000"
            });
          } else {
            this.interactionColors.push({
              "name": x.name.toString(),
              "value": "#A9A9A9"
            });
          }
        });

        this.ipColors = [];
        this.ipData.forEach(x => {
          if (x.extra.qvalue <= 0.05) {
            this.ipColors.push({
              "name": x.name.toString(),
              "value": "#AD0000"
            });
          } else {
            this.ipColors.push({
              "name": x.name.toString(),
              "value": "#A9A9A9"
            });
          }
        });

        this.inColors = [];
        this.inData.forEach(x => {
          if (x.extra.qvalue <= 0.05) {
            this.inColors.push({
              "name": x.name.toString(),
              "value": "#AD0000"
            });
          } else {
            this.inColors.push({
              "name": x.name.toString(),
              "value": "#A9A9A9"
            });
          }
        });
      });
  }

}
