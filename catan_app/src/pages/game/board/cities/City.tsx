interface CityProps {
    left: number,
    top: number,
    index: number,
    color: string
}

export function City({left, top, index, color} : CityProps){
    return (
        <div className='city'
             style={{
                 left: `${left}px`,
                 top: `${top}px`,
                 backgroundColor: color
             }}>
        </div>
    )
}