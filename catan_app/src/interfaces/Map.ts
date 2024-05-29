import {HexTileDto} from "./HexTileDto";
import {SettlementDto} from "./SettlementDto";
import {RoadDto} from "./RoadDto";
import {CityDto} from "./CityDto";

export interface Map {
    hexTiles: HexTileDto[];
    thiefPosition: number;
    settlements: SettlementDto[];
    cities: CityDto[];
    roads: RoadDto[];
}