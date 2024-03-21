<template>
  <div>
    <!-- header -->
    <HeaderBar require-login no-docking />

    <!-- Ui -->
    <div class="us" style="position: fixed; z-index: 100; top: 67px">
      <Button label="Recenter" @click="recenter" />
      <SpaceObjectPicker :solar-system="solarSystem" @select-id="Select" />
    </div>

    <!-- PIXI APP -->
    <div id="viewer" style="position: fixed"></div>
  </div>
</template>
<script setup lang="ts">
//components
import Button from 'primevue/button'
import HeaderBar from '@/components/Header/HeaderBar.vue'
import SolarSystemService from '@/services/SolarSystemService'
import Graphics from '@/scripts/pixie/DrawSolarSystem'
import SpaceObjectPicker from '@/components/ViewerUi/SpaceObjectPicker.vue'

//vue stuff
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'

//pixi
import { Application, Sprite } from 'pixi.js'
import { Viewport } from 'pixi-viewport'

//get the id from the route
const route = useRoute()
const systemId = Number(route.params.id)
const selectedObject = ref<number>()
const solarSystem = ref<any>()
onMounted(() => {
  //mounts the pixi app
  document.getElementById('viewer')?.appendChild(app.view as any)
  // Gets and draws the solar system
  SolarSystemService.GetSolarSystemByID(systemId).then((response) => {
    solarSystem.value = response
    graphics.DrawSolarSystem(solarSystem)
  })
})

//build the pixi app
const app = new Application({
  background: '#071129',
  resizeTo: window
})

const backgroundSprite = Sprite.from('/src/assets/Images/stars.webp')
backgroundSprite.anchor.set(0.5)

app.stage.addChild(backgroundSprite)

// builds viewport for panning and zooming and adds it to the app
const viewport = new Viewport({
  screenWidth: window.innerWidth,
  screenHeight: window.innerHeight,
  worldWidth: 2000,
  worldHeight: 2000,
  events: app.renderer.events // the interaction module is important for wheel to work properly when renderer.view is placed or scaled
})
//viewport settings
viewport.drag({}).decelerate({ friction: 0.95 }).pinch({}).wheel({})
viewport.fit()
viewport.moveCenter(0, 0)

//add the viewport to the app
app.stage.addChild(viewport)
const graphics = new Graphics(viewport)

function Select(id: number) {
  solarSystem.value.spaceObjects.forEach((spaceObject: any) => {
    if (spaceObject.spaceObjectID == id) {
      selectedObject.value = id
      viewport.moveCenter(spaceObject.xCoord, spaceObject.yCoord)
    }
  })
  graphics.HighlightSpaceObject(id)
}

function recenter() {
  viewport.fit()
  viewport.moveCenter(0, 0)
}
</script>
