export class StatValue {
    Id?: number;
    DaysPostInjury?: number;
    InputValue?: number;
    InputQvalue?: number;
    ImmunoprecipitateValue?: number;
    ImmunoprecipitateQvalue?: number;
    EnrichmentValue?: number;
    EnrichmentQvalue?: number;
    InteractionValue?: number;
    InteractionQvalue?: number;

    constructor(
        id: number = 0,
        daysPostInjury: number = 0,
        inputValue: number = 0,
        inputQvalue: number = 0,
        immunoprecipitateValue: number = 0,
        immunoprecipitateQvalue: number = 0,
        enrichmentValue: number = 0,
        enrichmentQvalue: number = 0,
        interactionValue: number = 0,
        interactionQvalue: number = 0
    ) {
        this.Id = id,
        this.DaysPostInjury = daysPostInjury,
        this.InputValue = inputValue,
        this.InputQvalue = inputQvalue,
        this.ImmunoprecipitateValue = immunoprecipitateValue,
        this.ImmunoprecipitateQvalue = immunoprecipitateQvalue,
        this.EnrichmentValue = enrichmentValue,
        this.EnrichmentQvalue = enrichmentQvalue,
        this.InteractionValue = interactionValue,
        this.InteractionQvalue = interactionQvalue
    }
}