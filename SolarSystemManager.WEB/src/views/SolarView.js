import * as PIXI from 'pixi.js';

const app = new PIXI.Application({
    background: '#071129',
    resizeTo: window,
});

document.body.appendChild(app.view);

// create a new Sprite from an image path
 const sun = PIXI.Sprite.from("../assets/Images/V_E/sun.png");
//const sun = PIXI.Sprite.from('https://pixijs.com/assets/bunny.png');

// center the sprite's anchor point
sun.anchor.set(0.5);

// move the sprite to the center of the screen
sun.x = app.screen.width / 2;
sun.y = app.screen.height / 2;

app.stage.addChild(sun);

// Listen for animate update
app.ticker.add((delta) =>
{
    // just for fun, let's rotate mr rabbit a little
    // delta is 1 if running at 100% performance
    // creates frame-independent transformation
    sun.rotation += 0.1 * delta;
});