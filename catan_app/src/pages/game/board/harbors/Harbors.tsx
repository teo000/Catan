import {HarborDto} from "../../../../interfaces/HarborDto";
import "./harbor.css"
import {Harbor} from "./Harbor";

interface HarborInfo{
    top: number,
    left: number,
    rotation: number
}

const HarborPositionInfo : HarborInfo[]  = [
    { top: -55, left: 130, rotation: -30},
    { top: -60, left: 395, rotation: 20},
    { top: 70, left: 615, rotation: 30},
    { top: 305, left: 740, rotation: 90},
    { top: 540, left: 615, rotation: 145},
    { top: 660, left: 390, rotation: 180},
    { top: 660, left: 120, rotation: -150},
    { top: 425, left: -15, rotation: -90},
    { top: 175, left: -10, rotation: -90}
];

export function Harbors({ harbors }: { harbors: HarborDto[] }) {
    return (
        <div>
            {Object.entries(HarborPositionInfo).map(([position, { top, left, rotation }]) => {
                const harbor = harbors.find(harbor => harbor.position === parseInt(position));
                const resource = harbor ? harbor.resource : 'general';

                return (
                    <Harbor
                        key={position}
                        top={top}
                        left={left}
                        rotation={rotation}
                        type={resource}
                    />
                );

            })}
        </div>
    );
}