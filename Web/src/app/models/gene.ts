export class Gene {
    Id?: number;
    EnsId?: number;
    Name?: string;
    Description?: string;

    constructor(
        id: number = 0,
        ensId: number = 0,
        name: string = "",
        description: string = ""
    ) {
        this.Id = id,
        this.EnsId = ensId,
        this.Name = name,
        this.Description = description
    }
}