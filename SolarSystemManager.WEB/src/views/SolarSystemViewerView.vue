<template>
  <div></div>
</template>



<script setup lang="js">

import SolarSystemService from '@/services/SolarSystemService'
import { ref } from 'vue'


import * as PIXI from 'pixi.js';
//import { json } from 'stream/consumers';

//build the pixi app
const app = new PIXI.Application({
    background: '#071129',
    resizeTo: window,
});

document.body.appendChild(app.view);

// set origin to the center of the screen
const container = new PIXI.Container();
container.x = app.screen.width / 2;
container.y = app.screen.height / 2;
app.stage.addChild(container);


// create a new Sprite from an image path

 
//const sun = PIXI.Sprite.from('https://pixijs.com/assets/bunny.png');
const sun = PIXI.Sprite.from('/src/assets/Images/V_E/sun.png');

// center the sprite's anchor point
sun.anchor.set(0.5);
sun.scale.set(2,2);


// move the sprite to the center of the screen
sun.x = app.screen.width / 2;
sun.y = app.screen.height / 2;

// app.stage.addChild(sun);
container.addChild(sun);

// Listen for animate update
app.ticker.add((delta) =>
{
    // just for fun, let's rotate mr rabbit a little
    // delta is 1 if running at 100% performance
    // creates frame-independent transformation
    sun.rotation += .01 * delta;
});



// Get the json data for the solar system based on the id
const solarObjects = SolarSystemService.GetSpaceObjects()

//loop through the json data and plot the objects on the screen
solarObjects.then((response) => {

  solarObjects.value = JSON.parse(response)
  console.log('hit')
  console.log(solarObjects.value)
  for(let i = 0; i < solarObjects.value.length; i++)
  {
    console.log('hit ' + i)
    console.log(solarObjects.value[i].xCoord)
    console.log(solarObjects.value[i].yCoord)
    console.log(solarObjects.value[i].objectSize)
    console.log(solarObjects.value[i].objectColor)

    const sObj = PIXI.Sprite.from('/src/assets/Images/V_E/sun.png');
    sObj.anchor.set(0.5);
    sObj.scale.set(solarObjects.value[i].objectSize,solarObjects.value[i].objectSize);
    sObj.x = solarObjects.value[i].xCoord;
    sObj.y = solarObjects.value[i].yCoord;
    sObj.blendMode = PIXI.BLEND_MODES.ADD()
    // app.stage.addChild(sObj);
    container.addChild(sObj);


  }
  
})


</script>

