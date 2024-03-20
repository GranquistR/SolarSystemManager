import * as PIXI from 'pixi.js'
import { Viewport } from 'pixi-viewport'

export function DrawSolarSystem(viewport: Viewport, solarSystem: any) {
  //for every object in the solar system
  solarSystem.value.spaceObjects.forEach((element: any) => {
    //sets the sprite
    const sprite = PIXI.Sprite.from('/src/assets/Images/sprites/sun.png')

    //sets the sprite properties
    sprite.anchor.set(0.5)
    sprite.scale.set(element.objectSize * 0.5, element.objectSize * 0.5)
    sprite.position.set(element.xCoord, element.yCoord)
    sprite.tint = element.objectColor
    //sObj.blendMode = PIXI.BLEND_MODES.ADD()

    //adds the sprite to the viewport
    viewport.addChild(sprite)
  })
}
