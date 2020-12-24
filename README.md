# Hardspace Shipbreaker Furnace Performance Improvement Mod
During furnace processing it calls `VaporizeEntity`. This mod changes that to `DestroyPart` instead.<br>
The end result is the furnace should have relatively the same impact as the processor does. It won't create the many particles during vaporization and you can drop a whole Mackerel in there without issue.

# Requirements
- Hardspace Shipbreaker obviously.
- BepInEx: https://github.com/BepInEx/BepInEx (You want the x64 version.)

# Installation
- Extract BepInEx to the root Hardspace Shipbreaker folder so `winhttp.dll` is in the same folder as `Shipbreaker.exe`.
- Extract this mod so `FurnacePerformanceImprovements.dll` is placed like this: `Hardspace Shipbreaker\BepInEx\plugins\FurnacePerformanceImprovements.dll`.

# Un-installation
BepInEx uses `winhttp.dll` as an injector/loader. Renaming or deleting this file is enough to disable both my mod and the loader.<br>
Or just remove `FurnacePerformanceImprovements.dll/pdb` from the plugins folder.