**Note:** This was written to document the steps I took as I was piecing everything together, but I've found a much simple method using AssetRipper. This document will be updated in the near future and I don't recommend using it in the mean time. 

## Updating the original PowerWheel assets

I'm documenting these steps for the inevitable event that the PowerWheel assets are updated and the Shaming Wheel needs to re-import them. If you're here because you're new to modding and just as lost as I was at first, welcome! But know that this guide is pretty specific and will likely become out-of-date very quickly.

1. Grab the latest version of [AssetStudio](https://github.com/Perfare/AssetStudio/releases), run it, and click on **File > Load File**
   - The file to load is `Timberborn_Data\resources.assets` in your Timberborn install folder. If you're using Steam on Windows, this will likely be `C:\Program Files (x86)\Steam\steamapps\common\Timberborn`
   - **Heads up:** Even with 32 GB of RAM, it gave me a lot of memory errors (like... a *lot*) while opening the file. I dismissed all of these errors and it eventually finished with seemingly zero consequences. Your mileage may very, but it's worth a shot. I found that choosing **File > Extract File** didn't work at all.
2. Once the assets are loaded in AssetStudio, make sure you're in the **Scene Hierarchy** tab and select the following:
   - `PowerWheel.Folktails`
   - `PowerWheel.Folktails.Model`
   - `PowerWheel.IronTeeth`
   - `PowerWheel.IronTeeth.Model`
3. Now select `Model > Export selected objects (split) + selected AnimationClips`
   - You should end up with a folder called `GameObject` containing a handful of texture PNG files and two `.fbx` files for each faction.
4. Head over to the `Resources\buildings\power\powerwheel` folder in this directory and replace the files that have been changed. When you head back into Unity, they should re-import and everything _should_ be fine because the ShamingWheel prefab is a variant that applies overrides to the imported PowerWheel object.
5. For updated animations, controllers, model avatars, etc. head back over to AssetStudio and switch to the **Asset List** tab. Type "powerwheel" into the filter bar and select the following:
   - `PowerWheel.Folktails (type: AnimationClip)`
   - `PowerWheel.Folktails.Model (type: Animator)`
   - `PowerWheel.IronTeeth (type: AnimationClip)`
   - `PowerWheel.IronTeeth.Model (type: Animator)`

#### If animations aren't working

This is a very manual process, but one thing that tripped me up was the fact that the animation was pointing to the mesh ID instead of referencing it by name. The gear was animating, but the wheel itself wasn't so I changed the animation layer's name to match the mesh name for the wheel and it started working.

The issue you encounter may (and likely will) be a completely different one, but hopefully that example gives you an idea of where / how to start troubleshooting. Go into every asset and make sure any references to materials, meshes, etc. aren't missing or pointing to files that don't exist. This might require some project-wide text searching to properly sleuth it out.