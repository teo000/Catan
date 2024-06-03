import {HexTile} from "./HexTile";
import {HexTileDto} from "./HexTile";
import {MapConstants} from "../../utils/MapConstants";


type HexagonRowLayout = {
    count: number;
    marginLeft: number;
    top: number;
}

const HexagonLayoutByRowNumber : Record<number, HexagonRowLayout> = {
    1: {count: 3, marginLeft: MapConstants.HEX_WIDTH, top: MapConstants.MARGIN_TOP},
    2: {count: 4, marginLeft: MapConstants.HEX_WIDTH / 2, top: MapConstants.HEX_HEIGHT + MapConstants.MARGIN_TOP},
    3: {count: 5, marginLeft: 0, top: MapConstants.HEX_HEIGHT * 2 + MapConstants.MARGIN_TOP},
    4: {count: 4, marginLeft: MapConstants.HEX_WIDTH / 2, top: MapConstants.HEX_HEIGHT * 3 + MapConstants.MARGIN_TOP},
    5: {count: 3, marginLeft: MapConstants.HEX_WIDTH, top: MapConstants.HEX_HEIGHT * 4 + MapConstants.MARGIN_TOP},
}

function HexTileRow({rowNumber, hexTileData} : {rowNumber: number, hexTileData: HexTileDto[]}){
    const hexagons = [];

    const {marginLeft, top, count} = HexagonLayoutByRowNumber[rowNumber];
    let left = marginLeft ;

    for (let i = 0; i < count ; i++) {
        hexagons.push(<HexTile key={i} left={left} top={top} hexTileData={hexTileData[i]}/>);
        left += MapConstants.HEX_WIDTH;
    }
    return <div className="hexTileRow" style={{ marginLeft: `${marginLeft}px` }}>{hexagons}</div>;
}

function ComputeHexTilesInfo(){
    const hexagons = [];
    let index = 0;
    for (let rowNumber = 1; rowNumber <= 5; rowNumber++) {
        const {marginLeft, top, count} = HexagonLayoutByRowNumber[rowNumber];
        let left = marginLeft;

        for (let i = 0; i < count; i++) {
            hexagons.push({id: index, top: top, left: left});
            left += MapConstants.HEX_WIDTH;
            index++;
        }
    }
    return hexagons;
}

export {HexTileRow, ComputeHexTilesInfo};