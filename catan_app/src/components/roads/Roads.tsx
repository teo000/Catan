import {RoadSpotInfo} from "./ComputeRoadSpotsInfo";
import {RoadSpot} from "./RoadSpot";
import {Road} from "./Road";
import {RoadDto} from "../../interfaces/RoadDto";
import {getPlayerColor, Player, PlayerColor} from "../../interfaces/Player";

interface RoadsProps {
    roadSpotInfo: RoadSpotInfo[];
    roads: RoadDto[];
    players: Player[]
}

export const Roads: React.FC<RoadsProps> = ({roadSpotInfo, roads, players }) => {

    const roadIds = roads.map( road => road.position);

    const playerColorDict: { [id: number]: PlayerColor } = roads.reduce((dict, road) => {
        let player = players.find(player => player.id === road.playerId);
        if (!player)
            throw new Error("All settlements could not be loaded.");
        dict[road.position] = getPlayerColor(player.color);
        return dict;
    }, {} as { [id: number]: PlayerColor });

    return (
        <div className="road-spots">
            {roadSpotInfo.map(road => (
                roadIds.includes(road.id) && (
                    <Road key={road.id}
                          index={road.id}
                          left={road.left}
                          top={road.top}
                          rotation={road.rotation}
                          color = {playerColorDict[road.id]}
                    />
                )
            ))}
        </div>
    );

}

