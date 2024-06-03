import {ComputeSettlementSpotsInfo} from "../board/settlements/ComputeSettlementSpotsInfo";
import {ComputeRoadSpotsInfo} from "../board/roads/ComputeRoadSpotsInfo";
import {ComputeHexTilesInfo} from "../board/hexTiles/HexTileRow";

export class MapInfo {
    static SettlementSpotInfo = ComputeSettlementSpotsInfo();
    static RoadSpotInfo = ComputeRoadSpotsInfo();
    static HexTileInfo = ComputeHexTilesInfo();
}