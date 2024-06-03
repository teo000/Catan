import {MapInfo} from "../../utils/MapInfo";
import "./robber.css"
import {MapConstants} from "../../utils/MapConstants";

export function Robber({hexTile} : {hexTile:number}){
    // const { data, error, loading, request } = useFetch<MapResponse>('/api/v1/Game');
    // const {player, gameId} = usePlayer();
    //
    // // async function onClick(){
    //     const requestData = {gameId, playerId: player?.id, position: hexTile};
    //
    //
    //     try {
    //         const response = await request('/thief', 'post', requestData);
    //         if (response === null || !response.success){
    //             console.error('Failed to move thief: Invalid response format', response);
    //         }
    //         console.log(response);
    //     } catch (err) {
    //         console.error('Failed to move thief', err);
    //     }
    // }

    return (
        <div className="robber"
             style={{
                 top: `${MapInfo.HexTileInfo[hexTile].top + MapConstants.HEX_HEIGHT*3/5}px`,
                 left: `${MapInfo.HexTileInfo[hexTile].left + MapConstants.HEX_WIDTH*3/5}px`,
             }}
        ></div>
    )
}