import {HexTileDto} from "./HexTileDto";
import {SettlementDto} from "./SettlementDto";
import {Road} from "./Road";

export interface Map {
    hexTiles: HexTileDto[];
    thiefPosition: number;
    settlements: SettlementDto[];
    roads: Road[];
}