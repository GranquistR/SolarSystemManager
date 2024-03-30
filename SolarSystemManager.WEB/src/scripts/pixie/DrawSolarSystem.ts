import { Sprite } from 'pixi.js'
import { Viewport } from 'pixi-viewport'
import { GlowFilter, AdvancedBloomFilter, BulgePinchFilter } from 'pixi-filters'

export default class gGraphics {
  viewport: Viewport

  defaultFilters = [
    new AdvancedBloomFilter({
      threshold: 0,
      bloomScale: 0,
      brightness: 1.5,
      blur: 0,
      quality: 0
    })
  ]

  constructor(viewport: Viewport) {
    this.viewport = viewport
  }

  DrawSolarSystem(solarSystem: any) {
    this.ClearSolarSystem()
    //for every object in the solar system
    solarSystem.value.spaceObjects.forEach((element: any) => {
      //sets the sprite
      const sprite = Sprite.from('/src/assets/Images/sprites/sun2.png')
      //sets the sprite properties
      sprite.anchor.set(0.5)
      sprite.scale.set(element.objectSize * (1 / 512), element.objectSize * (1 / 512))
      sprite.position.set(element.xCoord, element.yCoord)
      sprite.tint = element.objectColor
      sprite.filters = this.defaultFilters

      // if (element.objectType == 'black hole') {
      // }

      //THIS WORKS
      //TYPESCRIPT IS MAD
      //THIS IS THE EASY WAY TO ADD CUSTOM PROPERTIES TO A PIXI OBJECT
      //used in highlight space object
      sprite['spaceObjectId'] = element.spaceObjectID
      this.viewport.addChild(sprite)
    })

    //origin
    const origin = Sprite.from('/src/assets/Images/sprites/center.png')
    origin.anchor.set(0.5)
    origin.scale.set(10 / 360, 10 / 360)
    origin.position.set(0, 0)
    origin.tint = '0xFFFFFF'
    this.viewport.addChild(origin)
  }

  private ClearSolarSystem() {
    for (let i = this.viewport.children.length - 1; i >= 0; i--) {
      this.viewport.removeChild(this.viewport.children[i])
    }
  }

  HighlightSpaceObject(id: number) {
    this.viewport.children.forEach((element: any) => {
      if (element.spaceObjectId == id) {
        element.filters = [
          new GlowFilter({
            distance: 5,
            outerStrength: 10,
            innerStrength: 0,
            color: 0xffffff,
            quality: 0.5,
            knockout: false
          }),
          ...this.defaultFilters
        ]
      } else {
        element.filters = this.defaultFilters
      }
    })
  }

  RemoveHighlight() {
    this.viewport.children.forEach((element: any) => {
      element.filters = this.defaultFilters
    })
  }
}
