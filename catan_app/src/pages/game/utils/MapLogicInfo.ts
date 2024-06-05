export class MapLogicInfo{
    static adjacentSettlements: null | number[][] = null;
    static roadEnds: null | [number, number][] = null;
    static roadByRoadEnds: null | Map<string, number>

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
        function computeRoadByRoadEnds(list : [number, number][]) {
            const map: Map<string, number> = new Map();

            list.forEach((item, index) => {
                if (item[0] > item[1]){
                    let aux = item[0];
                    item[0] = item[1];
                    item[1] = aux;
                }

                map.set(JSON.stringify(item), index);
            });

            return map;
        }

        if (!this.roadEnds) {
            const response = await fetch('/gameMapData/roadEnds.json');
            this.roadEnds = await response.json();
            console.log(this.roadEnds);
            if (!this.roadEnds)
                throw new Error("Error initializing map logic info.")
            this.roadByRoadEnds = computeRoadByRoadEnds(this.roadEnds);
            console.log(this.roadByRoadEnds);
        }
    }

    static getRoadEnds() {
        if (this.roadEnds === null)
            throw new Error("Road ends must be initialized at start-up");
        return this.roadEnds;
    }

    static getRoadByRoadEnds(){
        if (this.roadByRoadEnds === null)
            throw new Error("Map logic info must be initialized at start-up");
        return this.roadByRoadEnds;
    }
}