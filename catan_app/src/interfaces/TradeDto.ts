export interface TradeDto{
    id: string;
    playerToGiveId: string;
    resourceToGive: string;
    countToGive: number;
    playerToReceiveId: string;
    resourceToReceive: string;
    countToReceive: number;
    status: string;
}