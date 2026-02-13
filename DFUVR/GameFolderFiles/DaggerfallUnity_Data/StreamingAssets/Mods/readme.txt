3D Weapons Shields And Items Array by westingtyler
MOD PAGE: https://www.nexusmods.com/daggerfallunity/mods/1058
ID FINDER PAGE: https://westingtyler.com/current/project/id_finder/index.html

2025-07-13:

This is a modder's prefab resource mod containing accurate texture-atlassed 3D models of all weapons and shields in Daggerfall, all hand-made in Blender and Paint Shop Pro by me, westingtyler. This is NOT an automatic in-game weapon replacer - until others make such mods using this library.

NOTE: There are currently mainly only weapons and shields in the mod - few "items" in the mod, though I have prepared/reserved IDs for them and would like to eventually add them as well. The non-combat items are an arrow, a crossbow bolt, and a few hand-held light sources, whose ids are shown in the ID spreadsheet images.

HOW TO USE (FOR MODDERS):
Modders using Daggerfall Unity can enable my DFMOD and spawn my 3D models by using the GameObjectHelper class.

You can easily search for ID numbers on this page: https://westingtyler.com/current/project/id_finder/index.html

TYPES OF MODS THIS RESOURCE COULD BE USEFUL FOR:
-interior redecoration mods (blacksmith, barracks, custom interiors)
-home decoration mods
-3d weapons/enemies/combat mods
-VR 1st person mods
-having physical weapons and shields in locations like stores could be one step toward a stealth/thieving system mod.

MODEL/TEXTURE DETAILS:
I 3D-modeled these in Blender and hand-painted a 3D texture atlas for them on May 14-15, 2025. Yes, like a single 30-hour workday. Then I then spent 1.5 months figuring out how to make a resource mod out of them at the suggestion of other DFU modders and being paranoid about settling on an unsound ID list, before settling on the one shown in the pictures and on my id finder web page.

Each model is about 480-800 triangles with a few curvy ones like axes being ~1,100, and the texture atlas is 1024x1024 pixels, giving it a slightly grainy and pixelated look. Their pivots align to industry standards as far as I can tell: hurty part (shooty, pokey, shield-bashy) goes toward bad guy (Z+), top goes up Y+, and X+ goes righrward toward right hand. They should ALL spawn in Daggerfall Unity with fully zeroed rotations (one of those things that means nothing to you unless you've dealt with export/import of 3d models between modeling software and game engines).

The unofficial Daggerfall Unity forum people suggested I make all the fbx files into prefabs for a resource mod that other mod authors could summon. For example, for decorating armor shops, for giving to 3d enemies, or for using in a 3D mod (VR or otherwise) as the player's hand-held weapons and shields.

FEEDBACK WELCOME:
I've never done this process before so feedback is welcome. If you use the mod and find the weapon or shield pivot points, scales, or textures need adjusting, just let me know so I can add those tweaks to a list for improving a future version. Similarly let me know if there are issues with the mod itself or practical areas I could tweak.

If you have suggestions about things to add or tweaks to make, let me know here in the POSTS tab, and I will add them to a list for consideration later on.

My dream is to be able to play Daggerfall with 3d weapons, characters, enemies, and ideally even in VR. For awhile there was little traction on many of these areas, but in recent times mod authers have been gearing up the tools and assets to make this happen, and it's awesome.

ABOUT ME:
I'm a game developer working on my own projects, and this has been a side project to improve my 3d-modelling and texturing skills.

my youtube:
https://www.youtube.com/channel/UCdJHTLc_M3CIssLVmykWn_w

my devlog:
https://westingtyler.com/current/devlog/index.htm

my site:
https://westingtyler.com/

STILL WOULD LIKE TO EVENTUALLY DO ( as of 2025-07-13):
-get feedback about scale of objects and pivot point locations, compared to paperdoll in game's ui. I recently tweaked them for version .904. - DONE
-make 66 percent and 33 percent durability shield variant models (once we're sure scale and pivot are good). - DONE
-make a hand-held torch model. - DONE
-make the other mod items in this order (crossbow, light flail, archer's axe). - DONE
-make Future Shock weapon models.

-add my own relevant custom textured models as I make them for my own games.
-get feedback to further refine the textures on the atlas (ie daedric)

-make a script to automate shield variant prep inside Blender (duplicate existing. move downward, re-mat, and rename).
-make a better enchantment shell object or shader workflow/script. For now there's an alpha clipped 128x128 png image of enchantment magic whose tiling movement could be shader-scripted - I tried adding it onto the default dfu shader material as emission but saw nothing added. There are also included inverted "enchanted-textured" variants of all the base models, but I'm not sure how useful these would be since they are just static alpha-clipped inverted shells.

Special thanks to the other DFU modders who helped provide insights and guidance to help turn my pile of 3d models into a proper DFMOD that is useable. Without their help the models would have just sat on my hard drive for like a decade annoying me, but now I am happy with the results.