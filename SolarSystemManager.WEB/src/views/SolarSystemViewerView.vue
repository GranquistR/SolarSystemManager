<!-- Implement SolarView.js here if needed -->


<!-- RESEARCH TESTING DELETE LATER -->
<!-- <template>
    <Application :width="640" :height="480">
    <container>
        <text text="Hello" :style="style" />
    <text text="World" :style="style" y="60" />
    </container>
  </Application>
</template>

<script lang="ts" setup>
import { ref } from 'vue'
import { Application } from "vue3-pixi";
//import textureUrl from "@/assets/myTexture.png";

//const hitArea = new Rectangle(0, 0, 64, 64);

import { reactive } from 'vue'
import { onTick } from 'vue3-pixi'

const style = { fill: 'white' }
let direction = -1

const position = reactive({ x: 120, y: 80 })

onTick((delta) => {
  if (position.x <= 0)
    direction = 10
  else if (position.x >= 180)
    direction = -10
  position.x += 2 * direction * delta
})
</script> -->
<!-- RESEARCH TESTING END -->

