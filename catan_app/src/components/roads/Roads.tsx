import {RoadSpotInfo} from "./ComputeRoadSpotsInfo";
import {RoadSpot} from "./RoadSpot";
import {Road} from "./Road";

interface RoadsProps {
    roadSpotInfo: RoadSpotInfo[];
    roadIds: number[];
}

export const Roads: React.FC<RoadsProps> = ({roadSpotInfo, roadIds }) => {

    return (
        <div className="road-spots">
            {roadSpotInfo.map(road => (
                roadIds.includes(road.id) && (
                    <Road key={road.id}
                          index={road.id}
                          left={road.left}
                          top={road.top}
                          rotation={road.rotation}
                    />
                )
            ))}
        </div>
    );

}

