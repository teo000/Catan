import {ComputeSettlementSpotsInfo} from "../board/settlements/ComputeSettlementSpotsInfo";
import {ComputeRoadSpotsInfo} from "../board/roads/ComputeRoadSpotsInfo";

export class SpotsInfo {
    static SettlementSpotInfo = ComputeSettlementSpotsInfo();
    static RoadSpotInfo = ComputeRoadSpotsInfo();
}