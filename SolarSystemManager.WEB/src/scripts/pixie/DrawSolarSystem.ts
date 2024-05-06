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
    solarSystem.spaceObjects.forEach((element: any) => {
      //sets the sprite
      let sprite = Sprite.from('/src/assets/Images/sprites/error.png')

      //checks the object type and changes the sprite accordingly
      //Planets
      if (element.objectType == 'Water Planet')
        sprite = Sprite.from('/src/assets/Images/sprites/waterPlanet.png')
      else if (element.objectType == 'Rocky Planet')
        sprite = Sprite.from('/src/assets/Images/sprites/rockyPlanet.png')
      else if (element.objectType == 'Gas Planet')
        sprite = Sprite.from('/src/assets/Images/sprites/gasPlanet.png')
      else if (element.objectType == 'Icy Planet')
        sprite = Sprite.from('/src/assets/Images/sprites/icyPlanet.png')
      else if (element.objectType == 'Lava Planet')
        sprite = Sprite.from('/src/assets/Images/sprites/lavaPlanet.png')
      else if (element.objectType == 'Crater Planet')
        sprite = Sprite.from('/src/assets/Images/sprites/craterPlanet.png')
      else if (element.objectType == 'Earthlike Planet')
        sprite = Sprite.from('/src/assets/Images/sprites/earthlikePlanet.png')
      else if (element.objectType == 'Water Ringed Planet')
        sprite = Sprite.from('/src/assets/Images/sprites/waterRingedPlanet.png')
      else if (element.objectType == 'Rocky Ringed Planet')
        sprite = Sprite.from('/src/assets/Images/sprites/rockyRingedPlanet.png')
      else if (element.objectType == 'Gas Ringed Planet')
        sprite = Sprite.from('/src/assets/Images/sprites/gasRingedPlanet.png')
      else if (element.objectType == 'Icy Ringed Planet')
        sprite = Sprite.from('/src/assets/Images/sprites/icyRingedPlanet.png')
      else if (element.objectType == 'Lava Ringed Planet')
        sprite = Sprite.from('/src/assets/Images/sprites/lavaRingedPlanet.png')
      else if (element.objectType == 'Crater Ringed Planet')
        sprite = Sprite.from('/src/assets/Images/sprites/craterRingedPlanet.png')
      else if (element.objectType == 'Earthlike Ringed Planet')
        sprite = Sprite.from('/src/assets/Images/sprites/earthlikeRingedPlanet.png')
      //Space Objects
      else if (element.objectType == 'Rocky Asteroid')
        sprite = Sprite.from('/src/assets/Images/sprites/rockyAsteroid.png')
      else if (element.objectType == 'Icy Asteroid')
        sprite = Sprite.from('/src/assets/Images/sprites/icyAsteroid.png')
      else if (element.objectType == 'Lava Asteroid')
        sprite = Sprite.from('/src/assets/Images/sprites/lavaAsteroid.png')
      else if (element.objectType == 'Crater Asteroid')
        sprite = Sprite.from('/src/assets/Images/sprites/craterAsteroid.png')
      else if (element.objectType == 'Star')
        sprite = Sprite.from('/src/assets/Images/sprites/star.png')
      else if (element.objectType == 'Neutron Star')
        sprite = Sprite.from('/src/assets/Images/sprites/neutronStar.png')
      else if (element.objectType == 'Black Hole')
        sprite = Sprite.from('/src/assets/Images/sprites/blackHole.png')
      else if (element.objectType == 'Comet')
        sprite = Sprite.from('/src/assets/Images/sprites/comet.png')
      else console.log('Error: Object type not found: ', element.objectType)

      //sets the sprite properties
      sprite.anchor.set(0.5)
      sprite.scale.set(element.objectSize * (1 / 1024), element.objectSize * (1 / 1024))
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
