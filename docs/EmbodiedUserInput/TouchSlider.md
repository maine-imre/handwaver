## Touch Slider
### Description
* The touch slider was made through constructing a Unity object that has a line renderer component and a sphere.
* The functionality is intended to be that a person performs a gesture, and then that gesture takes position data from the hands and compares it to the position of the line. When Someone is close enough to the line, the position of the sphere is updated to the closest point on the line to the designated body part.
* We have achieved this functionality by designating the gesture to be a point gesture. A user must be pointing which requires the index finger to be extended while the middle finger, ring finger and pinky are not extended. When the user brings their fingertip within 2 cm of the line, it updates the position of the sphere to be on the line nearest to the fingertip of the hand performing the gesture.
  * This will eventually be implemented in scenes to perform various functions.
  * The first functionality looks like it will be manipulating the 3 dimensionali cross sections of 4 dimensional objects. (rotating the object / moving it) The intention is to allow the 3d cross section to be dynamically altered in order to help people understand the overall structure of the 4d object.
