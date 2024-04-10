import {HexTile} from "./HexTile";
import {HexTileData} from "./HexTile";

const HEX_HEIGHT = 90;
const HEX_WIDTH = 100

type HexagonRowLayout = {
    count: number;
    marginLeft: number;
    top: number;
}

const HexagonLayoutByRowNumber : Record<number, HexagonRowLayout> = {
    1: {count: 3, marginLeft: HEX_WIDTH, top: 0},
    2: {count: 4, marginLeft: HEX_WIDTH / 2, top: HEX_HEIGHT},
    3: {count: 5, marginLeft: 0, top: HEX_HEIGHT * 2},
    4: {count: 4, marginLeft: HEX_WIDTH / 2, top: HEX_HEIGHT * 3},
    5: {count: 3, marginLeft: HEX_WIDTH, top: HEX_HEIGHT * 4},
}

function HexTileRow({rowNumber, hexTileData} : {rowNumber: number, hexTileData: HexTileData[]}){
    const hexagons = [];

    const {marginLeft, top, count} = HexagonLayoutByRowNumber[rowNumber];
    let left = marginLeft + 5;

    for (let i = 0; i < count ; i++) {
        hexagons.push(<HexTile key={i} left={left} top={top} hexTileData={hexTileData[i]}/>);
        left += HEX_WIDTH;
    }
    return <div className="HexTileRow" style={{ marginLeft: `${marginLeft}px` }}>{hexagons}</div>;
}

export {HexTileRow};