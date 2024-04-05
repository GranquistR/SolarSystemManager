<template>
  <Message class="alertMessage" :severity="severity" :closable="false" v-if="message != ''">{{
    message
  }}</Message>
  <Button label="Expand this Solar System" @click="open" class="w-22rem" />
  <OverlayPanel ref="op" :dismissable="false">
    <div class="p-2">
      <h3>Add an object to {{ solarSystem.systemName }}</h3>
      <div class="flex flex-column row-gap-2">
        <label for="objectName">Name</label>
        <InputText
          variant="filled"
          id="objectName"
          v-model="newObject.objectName"
          style="width: 245px"
        />

        <label for="type">Type</label>
        <Dropdown
          id="type"
          v-model="newObject.objectType"
          :options="types"
          placeholder="Select..."
        ></Dropdown>

        <div class="flex flex-column w-14rem row-gap-4">
          <div class="mt-3" v-tooltip.top="'Click where you want your object to be.'">
            Position: ({{ newObject.xCoord }},{{ newObject.yCoord }})
          </div>

          <label for="size">Size: {{ newObject.objectSize }}</label>
          <Slider
            id="size"
            v-model="newObject.objectSize"
            class="w-14rem"
            :min="1"
            :max="100"
          ></Slider>

          <div>
            <label for="color"> Color </label>
            <ColorPicker id="color" v-model="newObject.objectColor" />
          </div>
        </div>
        <Button label="Save" icon="pi pi-check" @click="AddSpaceObject" />
        <Button label="Cancel" icon="pi pi-times" severity="danger" @click="closePanel" />
      </div>
    </div>
  </OverlayPanel>
</template>
<script setup lang="ts">
import { ref, computed, watch } from 'vue'
import SpaceObject from '@/Entities/SpaceObject'
import ColorPicker from 'primevue/colorpicker'
import OverlayPanel from 'primevue/overlaypanel'
import InputText from 'primevue/inputtext'
import Slider from 'primevue/slider'
import Dropdown from 'primevue/dropdown'
import SolarSystemService from '@/services/SolarSystemService'
import Button from 'primevue/button'
import Graphics from '@/scripts/pixie/DrawSolarSystem'
import Message from 'primevue/message'

const props = defineProps<{
  system: any
  graphics: Graphics
}>()

defineExpose({ selectPosition })

const solarSystem = computed(() => {
  return props.system
})

const op = ref()

const message = ref('')
const severity = ref('')
const panelOpen = ref(false)

const newObject = ref<SpaceObject>({
  spaceObjectID: 0,
  solarSystemID: -1, //solarSystem.value.systemId,
  objectName: 'Sol',
  objectType: 'Star',
  xCoord: 0,
  yCoord: 0,
  objectSize: 5,
  objectColor: '#FFFFFF'
})

const types = ref([
  'Star',
  'Neutron Star',
  'Black Hole',
  'Comet',
  'Water Planet',
  'Rocky Planet',
  'Gas Planet',
  'Icy Planet',
  'Lava Planet',
  'Crater Planet',
  'Earthlike PLanet',
  'Water Ringed Planet',
  'Gas Ringed Planet',
  'Icy Ringed Planet',
  'Lava Ringed Planet',
  'Crater Ringed Planet',
  'Earthlike Ringed Planet',
  'Rocky Asteroid',
  'Icy Asteroid',
  'Lava Asteroid',
  'Creater Asteroid'
])

const open = (event: any) => {
  if (!panelOpen.value) {
    op.value.toggle(event)
    panelOpen.value = true
  }
  resetObject()
  redrawWithFake()
}

function closePanel() {
  //hides the panel
  op.value.hide()
  panelOpen.value = false
  //redraws the solar system without the fake object
  props.graphics.DrawSolarSystem(solarSystem.value)
  resetObject()
}

function AddSpaceObject() {
  if (!newObject.value) {
    return
  }
  //if new objects color does not start with #, add it
  if (!newObject.value.objectColor.startsWith('#')) {
    newObject.value.objectColor = '#' + newObject.value.objectColor
  }
  newObject.value.solarSystemID = solarSystem.value.systemId
  SolarSystemService.AddSpaceObject(newObject.value).then((data) => {
    if (data.success) {
      success()
    } else {
      failed()
    }
  })
}

function resetObject() {
  newObject.value.spaceObjectID = 0
  newObject.value.solarSystemID = -1
  newObject.value.objectName = 'Sol'
  newObject.value.objectType = 'Star'
  newObject.value.xCoord = 0
  newObject.value.yCoord = 0
  newObject.value.objectSize = 5
  newObject.value.objectColor = '#FFFFFF'
}

function failed() {
  alert('failed')
  severity.value = 'error'
  message.value = 'Failed to add!'
  setTimeout(() => {
    message.value = ''
    severity.value = ''
  }, 3000)
}

function success() {
  //adds the object to the solar system list
  const finalObject = JSON.parse(JSON.stringify(newObject.value))
  solarSystem.value.spaceObjects.push(finalObject)
  //redraws
  props.graphics.DrawSolarSystem(solarSystem.value)
  //hides the add object panel
  closePanel()

  //shows the success message
  message.value = 'Successfully saved!'
  severity.value = 'success'
  setTimeout(() => {
    message.value = ''
    severity.value = ''
  }, 3000)
}

watch(newObject.value, (newValue) => {
  if (panelOpen.value) {
    redrawWithFake()
  }
})

function redrawWithFake() {
  //deep clones to avoid reference issues
  let tempSolar = JSON.parse(JSON.stringify(solarSystem.value))
  let tempObj = JSON.parse(JSON.stringify(newObject.value))
  tempSolar.spaceObjects.push(tempObj)
  props.graphics.DrawSolarSystem(tempSolar)
}

function selectPosition(event: any) {
  newObject.value.xCoord = Math.round(event.x)
  newObject.value.yCoord = Math.round(event.y)
}
</script>

<style scoped>
.alertMessage {
  /* appear in top middle of page fixed */
  position: fixed;
  top: 4rem;
  left: 50%;
  transform: translate(-50%, 0);
  z-index: 1000;
}
</style>
