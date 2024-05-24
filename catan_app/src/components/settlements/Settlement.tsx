interface SettlementProps {
    left: number,
    top: number,
    index: number,
}

function Settlement({left, top, index} : SettlementProps){

    return (
        <div className='settlement'
             style={{
                 left: `${left}px`,
                 top: `${top}px`
             }}>
        </div>
    )
}

export {Settlement};