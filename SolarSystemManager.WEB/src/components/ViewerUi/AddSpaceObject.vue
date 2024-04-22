<template>
  <CustomMessage ref="message"></CustomMessage>
  <Button
    :icon="spaceObjectToEdit ? 'pi pi-pencil' : 'pi pi-plus'"
    @click="openPanel"
    outlined
    rounded
    class="tools"
    severity="secondary"
    :disabled="isDisabled"
  />
  <Dialog
    v-model:visible="visible"
    :dismissable="false"
    :position="'right'"
    :draggable="true"
    :pt="{
      root: 'border-none',
      background: 'none'
      //note to self: trying to get ride of dialog box background so that its actual transparent
    }"
  >
    <template #container>
      <div class="p-3 addBox">
        <h3 v-if="!spaceObjectToEdit">Add an object to {{ solarSystem.systemName }}</h3>
        <h3 v-else>Edit {{ spaceObjectToEdit.objectName }}</h3>
        <div class="flex flex-column row-gap-2">
          <label for="objectName">Name:</label>
          <InputText variant="filled" id="objectName" v-model="newObject.objectName" />

          <label for="type">Type:</label>
          <Dropdown
            id="type"
            v-model="newObject.objectType"
            :options="types"
            placeholder="Select..."
          ></Dropdown>
          <div class="flex flex-column row-gap-4">
            <div class="mt-3" v-tooltip.top="'Click where you want your object to be.'">
              Position: ({{ newObject.xCoord }},{{ newObject.yCoord }})
            </div>
            <div>
              Size:
              <InputNumber v-model="newObject.objectSize" inputId="minmax" :min="0" :max="100" />
            </div>
            <Slider v-model="newObject.objectSize" :min="1" :max="100"></Slider>
            <!--class="w-14rem"-->
            <div>
              <label for="color"> Color </label>
              <ColorPicker id="color" v-model="newObject.objectColor" />
            </div>
          </div>
          <Button label="Save" icon="pi pi-check" @click="AddSpaceObject" />
          <Button label="Cancel" icon="pi pi-times" severity="danger" @click="closePanel" />
        </div>
      </div>
    </template>
  </Dialog>
</template>
<script setup lang="ts">
import { ref, computed, watch, inject } from 'vue'
import SpaceObject from '@/Entities/SpaceObject'
import ColorPicker from 'primevue/colorpicker'
import InputText from 'primevue/inputtext'
import Slider from 'primevue/slider'
import Dropdown from 'primevue/dropdown'
import SolarSystemService from '@/services/SolarSystemService'
import Button from 'primevue/button'
import Graphics from '@/scripts/pixie/DrawSolarSystem'
import CustomMessage from '../CustomMessage.vue'
import InputNumber from 'primevue/inputnumber'
import Dialog from 'primevue/dialog'
import type User from '@/Entities/User'

//props
const props = defineProps<{
  system: any
  graphics: Graphics
  spaceObjectToEdit?: any
  disabled?: boolean
}>()

//refs
const user: User | undefined = inject('currentUser')
const visible = ref<boolean>(false)
const emit = defineEmits(['opened', 'closed'])
const message = ref()
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
  'Earthlike Planet',
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

//expose
defineExpose({ selectPosition })
function selectPosition(event: any) {
  if (panelOpen.value) {
    newObject.value.xCoord = Math.round(event.x)
    newObject.value.yCoord = Math.round(event.y)
  }
}

//computed
const solarSystem = computed(() => {
  return props.system
})
const isDisabled = computed(() => {
  return props.disabled
})

//watchers
watch(newObject.value, () => {
  if (panelOpen.value) {
    console.log(newObject.value)
    redrawWithFake()
  }
})

//methods
//opens the panel and begins the drawing of the edits
function openPanel(event: any) {
  if (!panelOpen.value) {
    //op.value.toggle(event)
    panelOpen.value = true
    visible.value = true
  }
  resetObject()
  redrawWithFake()
  emit('opened')
}

//resets everything on close
function closePanel() {
  //hides the panel
  panelOpen.value = false
  visible.value = false
  //redraws the solar system without the fake object
  props.graphics.DrawSolarSystem(solarSystem.value)
  resetObject()
  emit('closed')
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

  if (user) {
    if (props.spaceObjectToEdit) {
      SolarSystemService.RemoveSpaceObject(user, props.spaceObjectToEdit.spaceObjectID).then(
        (data) => {
          if (data.success) {
            // eslint-disable-next-line vue/no-mutating-props
            solarSystem.value.spaceObjects.splice(
              solarSystem.value.spaceObjects.indexOf(props.spaceObjectToEdit),
              1
            )
          } else {
            failed()
            return
          }
        }
      )
    }

    SolarSystemService.AddSpaceObject(newObject.value, user).then((result) => {
      if (result.success) {
        success(result.data)
      } else {
        failed()
      }
    })
  }
}

function resetObject() {
  if (props.spaceObjectToEdit) {
    newObject.value.spaceObjectID = props.spaceObjectToEdit.spaceObjectID
    newObject.value.solarSystemID = props.spaceObjectToEdit.solarSystemID
    newObject.value.objectName = props.spaceObjectToEdit.objectName
    newObject.value.objectType = props.spaceObjectToEdit.objectType
    newObject.value.xCoord = props.spaceObjectToEdit.xCoord
    newObject.value.yCoord = props.spaceObjectToEdit.yCoord
    newObject.value.objectSize = props.spaceObjectToEdit.objectSize
    newObject.value.objectColor = props.spaceObjectToEdit.objectColor
  } else {
    newObject.value.spaceObjectID = 0
    newObject.value.solarSystemID = -1
    newObject.value.objectName = 'Sol'
    newObject.value.objectType = 'Star'
    newObject.value.xCoord = 0
    newObject.value.yCoord = 0
    newObject.value.objectSize = 5
    newObject.value.objectColor = '#FFFFFF'
  }
}

function failed() {
  message.value.ShowMessage('Failed to add!', 'error')
}

function success(id: number) {
  newObject.value.spaceObjectID = id
  //adds the object to the solar system list
  const finalObject = JSON.parse(JSON.stringify(newObject.value))
  solarSystem.value.spaceObjects.push(finalObject)
  //redraws
  props.graphics.DrawSolarSystem(solarSystem.value)
  //hides the add object panel
  closePanel()

  //shows the success message
  message.value.ShowMessage('Successfully Saved!', 'success')
}

function redrawWithFake() {
  //deep clones to avoid reference issues
  let tempSolar = JSON.parse(JSON.stringify(solarSystem.value))
  if (props.spaceObjectToEdit) {
    tempSolar.spaceObjects.splice(
      solarSystem.value.spaceObjects.indexOf(props.spaceObjectToEdit),
      1
    )
  }
  let tempObj = JSON.parse(JSON.stringify(newObject.value))
  tempSolar.spaceObjects.push(tempObj)
  props.graphics.DrawSolarSystem(tempSolar)
}
</script>
<style scoped>
.addBox {
  backdrop-filter: blur(5px);
  background-color: rgba(0, 0, 0, 0.5);
  border: solid #27272a 1px;
  border-radius: 10px;
}
</style>
