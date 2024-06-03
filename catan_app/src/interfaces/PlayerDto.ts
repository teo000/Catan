import {ResourceCountDto} from "./ResourceCountDto";
import {SettlementDto} from "./SettlementDto";
import {RoadDto} from "./RoadDto";
import {CityDto} from "./CityDto";

export interface PlayerDto {
    id: string;
    name: string;
    isActive: boolean;
    resourceCount: ResourceCountDto;
    settlements: SettlementDto[];
    roads: RoadDto[];
    cities: CityDto[];
    color: string;
    winningPoints: number;
}

export enum PlayerColor {
    RED = "#FF0000",
    GREEN = "#00FF00",
    BLUE = "#0000FF",
    YELLOW = "#FFFF00"
}


export const getPlayerColor = (color: string): PlayerColor => {
    switch(color.toLowerCase()) {
        case 'red':
            return PlayerColor.RED;
        case 'green':
            return PlayerColor.GREEN;
        case 'blue':
            return PlayerColor.BLUE;
        case 'yellow':
            return PlayerColor.YELLOW;
        default:
            throw new Error(`Unknown color: ${color}`);
    }
}