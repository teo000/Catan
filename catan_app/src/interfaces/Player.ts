import {ResourceCount} from "./ResourceCount";
import {SettlementDto} from "./SettlementDto";
import {RoadDto} from "./RoadDto";

export interface Player {
    id: string;
    name: string;
    isActive: boolean;
    resourceCount: ResourceCount;
    settlements: SettlementDto[];
    roads: RoadDto[];
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