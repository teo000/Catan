import {MapConstants} from "./MapConstants";

type HexTileData = {
    resource: string
    numberToken: number
}

const ResourcePaths : Record<string, string> = {
    BRICK: 'images/map/brick.png',
    WOOD: 'images/map/wood.png',
    SHEEP: 'images/map/sheep.png',
    WHEAT: 'images/map/wheat.png',
    ORE: 'images/map/ore.png',
    DESERT: 'images/map/desert.png',
};

const NUMBER_PATH = 'images/numbers/'
function getNumberPath(number: number) : string {
    return NUMBER_PATH + number + '.png'
}

function HexTile({top, left, hexTileData}
                     : {top: number, left: number, hexTileData: HexTileData}) {
    const {resource, numberToken} = hexTileData
    const resourcePath : string = ResourcePaths[resource.toUpperCase()];

    const numberTokenPath = getNumberPath(numberToken);

    return (
        <div>
            <img
                className="hexTile"
                src={resourcePath}
                alt="hexagon"
                style={{
                    top: `${top}px`,
                    left: `${left}px`
                }}
            />
            <img
                className="numberToken"
                src = {numberTokenPath}
                alt="number"
                style={{
                    width: MapConstants.NUMBER_TOKEN_SIZE,
                    top: `${top + MapConstants.HEX_HEIGHT/2 }px`,
                    left: `${left + MapConstants.HEX_WIDTH/2 - MapConstants.NUMBER_TOKEN_SIZE/2}px`
                }}
            />
        </div>
    )
}
export {HexTile};
export type {HexTileData}