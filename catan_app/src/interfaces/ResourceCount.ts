export interface ResourceCount {
    Brick: number;
    Sheep: number;
    Ore: number;
    Wheat: number;
    Wood: number;
}

export const getEmptyResourceCount = (): ResourceCount => ({
    Brick: 0,
    Sheep: 0,
    Ore: 0,
    Wheat: 0,
    Wood: 0,
});