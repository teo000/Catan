import { useState, useEffect, useRef, DependencyList, EffectCallback } from 'react';
import isEqual from 'lodash.isequal';

export const useDeepCompareEffect = (callback: EffectCallback, dependencies: DependencyList) => {
    const currentDependenciesRef = useRef<DependencyList>();

    if (!isEqual(currentDependenciesRef.current, dependencies)) {
        currentDependenciesRef.current = dependencies;
    }

    useEffect(callback, [currentDependenciesRef.current]);
};

export const useDeepCompareState = <T>(initialValue: T): [T, (newState: T) => void] => {
    const [state, setState] = useState<T>(initialValue);
    const prevStateRef = useRef<T>(initialValue);

    const setDeepCompareState = (newState: T) => {
        if (!isEqual(prevStateRef.current, newState)) {
            prevStateRef.current = newState;
            setState(newState);
        }
    };

    return [state, setDeepCompareState];
};