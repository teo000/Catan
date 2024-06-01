import {RoadSpot} from "./RoadSpot";
import {RoadSpotInfo} from "./ComputeRoadSpotsInfo";
import {SpotsInfo} from "../../utils/SpotsInfo";

interface RoadSpotsProps {
    visibleRoadSpots: number[];
    onRoadClick: (id: number) => void;
}

const RoadSpots: React.FC<RoadSpotsProps> = ({visibleRoadSpots, onRoadClick }) => {

    const roadSpotInfo = SpotsInfo.RoadSpotInfo;

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