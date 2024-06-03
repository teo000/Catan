import {RoadSpot} from "./RoadSpot";
import {RoadSpotInfo} from "./ComputeRoadSpotsInfo";
import {MapInfo} from "../../utils/MapInfo";

interface RoadSpotsProps {
    visibleRoadSpots: number[];
    onRoadClick: (id: number) => void;
}

const RoadSpots: React.FC<RoadSpotsProps> = ({visibleRoadSpots, onRoadClick }) => {

    const roadSpotInfo = MapInfo.RoadSpotInfo;

    return (
        <div className="road-spots">
            {roadSpotInfo.map(road => (
                visibleRoadSpots.includes(road.id) && (
                    <RoadSpot key={road.id}
                              index={road.id}
                              left={road.left}
                              top={road.top}
                              rotation={road.rotation}
                              onClick={() => onRoadClick(road.id)}
                    />
                )
            ))}
        </div>
    );

}

export {RoadSpots};