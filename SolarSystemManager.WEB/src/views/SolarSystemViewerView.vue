<template>
  <div>
    <!-- header -->
    <HeaderBar require-login no-docking />

    <!-- Ui left-->
    <div class="absolute z-1 p-2">
      <div class="spacer"></div>
      <SpaceObjectPicker
        ref="editSpaceObject"
        class="w-22rem"
        :solar-system="solarSystem"
        @select-id="Select"
        :graphics="graphics"
        :editing-disabled="isEditing"
        @opened="isEditing = true"
        @closed="isEditing = false"
        :is-authorized="isAuthorized"
      />
    </div>
    <!-- PIXI APP -->
    <div id="viewer" style="position: fixed"></div>

    <!-- Ui Right -->
    <div class="spacer"></div>
    <div class="p-2 flex justify-content-end flex-wrap">
      <div class="flex flex-column gap-2">
        <AddSpaceObject
          ref="addSpaceObject"
          v-if="isAuthorized"
          :system="solarSystem"
          :graphics="graphics"
          :disabled="isEditing"
          @opened="isEditing = true"
          @closed="isEditing = false"
        ></AddSpaceObject>
        <Button
          icon="pi pi-sun"
          @click="recenter"
          outlined
          rounded
          class="ml-2 mt-2 buttons"
          severity="contrast"
        />
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
//components
import Button from 'primevue/button'
import HeaderBar from '@/components/Header/HeaderBar.vue'
import SolarSystemService from '@/services/SolarSystemService'
import Graphics from '@/scripts/pixie/DrawSolarSystem'
import SpaceObjectPicker from '@/components/ViewerUi/SpaceObjectPicker.vue'
import AddSpaceObject from '@/components/ViewerUi/AddSpaceObject.vue'

//vue stuff
import { ref, onMounted, watch, inject } from 'vue'
import { useRoute } from 'vue-router'

//pixi
import { Application, Sprite } from 'pixi.js'
import { Viewport } from 'pixi-viewport'
import router from '@/router'
import type User from '@/Entities/User'

//get the id from the route
const route = useRoute()
const systemId = Number(route.params.id)
const selectedObject = ref<any>()
const solarSystem = ref<any>()
const editSpaceObject = ref()
const addSpaceObject = ref()
const isEditing = ref(false)
const user: User | undefined = inject('currentUser')
const isAuthorized = ref(false)

onMounted(() => {
  //mounts the pixi app
  document.getElementById('viewer')?.appendChild(app.view as any)

  // Gets and draws the solar system
  SolarSystemService.GetSolarSystemByID(systemId, user).then((response) => {
    if (response.success) {
      solarSystem.value = response.data
      graphics.DrawSolarSystem(solarSystem.value)
      if (user?.role == 1 || user?.userID == solarSystem.value.ownerId) {
        isAuthorized.value = true
      }
    } else {
      if (response.status === 401) {
        router.push('/unauthorized')
        return
      }
      if (response.status === 403) {
        router.push('/forbidden')
        return
      }
      router.push('/notfound')
    }
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
  worldWidth: 100,
  worldHeight: 100,
  events: app.renderer.events // the interaction module is important for wheel to work properly when renderer.view is placed or scaled
})
//viewport settings
viewport.drag({}).decelerate({ friction: 0.95 }).pinch({}).wheel({})
viewport.fit()
viewport.moveCenter(0, 0)

//viewport click position handling
viewport.on('clicked', function (e) {
  editSpaceObject.value.selectPosition(e.world)
  addSpaceObject.value.selectPosition(e.world)
})

//add the viewport to the app
app.stage.addChild(viewport)
const graphics = new Graphics(viewport)

function Select(selection: any) {
  selectedObject.value = selection
}

//updates the viewport when a new object is selected
watch(selectedObject, (newValue) => {
  if (newValue != null) {
    solarSystem.value.spaceObjects.forEach((spaceObject: any) => {
      if (spaceObject.spaceObjectID === selectedObject.value.spaceObjectID) {
        if (newValue.objectSize <= 40) {
          viewport.moveCenter(spaceObject.xCoord, spaceObject.yCoord)
          viewport.setZoom(30)
        } else if (newValue.objectSize > 40 || 50 > newValue.objectSize) {
          viewport.setZoom(10)
        } else {
          viewport.setZoom(8)
        }
        viewport.moveCenter(spaceObject.xCoord, spaceObject.yCoord)
      }
    })

    graphics.HighlightSpaceObject(newValue.spaceObjectID)
  } else {
    graphics.RemoveHighlight()
  }
})

//updates graphics after every change to solarSystems
watch(solarSystem, (newValue) => {
  if (newValue != null) {
    graphics.DrawSolarSystem(solarSystem.value)
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
