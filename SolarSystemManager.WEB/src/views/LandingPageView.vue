<template>
  <div>
    <HeaderBar no-docking></HeaderBar>
  </div>

  <!-- greeting -->
  <div class="bg-cover bg-center w-full image-background" style="height: 65rem">
    <div class="greeting">
      <h1>
        DISCOVER
        <br />
        ENDLESS
        <br />
        SPACE
      </h1>
      <div class="fading-seperator"></div>
      <p>
        "SpaceBox empowers users to delve into the wonders of the universe from the comfort of their
        own homes. We provide an interactive platform for you to explore, create, and manage your
        own solar systems."
        <!-- Users can add, modify, or remove celestial bodies, visualizing
          their changes in a dynamic 3D environment. With detailed information about each celestial
          body, including size, composition, and orbital characteristics, SpaceBox fosters a deeper
          understanding of our universe. Whether you're a budding astronomer or a seasoned space
          enthusiast, SpaceBox offers a unique, engaging, and educational experience. -->
      </p>
      <RouterLink to="/login">
        <Button
          class="mt-4 text-700 border-300"
          label="Get Started"
          outlined
          severity="secondary"
        ></Button>
      </RouterLink>
    </div>
  </div>

  <!-- statistics -->
  <div class="grid grid-nogutter text-center">
    <div class="col-4">
      <h1>69</h1>
      <p>Active Users</p>
    </div>
    <div class="col-4">
      <h1>69</h1>
      <p>Solar Systems</p>
    </div>
    <div class="col-4">
      <h1>69</h1>
      <p>Unique Planets</p>
    </div>
  </div>

  <!-- spacer -->
  <div style="margin-top: 20rem"></div>
  <div class="text-center">
    <h2>Take a look</h2>
    <p>See what we have to offer with these popular, user-made solar systems.</p>
    <Carousel
      :value="allSolarSystems"
      :numVisible="3"
      :numScroll="3"
      :responsiveOptions="responsiveOptions"
    >
      <template #item="slotProps">
        <div class="border-1 surface-border border-round m-2 p-3">
          <div>
            {{ slotProps.data.systemName }}
          </div>
          <img src="../assets/Images/among-us-twerk.gif" width="200px" />
        </div>
      </template>
    </Carousel>
  </div>
</template>

<script setup lang="ts">
import HeaderBar from '@/components/Header/HeaderBar.vue'
import SolarSystemService from '@/services/SolarSystemService'
import Button from 'primevue/button'
import { RouterLink } from 'vue-router'
import { ref } from 'vue'
import Carousel from 'primevue/carousel'

const allSolarSystems = ref<string>('')

SolarSystemService.GetPublicSolarSystems().then((response) => {
  allSolarSystems.value = response
})
const responsiveOptions = ref([
  {
    breakpoint: '1920px',
    numVisible: 4,
    numScroll: 4
  },
  {
    breakpoint: '1199px',
    numVisible: 3,
    numScroll: 3
  },
  {
    breakpoint: '767px',
    numVisible: 2,
    numScroll: 2
  },
  {
    breakpoint: '575px',
    numVisible: 1,
    numScroll: 1
  }
])
</script>

<style scoped>
/* Greeting css */
h1 {
  font-weight: bold;
  font-stretch: expanded;
  font-size: 600%;
  letter-spacing: 0.4rem;
}
h2 {
  font-weight: bold;
  font-stretch: expanded;
  font-size: 300%;
  letter-spacing: 0.4rem;
}
p {
  opacity: 75%;
  font-size: 1.1rem;
  line-height: 1.5;
  margin-top: 2rem;
}
.greeting {
  padding-top: 10rem;
  margin-left: 10%;
}
.greeting p {
  width: 34rem;
}
/* background image css */
.image-background {
  position: relative;
}
.image-background::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: url(../assets/Images/spaceman-looking-at-planet.jpg) center center / cover no-repeat;
  -webkit-mask-image: linear-gradient(to bottom, black 30%, transparent 85%);
  mask-image: linear-gradient(to bottom, black 30%, transparent 85%);

  opacity: 0.4;
  z-index: -1;
}

/* fading seperator css */
.fading-seperator {
  width: 40%;
  height: 2px;
  background: linear-gradient(to right, rgba(255, 255, 255, 0.5), rgba(255, 255, 255, 0));
  border-radius: 15px;
  margin-top: 1rem;
  margin-bottom: 1rem;
}
</style>
