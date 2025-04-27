# Hide-Demo-Unity
This is a simple project based on https://learn.unity.com/tutorial/hide-h1zl#

# How to play
1. Go to https://spudoku.github.io/Hide-Demo-Unity/
2. Look with the mouse, and move with WASD
3. Left click to shoot at Props. You're shooting the ones that move!
4. You get a time penalty for missing. Try to hunt down all the Props
    as quickly as possible!

# The Technique
Bots use raycasting to determine points that are "hidden" from the Seeker (the player character).

All valid hiding objects (objects tagged with the "Hide" tag) are checked. The nearest possible
location is chosen. Some calculations are done to make sure the chosen location is outside of the
collider of the chosen object, notably by using a raycast to find the outer edge of the object.


# Credits
Rock assets (Stylized Low Poly Rocks) by: https://justcreate3d.studio/
Plant assets ("Nature Pack - Low Poly Trees & Bushes") by: https://downtowngs.com/

Classic Laser Pew sound effect: https://freesound.org/people/SeanSecret/sounds/440661/?
bell ding 2.wav: Alva Majo, https://freesound.org/people/5ro4/sounds/611112/?
Wrong.mp3 by AbdrTar -- https://freesound.org/s/558121/ -- License: Creative Commons 0
GunClick.mp3 by Yap_Audio_Production -- https://freesound.org/s/218482/ -- License: Attribution 4.0
Empty Gun Shot by KlawyKogut -- https://freesound.org/s/154934/ -- License: Creative Commons 0