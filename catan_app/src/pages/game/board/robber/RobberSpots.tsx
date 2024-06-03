import {MapInfo} from "../../utils/MapInfo";
import {MapConstants} from "../../utils/MapConstants";
import {RobberSpot} from "./RobberSpot";

function RobberSpots({visible, onRobberSpotClick, currentSpot} :
                         {visible:boolean, onRobberSpotClick : (position:number) => void , currentSpot:number}) {
    const hexTileInfo = MapInfo.HexTileInfo;

    if (!visible)
        return <></>

    return (
        <div className="robber-spots">
            {hexTileInfo.map(spot => (
                (spot.id !== currentSpot) &&
                    <RobberSpot key={spot.id}
                                    left={spot.left + MapConstants.HEX_WIDTH*3/5}
                                    top={spot.top + MapConstants.HEX_HEIGHT*3/5}
                                    index={spot.id}
                                    onClick={() => onRobberSpotClick(spot.id)}
                    />
                )
            )}
        </div>
    );
}

export {RobberSpots}