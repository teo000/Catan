import {PlayerDto} from "../../../../interfaces/PlayerDto";
import {usePlayer} from "../../../../context/PlayerProvider";

interface PlayerProps {
    playerInfo: PlayerDto
}

const AVATAR_PATH = "/images/avatars/"

export const PlayerInfo: React.FC<PlayerProps> = ({playerInfo}) =>{
    const {player} = usePlayer();

    return (
        <div className="player">
            <div className="avatar-name">
                <img className = "avatar" src={AVATAR_PATH + playerInfo.color.toLowerCase() + ".png"} alt="avatar" />
                <div className="player-name"> {playerInfo.name} {player?.id === playerInfo.id && "(you)"} </div>
            </div>
            <div className="player-info"> VP: {playerInfo.winningPoints} </div>
            <div className="player-info"> cards: 7 </div>
            {/*<h6> </h6>*/}
        </div>
    )
}