export default class SpaceObject {
  spaceObjectID: number
  solarSystemID: number
  objectName: string
  objectType: string
  xCoord: number
  yCoord: number
  objectSize: number
  objectColor: string

  constructor(
    spaceObjectID: number = 0,
    solarSystemID: number = 0,
    objectName: string = '',
    objectType: string = '',
    xCoord: number = 0,
    yCoord: number = 0,
    objectSize: number = 0,
    objectColor: string = ''
  ) {
    this.spaceObjectID = spaceObjectID
    this.solarSystemID = solarSystemID
    this.objectName = objectName
    this.objectType = objectType
    this.xCoord = xCoord
    this.yCoord = yCoord
    this.objectSize = objectSize
    this.objectColor = objectColor
  }
}
