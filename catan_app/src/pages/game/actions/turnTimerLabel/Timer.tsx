import React, {useEffect, useState} from "react";

interface TimerProps {
    time:string
}

export const Timer: React.FC<TimerProps> = ({ time }) => {
    const calculateTimeLeft = (endTime: Date) => {
        const timeDifference = endTime.getTime() - Date.now();
        const secondsDifference = Math.floor(timeDifference / 1000);

        const minutesLeft = Math.floor(secondsDifference / 60);
        const secondsLeft = secondsDifference - minutesLeft * 60;

        return {
            minutesLeft,
            secondsLeftString: secondsLeft > 9 ? secondsLeft.toString() : `0${secondsLeft}`,
        };
    };

    const [timeLeft, setTimeLeft] = useState(() => calculateTimeLeft(new Date(time)));

    useEffect(() => {
        const turnEndTime = new Date(time);
        setTimeLeft(calculateTimeLeft(turnEndTime));

        const interval = setInterval(() => {
            setTimeLeft(calculateTimeLeft(turnEndTime));
        }, 1000);

        return () => clearInterval(interval);
    }, [time]);

    return <label> {timeLeft.minutesLeft}:{timeLeft.secondsLeftString} </label>;
};