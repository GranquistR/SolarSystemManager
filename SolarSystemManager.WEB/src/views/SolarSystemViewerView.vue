<template>
  <div>
    <HeaderBar require-login no-docking />
    <div id="viewer"></div>
  </div>
</template>
<script setup lang="js">
import { ref, onMounted } from 'vue'
import * as PIXI from 'pixi.js'
import SolarSystemService from '@/services/SolarSystemService'
import HeaderBar from '@/components/Header/HeaderBar.vue'
import { useRoute } from 'vue-router'

const route = useRoute()
const systemId = route.params.id

//build the pixi app
const app = new PIXI.Application({
  background: '#071129',
  resizeTo: window
})

//mounts pixie to vue template
onMounted(() => {
  document.getElementById('viewer').appendChild(app.view)
})

// set origin to the center of the screen
const container = new PIXI.Container()
container.x = 1000
container.y = 1000
app.stage.addChild(container)

const solarSystem = ref(null)
// Get the json data for the solar system based on the id
SolarSystemService.GetSolarSystemByID(systemId).then((response) => {
  solarSystem.value = response
  solarSystem.value.spaceObjects.forEach((element) => {
    console.log(element)

    if (element.objectType == 'Star') {
      //const sprite = PIXI.Sprite.from('/src/assets/Images/V_E/sun.png');
    } else if (element.objectType == 'Planet') {
      //const sprite = PIXI.Sprite.from('/src/assets/Images/V_E/planet.png');
    }

    const sprite = PIXI.Sprite.from('/src/assets/Images/V_E/sun.png')
    sprite.anchor.set(0.5)
    sprite.scale.set(element.objectSize * 0.4, element.objectSize * 0.4)
    sprite.x = element.xCoord
    sprite.y = element.yCoord
    sprite.tint = element.objectColor
    //sObj.blendMode = PIXI.BLEND_MODES.ADD()
    // app.stage.addChild(sObj);
    container.addChild(sprite)
  })
})

const center = PIXI.Sprite.from('/src/assets/Images/V_E/sun.png')
center.anchor.set(0.5)
center.scale.set(0.5, 0.5)
center.x = 0
center.y = 0
app.stage.addChild(center)
const end = PIXI.Sprite.from('/src/assets/Images/V_E/sun.png')
end.anchor.set(0.5)
end.scale.set(0.5, 0.5)
end.x = 1000
end.y = 1000
app.stage.addChild(end)
</script>
