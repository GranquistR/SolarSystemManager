export default class SpaceObject {

    spaceObjectID: number;
    solarSystemID: number;
    objectName: string;
    objectType: string;
    xCoord: number;
    yCoord: number;
    objectSize: number;
    objectColor: string;

    constructor(spaceObjectID: number, solarSystemID: number, objectName: string, objectType: string,
        xCoord: number, yCoord: number, objectSize: number, objectColor: string) {

        this.spaceObjectID = spaceObjectID;
        this.solarSystemID = solarSystemID;
        this.objectName = objectName;
        this.objectType = objectType;
        this.xCoord = xCoord;
        this.yCoord = yCoord;
        this.objectSize = objectSize;
        this.objectColor = objectColor;
    }
}