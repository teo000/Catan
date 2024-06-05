import {ComputeSettlementSpotsInfo} from "../board/settlements/ComputeSettlementSpotsInfo";
import {ComputeRoadSpotsInfo} from "../board/roads/ComputeRoadSpotsInfo";
import {ComputeHexTilesInfo} from "../board/hexTiles/HexTileRow";

export class MapDrawInfo {
    static SettlementSpotInfo = ComputeSettlementSpotsInfo();
    static RoadSpotInfo = ComputeRoadSpotsInfo();
    static HexTileInfo = ComputeHexTilesInfo();
}