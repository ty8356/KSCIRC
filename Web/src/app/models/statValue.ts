export class StatValue {
    Id?: number;
    DaysPostInjury?: number;
    InputValue?: number;
    InputQValue?: number;
    ImmunoprecipitateValue?: number;
    ImmunoprecipitateQValue?: number;
    EnrichmentValue?: number;
    EnrichmentQValue?: number;
    InteractionValue?: number;
    InteractionQValue?: number;

    constructor(
        id: number = 0,
        daysPostInjury: number = 0,
        inputValue: number = 0,
        inputQValue: number = 0,
        immunoprecipitateValue: number = 0,
        immunoprecipitateQValue: number = 0,
        enrichmentValue: number = 0,
        enrichmentQValue: number = 0,
        interactionValue: number = 0,
        interactionQValue: number = 0
    ) {
        this.Id = id,
        this.DaysPostInjury = daysPostInjury,
        this.InputValue = inputValue,
        this.InputQValue = inputQValue,
        this.ImmunoprecipitateValue = immunoprecipitateValue,
        this.ImmunoprecipitateQValue = immunoprecipitateQValue,
        this.EnrichmentValue = enrichmentValue,
        this.EnrichmentQValue = enrichmentQValue,
        this.InteractionValue = interactionValue,
        this.InteractionQValue = interactionQValue
    }
}