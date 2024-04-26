<template>
  <img
    :src="imageLink"
    :width="size"
    :height="size"
    :style="{
      filter:
        'opacity(0.7) drop-shadow(0 0 0px ' +
        spaceObject.objectColor +
        ') drop-shadow(0 0 0px ' +
        spaceObject.objectColor +
        ') contrast(1.3)'
    }"
  />
</template>
<script setup lang="ts">
import { computed } from 'vue'
// eslint-disable-next-line vue/return-in-computed-property
const imageLink = computed(() => {
  if (props.spaceObject.objectType == 'Water Planet')
    return '/src/assets/Images/sprites/waterPlanet.png'
  else if (props.spaceObject.objectType == 'Rocky Planet')
    return '/src/assets/Images/sprites/rockyPlanet.png'
  else if (props.spaceObject.objectType == 'Gas Planet')
    return '/src/assets/Images/sprites/gasPlanet.png'
  else if (props.spaceObject.objectType == 'Icy Planet')
    return '/src/assets/Images/sprites/icyPlanet.png'
  else if (props.spaceObject.objectType == 'Lava Planet')
    return '/src/assets/Images/sprites/lavaPlanet.png'
  else if (props.spaceObject.objectType == 'Crater Planet')
    return '/src/assets/Images/sprites/craterPlanet.png'
  else if (props.spaceObject.objectType == 'Earthlike Planet')
    return '/src/assets/Images/sprites/earthlikePlanet.png'
  else if (props.spaceObject.objectType == 'Water Ringed Planet')
    return '/src/assets/Images/sprites/waterRingedPlanet.png'
  else if (props.spaceObject.objectType == 'Rocky Ringed Planet')
    return '/src/assets/Images/sprites/rockyRingedPlanet.png'
  else if (props.spaceObject.objectType == 'Gas Ringed Planet')
    return '/src/assets/Images/sprites/gasRingedPlanet.png'
  else if (props.spaceObject.objectType == 'Icy Ringed Planet')
    return '/src/assets/Images/sprites/icyRingedPlanet.png'
  else if (props.spaceObject.objectType == 'Lava Ringed Planet')
    return '/src/assets/Images/sprites/lavaRingedPlanet.png'
  else if (props.spaceObject.objectType == 'Crater Ringed Planet')
    return '/src/assets/Images/sprites/craterRingedPlanet.png'
  else if (props.spaceObject.objectType == 'Earthlike Ringed Planet')
    return '/src/assets/Images/sprites/earthlikeRingedPlanet.png'
  //Space Objects
  else if (props.spaceObject.objectType == 'Rocky Asteroid')
    return '/src/assets/Images/sprites/rockyAsteroid.png'
  else if (props.spaceObject.objectType == 'Icy Asteroid')
    return '/src/assets/Images/sprites/icyAsteroid.png'
  else if (props.spaceObject.objectType == 'Lava Asteroid')
    return '/src/assets/Images/sprites/lavaAsteroid.png'
  else if (props.spaceObject.objectType == 'Crater Asteroid')
    return '/src/assets/Images/sprites/craterAsteroid.png'
  else if (props.spaceObject.objectType == 'Star') return '/src/assets/Images/sprites/star.png'
  else if (props.spaceObject.objectType == 'Neutron Star')
    return '/src/assets/Images/sprites/neutronStar.png'
  else if (props.spaceObject.objectType == 'Black Hole')
    return '/src/assets/Images/sprites/blackHole.png'
  else if (props.spaceObject.objectType == 'Comet') return '/src/assets/Images/sprites/comet.png'
  else return '/src/assets/Images/nope-not-here.webp'
})

const props = defineProps<{
  spaceObject: any
  size: number
}>()
</script>
<style scoped></style>
