<template>
  <HeaderBar require-login></HeaderBar>
  <div>Editor</div>
  <div>Welcome to {{ currentSolarSystem.data.systemName }}</div>
  <div class="flex-column flex w-9">
    <Card class="mb-4">
      <template #title>Add an object to {{ currentSolarSystem.data.systemName }}</template>
      <template #content>
        <div>
          <label for="objectName">Object Name: </label>
          <InputText
            variant="filled"
            id="objectName"
            v-model="objectName"
            style="width: 245px"
          /><br />
          <label for="type">Object type: </label>
          <Dropdown
            id="type"
            v-model="objectType"
            :options="types"
            placeholder="Select object type"
          ></Dropdown
          ><br />
          <label for="xCoord">X coordinate: {{ objectXCoord }}</label>
          <div class="w-14rem">
            <Slider id="xCoord" v-model="objectXCoord" class="w-full"></Slider>
          </div>
          <br />
          <label for="yCoord">Y coordinate: {{ objectYCoord }}</label>
          <div class="w-14rem">
            <Slider id="yCoord" v-model="objectYCoord" class="w-full"></Slider>
          </div>
          <br />
          <label for="size">Size: {{ objectSize }}</label>
          <div class="w-14rem">
            <Slider id="size" v-model="objectSize" class="w-full"></Slider>
          </div>
          <br />
          <ColorPicker v-model="objectColor" />
          {{ objectColor }}
        </div>
        <Button @click="AddSpaceObject">Add Object</Button>
      </template>
    </Card>
  </div>

  <Button @click="toggle">Create</Button>
  <OverlayPanel ref="op">
    <div class="p-2">
      <h3>Add an object to {{ currentSolarSystem.data.systemName }}</h3>
      <div class="flex flex-column row-gap-2">
        <label for="objectName">Name</label>
        <InputText variant="filled" id="objectName" v-model="objectName" style="width: 245px" />

        <label for="type">Type</label>
        <Dropdown
          id="type"
          v-model="objectType"
          :options="types"
          placeholder="Select..."
        ></Dropdown>

        <div class="flex flex-column w-14rem row-gap-4">
          <label for="xCoord">X coordinate: {{ objectXCoord }}</label>
          <Slider
            id="xCoord"
            v-model="objectXCoord"
            class="w-14rem"
          ></Slider>

          <label for="yCoord">Y coordinate: {{ objectYCoord }}</label>
          <Slider id="yCoord" v-model="objectYCoord" class="w-14rem"></Slider>

          <label for="size">Size: {{ objectSize }}</label>
          <Slider id="size" v-model="objectSize" class="w-14rem"></Slider>

          <div>
            <label for="color"> Color </label>
            <ColorPicker id="color" v-model="objectColor" />
          </div>
        </div>
        <Button label="Save" icon="pi pi-check" @click="AddSpaceObject" />
      </div>
    </div>
  </OverlayPanel>

  <div class="flex-column flex w-9">
    <Card class="mb-4">
      <template #title>Remove an object from {{ currentSolarSystem.data.systemName }}</template>
      <template #content>
        <div>
          <label for="removeID">Object ID: </label>
          <InputText
            type="number"
            variant="filled"
            id="removeID"
            v-model="deleteSpaceObject"
            style="width: 245px"
          />
        </div>
        <Button @click="RemoveSpaceObject">Remove object</Button>
      </template>
    </Card>
  </div>

  <div class="flex-column flex w-9">
    <Card class="mb-4">
      <template #title>Objects in this solar system</template>
      <template #content>
        <DataTable :value="currentSolarSystem.data.spaceObjects">
          <Column field="spaceObjectID" header="Database ID"></Column
          ><!--For testing purposes only-->
          <Column field="objectName" header="Name"></Column>
          <Column field="objectType" header="Type"></Column>
          <Column field="xCoord" header="X Coordinte"></Column>
          <Column field="yCoord" header="Y Coordinate"></Column>
          <Column field="objectSize" header="Size"></Column>
          <Column field="objectColor" header="Color"></Column>
        </DataTable>
      </template>
    </Card>
  </div>
</template>

<script setup lang="ts">
import HeaderBar from '@/components/Header/HeaderBar.vue'
import SolarSystemService from '@/services/SolarSystemService'
import { ref } from 'vue'
import Card from 'primevue/card'
import Button from 'primevue/button'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import InputText from 'primevue/inputtext'
import Slider from 'primevue/slider'
import Dropdown from 'primevue/dropdown'
import SpaceObject from '@/Entities/SpaceObject'
import ColorPicker from 'primevue/colorpicker'
import OverlayPanel from 'primevue/overlaypanel'

const currentSolarSystem = ref<any>([])
const deleteSpaceObject = ref('')

const objectSize = ref<number>(0)
const objectName = ref('')
const objectType = ref('')
const types = ref(['Planet', 'Star'])
const objectXCoord = ref<number>(0)
const objectYCoord = ref<number>(0)
const objectColor = ref('ff0000')

const op = ref()

SolarSystemService.GetSolarSystemByID(19).then((response) => {
  currentSolarSystem.value = response
})

function AddSpaceObject() {
  console.log("Hit 0")
  SolarSystemService.AddSpaceObject(
    new SpaceObject(
      0,
      19,
      objectName.value,
      objectType.value,
      objectXCoord.value,
      objectYCoord.value,
      objectSize.value,
      objectColor.value
    )
  )
}

function RemoveSpaceObject() {
  SolarSystemService.RemoveSpaceObject(parseInt(deleteSpaceObject.value))
}

const toggle = (event: any) => {
  op.value.toggle(event)
}
</script>
