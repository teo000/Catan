import {TradeDto} from "../../../../interfaces/TradeDto";
import "./chat.css"
import {usePlayer} from "../../../../context/PlayerProvider";
import {PlayerDto} from "../../../../interfaces/PlayerDto";
import useFetch from "../../../../hooks/useFetch";
import {LobbyResponse} from "../../../../responses/LobbyResponse";

interface TradeListProps{
    trades: TradeDto[];
    players: PlayerDto[];
}

export const TradeList: React.FC<TradeListProps> = ({trades, players}) => {
    const { request } = useFetch<LobbyResponse>('/api/v1/Trade');


    const { player, gameId} = usePlayer();

    const getPlayerName = (playerId: string) => {
        const player = players.find(p => p.id === playerId);
        return player ? player.name : 'Unknown player';
    };

    const handleAccept = async (tradeId: string) => {
        console.log(`Accepted trade with ID: ${tradeId}`);
        const requestData = {gameId, playerId: player?.id, tradeId: tradeId};

        try {
            const response = await request('/accept', 'post', requestData);
            if (response === null || !response.success){
                console.error('Failed to accept trade: Invalid response format', response);
            }
            console.log(response);
        } catch (err) {
            console.error('Failed to accept trade', err);
        }

    };

    const handleReject = (tradeId: string) => {
        console.log(`Rejected trade with ID: ${tradeId}`);
        // reject
    };

    const tradesToMe = trades.filter(t => t.playerToReceiveId === player?.id);
    const pendingTradesToMe = tradesToMe.filter(t=> t.status === "Pending")

    {/*<div className="chat-div">*/}
    {/*    Trades will appear here...*/}
    {/*</div>*/}

    return (
        <div className="trade-list">
            {pendingTradesToMe.map(trade => (
                <div key={trade.id} className="trade-item">
                    <span>
                        Player {getPlayerName(trade.playerToGiveId)} wants to exchange {trade.countToGive}
                        {trade.resourceToGive} for {trade.countToReceive} {trade.resourceToReceive}
                    </span>
                    <button className="accept-button" onClick={() => handleAccept(trade.id)}>✔</button>
                    <button className="reject-button" onClick={() => handleReject(trade.id)}>✘</button>
                </div>
            ))}
        </div>
    );
};