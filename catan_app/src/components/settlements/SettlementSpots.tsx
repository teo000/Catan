





//asta va primi in final lista cu settlements ca sa poata sa deseneze
//momentan o sa faca doar niste cerculete
import {SettlementSpotRow} from "./SettlementSpotRow";

function SettlementSpots(){
    return (
        <div className="spots">
            <SettlementSpotRow rowNumber={1}/>
            <SettlementSpotRow rowNumber={2}/>
            <SettlementSpotRow rowNumber={3}/>
            <SettlementSpotRow rowNumber={4}/>
            <SettlementSpotRow rowNumber={5}/>
            <SettlementSpotRow rowNumber={6}/>
            <SettlementSpotRow rowNumber={7}/>
            <SettlementSpotRow rowNumber={8}/>
            <SettlementSpotRow rowNumber={9}/>
            <SettlementSpotRow rowNumber={10}/>
            <SettlementSpotRow rowNumber={11}/>
            <SettlementSpotRow rowNumber={12}/>
        </div>
    )
}

export {SettlementSpots}