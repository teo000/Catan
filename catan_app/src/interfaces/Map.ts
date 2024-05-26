import {HexTile} from "./HexTile";

export interface Map {
    hexTiles: HexTile[];
    thiefPosition: number;
    settlements: any[]; // Adjust type as needed
    roads: any[]; // Adjust type as needed
}