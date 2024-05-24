import {ComputeRoadSpotRowInfo} from "./ComputeRoadSpotRowInfo";
import {SettlementSpot} from "../settlements/SettlementSpot";
import {RoadSpot} from "./RoadSpot";

interface RoadSpotsProps {
    visibleRoads: number[];
    isRoad: boolean[];
    onRoadClick: (id: number) => void;
}

const RoadSpots: React.FC<RoadSpotsProps> = ({ visibleRoads, isRoad, onRoadClick }) => {

    let roads = [];
    let startingNumber = 0;

    for (let i = 1; i <= 11; i++){
        let row = ComputeRoadSpotRowInfo(i, startingNumber);
        startingNumber += row.length;
        roads.push(...row);
    }

    return (
        <div className="road-spots">
            {roads.map(road => (
                visibleRoads.includes(road.id) && (
                    <RoadSpot key={road.id}
                              index={road.id}
                              left={road.left}
                              top={road.top}
                              rotation={road.rotation}
                              isRoad={isRoad[road.id]}
                              onClick={() => onRoadClick(road.id)}
                    />
                )
            ))}
        </div>
    );

}

export {RoadSpots};