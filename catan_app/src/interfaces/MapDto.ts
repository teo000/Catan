import {HexTileDto} from "./HexTileDto";
import {SettlementDto} from "./SettlementDto";
import {RoadDto} from "./RoadDto";
import {CityDto} from "./CityDto";

export interface MapDto {
    hexTiles: HexTileDto[];
    thiefPosition: number;
    settlements: SettlementDto[];
    cities: CityDto[];
    roads: RoadDto[];
}