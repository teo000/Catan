import {HexTileDto} from "./HexTileDto";
import {SettlementDto} from "./SettlementDto";
import {RoadDto} from "./RoadDto";

export interface Map {
    hexTiles: HexTileDto[];
    thiefPosition: number;
    settlements: SettlementDto[];
    roads: RoadDto[];
}