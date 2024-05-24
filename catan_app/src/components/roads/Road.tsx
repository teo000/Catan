
interface RoadProps {
    index: number,
    top: number,
    left: number,
    rotation: number,
}


export function Road({index, top, left, rotation, } : RoadProps){

    return (
        <div
            className="road"
            style={{
                transform: `rotate(${rotation}deg)`,
                top: `${top-18}px` ,
                left: `${left}px`
            }}
        />
    );
}