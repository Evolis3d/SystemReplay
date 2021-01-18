# SystemReplay

### Generic Recording/Replay Library for Unity3D games.

by Carlos Lecina

------

**SystemReplay** is a Recording & Replay library for **<u>Unity3D v.2018+ [LTS]</u>**.  It allows you to store and save in-game demos for your games (ala Doom/Quake style demos).

It's being designed to be both simple and modular enough so you can use it on different genres of games and platforms.

**NOTE**: Everything is still W.I.P.

### Features
- Record/Playback of multiple GameObjects in your Scene.
- Load/Save files in custom data binary file.
- Simple, easy to use API.


### Usage

1) Add the following line at the top of your script:

```namespace
using evolis3d.SystemReplay;
```

2) Use the API to subscribe your elements to SystemReplay notification events. Then add the GameObjects you want them to be recorded, specified by Tag or by name.
3) Assign a UI Button or an Input key to these functions for basic usage:

**Recording**
```Recoding
ReplayMode = ReplayModeEnum.Recording;
```

**Playback**
```Playback
ReplayMode = ReplayModeEnum.Playing;
```  

### Future Plans
- JSON support.
- Encoding demos into a Texture2D (kinda FlowMap).
- More functions to manage your demos.
- Specify time delay between saves.
