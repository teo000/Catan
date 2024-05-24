import {SettlementSpot} from "./SettlementSpot";
import {MapConstants} from "../MapConstants";
import {start} from "repl";

type SpotRowLayout = {
    count: number;
    marginLeft: number;
    top: number;
}

const SpotsLayoutByRowNumber : Record<number, SpotRowLayout> = {
    1: {count: 3, marginLeft: MapConstants.HEX_WIDTH * 3/2, top: 0},
    2: {count: 4, marginLeft: MapConstants.HEX_WIDTH, top: MapConstants.HEX_HEIGHT / 3},

    3: {count: 4, marginLeft: MapConstants.HEX_WIDTH, top: MapConstants.HEX_HEIGHT},
    4: {count: 5, marginLeft: MapConstants.HEX_WIDTH / 2, top: MapConstants.HEX_HEIGHT * 4/3},

    5: {count: 5, marginLeft: MapConstants.HEX_WIDTH / 2, top: MapConstants.HEX_HEIGHT * 2},
    6: {count: 6, marginLeft: 0, top: MapConstants.HEX_HEIGHT * 7/3},

    7: {count: 6, marginLeft: 0, top: MapConstants.HEX_HEIGHT * 3},
    8: {count: 5, marginLeft: MapConstants.HEX_WIDTH / 2, top: MapConstants.HEX_HEIGHT * 10/3},

    9: {count: 5, marginLeft: MapConstants.HEX_WIDTH / 2, top: MapConstants.HEX_HEIGHT * 4},
    10: {count: 4, marginLeft: MapConstants.HEX_WIDTH, top: MapConstants.HEX_HEIGHT * 13/3},

    11: {count: 4, marginLeft: MapConstants.HEX_WIDTH, top: MapConstants.HEX_HEIGHT * 5},
    12: {count: 3, marginLeft: MapConstants.HEX_WIDTH * 3 / 2, top: MapConstants.HEX_HEIGHT * 16/3},
}

export type SettlementSpotInfo = {
    id: number;
    left: number;
    top: number;
}

function ComputeSettlementSpotRow(rowNumber: number, startingNumber: number){
    const spotsInfo = [];
    const {count, marginLeft, top} = SpotsLayoutByRowNumber[rowNumber];
    let left = marginLeft

    for (let i = 0; i<count; i++){
        let settlementInfo : SettlementSpotInfo = {
            id: startingNumber + i,
            left: left - MapConstants.SETTLEMENT_SPOT_SIZE/2,
            top: top - MapConstants.SETTLEMENT_SPOT_SIZE/2
        };

        spotsInfo.push(settlementInfo)
        left += MapConstants.HEX_WIDTH;
    }

    return spotsInfo

}

function ComputeSettlementSpotsInfo(){
    let settlements = [];
    let startingNumber = 0;

    for (let i = 1; i <= 12; i++) {
        let row = ComputeSettlementSpotRow(i, startingNumber);
        startingNumber += row.length;
        settlements.push(...row);
    }

    return settlements
}

export {ComputeSettlementSpotRow, ComputeSettlementSpotsInfo};