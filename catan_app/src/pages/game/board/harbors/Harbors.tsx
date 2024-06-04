import {HarborDto} from "../../../../interfaces/HarborDto";
import "./harbor.css"
import {Harbor} from "./Harbor";

const HarborPositionInfo = {
    1: {top: -35, left: 140, rotation: 150},
    2: {top: -35}
}

export function Harbors({harbors} : {harbors : HarborDto[]}){
    return (<>
        <Harbor top={-55} left={130} rotation={-30} type="Ore" />
        <Harbor top={-60} left={395} rotation={20} type="wheat"/>
        <Harbor top={70} left={615} rotation={30} type="general"/>
        <Harbor top={305} left={740} rotation={90} type="wheat"/>
        <Harbor top={540} left={615} rotation={145} type="general"/>
        <Harbor top={660} left={390} rotation={180} type="general"/>

    </>)
}