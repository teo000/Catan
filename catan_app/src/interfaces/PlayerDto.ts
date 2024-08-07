import {ResourceCountDto} from "./ResourceCountDto";
import {SettlementDto} from "./SettlementDto";
import {RoadDto} from "./RoadDto";
import {CityDto} from "./CityDto";
import {DevelopmentCardDto} from "./DevelopmentCardDto";

export interface PlayerDto {
    id: string;
    name: string;
    isActive: boolean;
    resourceCount: ResourceCountDto;
    tradeCount: ResourceCountDto;
    settlements: SettlementDto[];
    roads: RoadDto[];
    cities: CityDto[];
    developmentCards: DevelopmentCardDto[];
    color: string;
    winningPoints: number;
    discardedThisTurn: boolean;
    knightsPlayed: number;
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