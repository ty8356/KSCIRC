import { Component, OnInit } from '@angular/core';
import { Gene } from '../models/gene';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {

  searchTerm: String = "";

  constructor() { }

  ngOnInit(): void {
  }

}
