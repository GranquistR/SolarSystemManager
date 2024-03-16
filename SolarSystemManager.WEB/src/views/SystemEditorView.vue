<template>
  <!--Formatting is ugly as sin but will fix next sprint-->
  <HeaderBar require-login></HeaderBar>
  <div>Editor</div>
  <div> Welcome to {{ currentSolarSystem.systemName }} </div>
  <div class="flex-column flex w-9">
    <Card class="mb-4">
      <template #title>Add an object to {{currentSolarSystem.systemName}}</template>
      <template #content>
        <div>
          <p1>Name: Earth</p1><br>
          <p1>Type: Planet</p1><br>
          <p1>X coordinate: 000</p1><br>
          <p1>Y coordinate: 000</p1><br>
          <label for="size">Size: </label>
          <InputText variant="filled" id="size" v-model="objectSize" style="width: 245px" />
          <br><p1>Color: #5DE2E7</p1>
        </div>
        <Button @click="AddSpaceObject">Add Object</Button>
      </template>
    </Card>
  </div>

  <div class="flex-column flex w-9">
    <Card class="mb-4">
      <template #title>Remove an object from {{currentSolarSystem.systemName}}</template>
      <template #content>
        <div>
          <label for="size">Object ID: </label>
          <InputText variant="filled" id="size" v-model="deleteSpaceObject" style="width: 245px" />
        </div>
        <Button @click="RemoveSpaceObject">Remove object</Button>
      </template>
    </Card>
  </div>

  <div class="flex-column flex w-9">
    <Card class="mb-4">
      <template #title>Objects in this solar system</template>
      <template #content>
        <DataTable :value="currentSolarSystem.spaceObjects">
          <Column field="spaceObjectID" header="Database ID"></Column><!--For testing purposes only-->
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

const currentSolarSystem = ref<any>([])
const objectSize = ref('')
const deleteSpaceObject = ref('')

SolarSystemService.GetSolarSystemByID(22).then((response) => {
  currentSolarSystem.value = response
})

function AddSpaceObject(){
  SolarSystemService.AddSpaceObject(parseInt(objectSize.value))
}

function RemoveSpaceObject(){
  SolarSystemService.RemoveSpaceObject(parseInt(deleteSpaceObject.value))
}
</script>