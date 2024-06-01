import {getPlayerColor, Player, PlayerColor} from "../../../../interfaces/Player";
import {SettlementSpotInfo} from "../settlements/ComputeSettlementSpotsInfo";
import {CityDto} from "../../../../interfaces/CityDto";
import "./city.css"
import {City} from "./City";
interface CitiesProps{
    settlementSpotInfo : SettlementSpotInfo[],
    cities : CityDto[],
    players: Player[]
}
export function Cities({settlementSpotInfo, cities, players} : CitiesProps){
    const cityIds = cities.map(city=> city.position);

    const playerColorDict: { [id: number]: PlayerColor } = cities.reduce((dict, city) => {
        let player = players.find(player => player.id === city.playerId);
        if (!player)
            throw new Error("All settlements could not be loaded.");
        dict[city.position] = getPlayerColor(player.color);
        return dict;
    }, {} as { [id: number]: PlayerColor });


    return (
        <div className="cities">
            {settlementSpotInfo.map(city => (
                cityIds.includes(city.id) && (
                    <City key={city.id}
                                left={city.left}
                                top={city.top}
                                index={city.id}
                                color={playerColorDict[city.id]}
                    />
                )
            ))}
        </div>
    );
}