<template>
  <CustomMessage ref="message"></CustomMessage>
  <Button
    :icon="spaceObjectToEdit ? 'pi pi-pencil' : 'pi pi-plus'"
    @click="openPanel"
    outlined
    rounded
    class="ml-2 mt-2"
    severity="contrast"
    :disabled="isDisabled"
  />
  <Dialog
    class="dialogNoBackground dialogNoX dialogNoPadding"
    v-model:visible="visible"
    :draggable="true"
    :dismissable="false"
    :position="'right'"
  >
    <template #header>
      <div class="p-3 addBox">
        <h3 v-if="!spaceObjectToEdit">Add an object to {{ solarSystem.systemName }}</h3>
        <h3 v-else>Edit {{ spaceObjectToEdit.objectName }}</h3>
        <div class="flex flex-column row-gap-2">
          <label for="objectName">Name:</label>
          <InputText variant="filled" id="objectName" v-model="newObject.objectName" />

          <label for="type">Type:</label>
          <div>
            <Dropdown
              id="type"
              v-model="objectMainType"
              :options="types"
              placeholder="Select..."
            ></Dropdown>
            <div>
              <div v-if="objectMainType == 'Planet'" class="flex flex-row flex-wrap gap-3 mt-2">
                <Dropdown
                  v-model="objectSubType"
                  :options="planetTypes"
                  placeholder="Planet type"
                  class="flex align-items-center justify-content-center"
                ></Dropdown>
                <div class="flex align-items-center justify-content-center">
                  <label for="ringed" class="mr-2"> Ringed </label>
                  <Checkbox id="ringed" v-model="objectRinged" :binary="true" />
                </div>
              </div>
            </div>
            <div v-if="objectMainType == 'Asteroid'">
              <Dropdown
                v-model="objectSubType"
                :options="asteroidTypes"
                placeholder="Asteroid type"
                class="mt-2"
              ></Dropdown>
            </div>
          </div>
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
import Checkbox from 'primevue/checkbox'
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

//for constructing specific object types
const objectMainType = ref<string>('Star')
const objectSubType = ref<string>('Rocky')
const objectRinged = ref<boolean>(false)

const newObject = ref<SpaceObject>({
  spaceObjectID: 0,
  solarSystemID: -1, //solarSystem.value.systemId,
  objectName: 'Sol',
  objectType: objectMainType.value,
  xCoord: 0,
  yCoord: 0,
  objectSize: 5,
  objectColor: '#FFFFFF'
})

const types = ref(['Star', 'Neutron Star', 'Black Hole', 'Comet', 'Planet', 'Asteroid'])
const planetTypes = ref(['Water', 'Rocky', 'Gas', 'Icy', 'Lava', 'Crater', 'Earthlike'])
const asteroidTypes = ref(['Rocky', 'Icy', 'Lava', 'Crater'])

watch(objectMainType, () => {
  //constructs the object type
  if (objectMainType.value == 'Asteroid' || objectMainType.value == 'Planet') {
    //sets default subtype
    objectSubType.value = 'Rocky'
    objectRinged.value = false
    newObject.value.objectType = objectSubType.value + ' ' + objectMainType.value
  } else {
    newObject.value.objectType = objectMainType.value
  }
})
watch(objectSubType, () => {
  //watching for changes in subtype
  if (objectRinged.value) {
    newObject.value.objectType = objectSubType.value + ' Ringed ' + objectMainType.value
  } else {
    newObject.value.objectType = objectSubType.value + ' ' + objectMainType.value
  }
})
watch(objectRinged, () => {
  if (objectRinged.value) {
    //add rings
    newObject.value.objectType = objectSubType.value + ' Ringed ' + objectMainType.value
  } else {
    //reset if not
    newObject.value.objectType = objectSubType.value + ' ' + objectMainType.value
  }
})

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
  resetObject()
  //returns to proper solar system
  props.graphics.DrawSolarSystem(solarSystem.value)
  //unlocks all edit/add buttons
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
          if (!data.success) {
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
  //removes the old space object if editing
  if (props.spaceObjectToEdit) {
    solarSystem.value.spaceObjects.splice(
      solarSystem.value.spaceObjects.indexOf(props.spaceObjectToEdit),
      1
    )
  }
  //shows the success message
  message.value.ShowMessage('Successfully Saved!', 'success')
  //hides the add object panel
  closePanel()

  //shows the success message
  message.value.ShowMessage('Successfully Saved!', 'success')
}

watch(newObject.value, () => {
  if (panelOpen.value) {
    redrawWithFake()
  }
})

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
