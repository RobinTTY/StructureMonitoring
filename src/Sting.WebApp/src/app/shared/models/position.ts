export class Position {
    buildingId: number;
    floorId?: number;
    roomId?: number;

    constructor(buildingId: number, floorId?: number, roomId?: number) {
        this.buildingId = buildingId;
        this.floorId = floorId;
        this.roomId = roomId;
    }
}
