<template>
  <div>
    <!-- header -->
    <HeaderBar require-login no-docking />

    <!-- Ui -->
    <div class="absolute z-1 p-2">
      <div class="spacer"></div>
      <SpaceObjectPicker class="w-22rem" :solar-system="solarSystem" @select-id="Select" />
      <Button label="Recenter" @click="recenter" />
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
import { ref, onMounted, watch } from 'vue'
import { useRoute } from 'vue-router'

//pixi
import { Application, Sprite } from 'pixi.js'
import { Viewport } from 'pixi-viewport'

//get the id from the route
const route = useRoute()
const systemId = Number(route.params.id)
const selectedObject = ref<any>()
const solarSystem = ref<any>()
onMounted(() => {
  //mounts the pixi app
  document.getElementById('viewer')?.appendChild(app.view as any)

  // Gets and draws the solar system
  SolarSystemService.GetSolarSystemByID(systemId).then((response) => {
    solarSystem.value = response
    graphics.DrawSolarSystem(solarSystem)
    console.log(solarSystem.value)
  })

  // resize the viewport when the window is resized
  window.addEventListener('resize', () => {
    viewport.resize(window.innerWidth, window.innerHeight)
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

function Select(selection: any) {
  selectedObject.value = selection
}

//updates the viewport when a new object is selected
watch(selectedObject, (newValue) => {
  if (newValue != null) {
    console.log(newValue.spaceObjectID)
    solarSystem.value.spaceObjects.forEach((spaceObject: any) => {
      if (spaceObject.spaceObjectID === selectedObject.value.spaceObjectID) {
        viewport.moveCenter(spaceObject.xCoord, spaceObject.yCoord)
      }
    })

    graphics.HighlightSpaceObject(newValue.spaceObjectID)
  } else {
    graphics.RemoveHighlight()
  }
})

//updates graphics after every change to solarSystems
watch(solarSystem.value, (newValue) => {
  if (newValue != null) {
    graphics.DrawSolarSystem(solarSystem)
  }
})

//recenters
function recenter() {
  viewport.fit()
  viewport.moveCenter(0, 0)
}
</script>
<style scoped>
.spacer {
  height: 66px;
}
</style>
