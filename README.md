# Hide-Demo-Unity
This is a simple project based on https://learn.unity.com/tutorial/hide-h1zl#

# How to play
1. Go to https://spudoku.github.io/Hide-Demo-Unity/
2. Look with the mouse, and move with WASD
3. Try to tag the red Hiders to increase your score.

# The Technique
Bots use raycasting to determine points that are "hidden" from the Seeker (the player character).

All valid hiding objects (objects tagged with the "Hide" tag) are checked. The nearest possible
location is chosen. Some calculations are done to make sure the chosen location is outside of the
collider of the chosen object, notably by using a raycast to find the outer edge of the object.


# Credits
Rock assets (Stylized Low Poly Rocks) by: https://justcreate3d.studio/
Plant assets ("Nature Pack - Low Poly Trees & Bushes") by: https://downtowngs.com/
