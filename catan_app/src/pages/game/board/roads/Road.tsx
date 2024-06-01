
interface RoadProps {
    index: number,
    top: number,
    left: number,
    rotation: number,
    color: string
}


export function Road({index, top, left, rotation, color} : RoadProps){
    return (
        <div
            className="road"
            style={{
                transform: `rotate(${rotation}deg)`,
                top: `${top-18}px` ,
                left: `${left}px`,
                backgroundColor: color
            }}
        />
    );
}