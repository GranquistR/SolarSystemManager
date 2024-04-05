<template>
  <Button label="Expand this Solar System" @click="toggle" class="w-22rem" />
  <OverlayPanel ref="op">
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
          <label for="xCoord">X coordinate: {{ newObject.xCoord }}</label>
          <Slider id="xCoord" v-model="newObject.xCoord" class="w-14rem"></Slider>

          <label for="yCoord">Y coordinate: {{ newObject.yCoord }}</label>
          <Slider id="yCoord" v-model="newObject.yCoord" class="w-14rem"></Slider>

          <label for="size">Size: {{ newObject.objectSize }}</label>
          <Slider id="size" v-model="newObject.objectSize" class="w-14rem"></Slider>

          <div>
            <label for="color"> Color </label>
            <ColorPicker id="color" v-model="newObject.objectColor" />
          </div>
        </div>
        <Button label="Save" icon="pi pi-check" @click="AddSpaceObject" />
      </div>
    </div>
  </OverlayPanel>
</template>
<script setup lang="ts">
import { ref, computed } from 'vue'
import SpaceObject from '@/Entities/SpaceObject'
import ColorPicker from 'primevue/colorpicker'
import OverlayPanel from 'primevue/overlaypanel'
import InputText from 'primevue/inputtext'
import Slider from 'primevue/slider'
import Dropdown from 'primevue/dropdown'
import SolarSystemService from '@/services/SolarSystemService'
import Button from 'primevue/button'
import { isConstructorDeclaration } from 'typescript'

const props = defineProps<{
  system: any
}>()
console.log(props)

const solarSystem = computed(() => {
  return props.system
})
console.log(solarSystem)

const op = ref()
const newObject = ref<SpaceObject>({
  spaceObjectID: 0,
  solarSystemID: -1, //solarSystem.value.systemId,
  objectName: 'Earth',
  objectType: 'Planet',
  xCoord: 0,
  yCoord: 0,
  objectSize: 5,
  objectColor: '0E7E8D'
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

const toggle = (event: any) => {
  op.value.toggle(event)
  console.log(solarSystem.value)
}

function AddSpaceObject() {
  newObject.value.objectColor = '#' + newObject.value.objectColor
  newObject.value.solarSystemID = solarSystem.value.systemId
  console.log(newObject.value)
  SolarSystemService.AddSpaceObject(newObject.value).then((data) => {
    console.log(data)
  })
}
</script>
