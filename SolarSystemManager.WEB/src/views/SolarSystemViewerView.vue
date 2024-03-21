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
import { DrawSolarSystem } from '@/scripts/pixie/DrawSolarSystem'
import SpaceObjectPicker from '@/components/ViewerUi/SpaceObjectPicker.vue'

//vue stuff
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'

//pixi
import * as PIXI from 'pixi.js'
import { Viewport } from 'pixi-viewport'

//get the id from the route
const route = useRoute()
const systemId = Number(route.params.id)

//build the pixi app
const app = new PIXI.Application({
  background: '#071129',
  resizeTo: window
})

// builds viewport for panning and zooming and adds it to the app
const viewport = new Viewport({
  screenWidth: window.innerWidth,
  screenHeight: window.innerHeight,
  worldWidth: 1000,
  worldHeight: 1000,
  events: app.renderer.events // the interaction module is important for wheel to work properly when renderer.view is placed or scaled
})
//viewport settings
viewport.drag({}).decelerate({ friction: 0.95 }).pinch({}).wheel({})

//add the viewport to the app
app.stage.addChild(viewport)

//origin
const origin = PIXI.Sprite.from('/src/assets/Images/sprites/center.png')
origin.anchor.set(0.5)
origin.scale.set(0.3, 0.3)
origin.position.set(0, 0)
origin.tint = '0xFFFFFF'

viewport.addChild(origin)

const solarSystem = ref<any>()
onMounted(() => {
  //mounts the pixi app
  document.getElementById('viewer')?.appendChild(app.view as any)
  // Gets and draws the solar system
  SolarSystemService.GetSolarSystemByID(systemId).then((response) => {
    solarSystem.value = response
    DrawSolarSystem(viewport, solarSystem)
  })
})

function recenter() {
  viewport.fit()
  viewport.moveCenter(0, 0)
}
recenter()

function Select(id: number) {
  solarSystem.value.spaceObjects.forEach((spaceObject: any) => {
    if (spaceObject.spaceObjectID == id) {
      viewport.moveCenter(spaceObject.xCoord, spaceObject.yCoord)
    }
  })
}
</script>
