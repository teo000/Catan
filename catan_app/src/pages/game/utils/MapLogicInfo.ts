export class MapLogicInfo{
    static adjacentSettlements: null | number[][] = null;
    static roadEnds: null | number[][] = null;

    static async load(){
        await this.loadAdjacentSettlements();
        await this.loadRoadEnds();
    }

    static async loadAdjacentSettlements() {
        if (!this.adjacentSettlements) {
            const response = await fetch('/gameMapData/adjacentSettlements.json');
            this.adjacentSettlements = await response.json();
            console.log(this.adjacentSettlements);
        }
    }

    static getAdjacentSettlements() {
        if (this.adjacentSettlements === null)
            throw new Error("Adjacency matrix must be initialized at start-up");
        return this.adjacentSettlements;
    }

    static async loadRoadEnds() {
        if (!this.roadEnds) {
            const response = await fetch('/gameMapData/roadEnds.json');
            this.roadEnds = await response.json();
            console.log(this.roadEnds);
        }
    }

    static getRoadEnds() {
        if (this.roadEnds === null)
            throw new Error("Road ends must be initialized at start-up");
        return this.roadEnds;
    }
}