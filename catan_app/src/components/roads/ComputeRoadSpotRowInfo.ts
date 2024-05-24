import {MapConstants} from "../MapConstants";
import {RoadSpot} from "./RoadSpot";

type RoadRowLayout = {
    count: number;
    marginLeft: number;
    top: number;
    rotation: RotationType;
    spaceBetween: number;
}

enum RotationType {
    VERTICAL,
    STARTS_UP,
    STARTS_DOWN
}

const RoadsLayoutByRowNumber : Record<number, RoadRowLayout> = {
    1: {count: 6, marginLeft: MapConstants.HEX_WIDTH * 5/4, top: MapConstants.HEX_HEIGHT / 6,
        rotation: RotationType.STARTS_UP, spaceBetween: MapConstants.HEX_WIDTH/2},
    2: {count: 4, marginLeft: MapConstants.HEX_WIDTH, top: MapConstants.HEX_HEIGHT * 2/3,
        rotation: RotationType.VERTICAL, spaceBetween: MapConstants.HEX_WIDTH},

    3: {count: 8, marginLeft: MapConstants.HEX_WIDTH * 3/4, top: MapConstants.HEX_HEIGHT * 7/6,
        rotation: RotationType.STARTS_UP, spaceBetween: MapConstants.HEX_WIDTH/2},
    4: {count: 5, marginLeft: MapConstants.HEX_WIDTH / 2, top: MapConstants.HEX_HEIGHT * 5/3,
        rotation: RotationType.VERTICAL, spaceBetween: MapConstants.HEX_WIDTH},

    5: {count: 10, marginLeft: MapConstants.HEX_WIDTH / 4, top: MapConstants.HEX_HEIGHT * 13/6,
        rotation: RotationType.STARTS_UP, spaceBetween: MapConstants.HEX_WIDTH/2},
    6: {count: 6, marginLeft: 0, top: MapConstants.HEX_HEIGHT * 8/3,
        rotation: RotationType.VERTICAL, spaceBetween: MapConstants.HEX_WIDTH},
    7: {count: 10, marginLeft: MapConstants.HEX_WIDTH / 4, top: MapConstants.HEX_HEIGHT * 19/6,
        rotation: RotationType.STARTS_DOWN, spaceBetween: MapConstants.HEX_WIDTH/2},

    8: {count: 5, marginLeft: MapConstants.HEX_WIDTH / 2, top: MapConstants.HEX_HEIGHT * 11/3,
        rotation: RotationType.VERTICAL, spaceBetween: MapConstants.HEX_WIDTH},
    9: {count: 8, marginLeft: MapConstants.HEX_WIDTH * 3/4, top: MapConstants.HEX_HEIGHT * 25/6,
        rotation: RotationType.STARTS_DOWN, spaceBetween: MapConstants.HEX_WIDTH/2},

    10: {count: 4, marginLeft: MapConstants.HEX_WIDTH, top: MapConstants.HEX_HEIGHT * 14/3,
        rotation: RotationType.VERTICAL, spaceBetween: MapConstants.HEX_WIDTH},
    11: {count: 6, marginLeft: MapConstants.HEX_WIDTH * 5/4, top: MapConstants.HEX_HEIGHT * 31/6,
        rotation: RotationType.STARTS_DOWN, spaceBetween: MapConstants.HEX_WIDTH/2},
}

export type RoadSpotInfo = {
    id: number,
    left: number,
    top: number,
    rotation: number
}

function ComputeRoadSpotRowInfo(rowNumber: number, startingNumber: number){
    const spotsInfo = [];
    const {count, marginLeft, top,
        rotation, spaceBetween} = RoadsLayoutByRowNumber[rowNumber];
    let left = marginLeft
    let degrees;
    switch(rotation){
        case RotationType.VERTICAL: degrees = 0; break;
        case RotationType.STARTS_UP: degrees = 60; break;
        case RotationType.STARTS_DOWN: degrees = -60; break;
    }

    for (let i = 0; i<count; i++){
        let spot : RoadSpotInfo = {
            id: startingNumber + i,
            left: left - MapConstants.ROAD_SPOT_SIZE / 2,
            top: top - MapConstants.ROAD_SPOT_SIZE / 2,
            rotation: i % 2 === 0 ? degrees : -degrees
        }
        spotsInfo.push(spot);
        left += spaceBetween;
    }

    return spotsInfo;

}

export {ComputeRoadSpotRowInfo};