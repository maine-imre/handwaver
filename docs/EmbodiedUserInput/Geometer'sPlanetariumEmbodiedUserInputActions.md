## Geometer's Planetarium Embodied User Input Actions

- [Point to Select Location on Earth](Geometer'sPlanetariumEmbodiedUserInputActions.md#Point-to-Select-a-Location-on-the-Earth)
    - [Gestures at the pin](Geometer'sPlanetariumEmbodiedUserInputActions.md#Gestures-at-the-Pins)
    - **Open Palm toggles at pins**
        - [tangent plane](Geometer'sPlanetariumEmbodiedUserInputActions.md#Tangent-Plane) 
        - [terminator](Geometer'sPlanetariumEmbodiedUserInputActions.md#Terminator) 
        - [latitude](Geometer'sPlanetariumEmbodiedUserInputActions.md#latitude) 
        - [longitude](Geometer'sPlanetariumEmbodiedUserInputActions.md#longitude)
        - [Starlight Rays](Geometer'sPlanetariumEmbodiedUserInputActions.md#Tangent-Plane) 
    - **Pin selection / connections**
        - [Great Arc](Geometer'sPlanetariumEmbodiedUserInputActions.md#Great-Arc) 
        - [Great Circle](Geometer'sPlanetariumEmbodiedUserInputActions.md#Great-Circle) 
    - [Double Pinch to Scale](Geometer'sPlanetariumEmbodiedUserInputActions.md#Double-Pinch-to-scale) 
    - [Pinch to move](Geometer'sPlanetariumEmbodiedUserInputActions.md#Pinch-to-Move) 
    - [Rotate Earth](Geometer'sPlanetariumEmbodiedUserInputActions.md#Rotate-Earth) 
    - [Push Earth](Geometer'sPlanetariumEmbodiedUserInputActions.md#Push-Earth) 
    - [Global Latitude](Geometer'sPlanetariumEmbodiedUserInputActions.md#Global-Latitude) 
    - [Global Longitude](Geometer'sPlanetariumEmbodiedUserInputActions.md#Global-Longitude) 
    
    ---
    
    ### Point to Select a Location on the Earth  
    This is the action of activating a comparison of the position of a classifier.position value and the closest point on the surface of 
    the Earth. The position of the index finger's finger tip will be used in this case for the classifier.position value.
    The gesture of choice to activate this comparison is the point gesture. The position comparison will not place a pin on the planet's 
    surface until the distance between the index finger and Earth's surface is small enough to essentially be touching. This distance       will be determined by a tolerance value.  
      
    The pin that is placed will hold the position data and facilitate further interactions that allow for dynamic location based
    functionality to happen.
    
    ---
    ### Gestures at the Pins
    Most of the gestures at the pin were intended to be swipes. However the swipe velocity is currently not functioning with Vive Pro.
    So to avoid this problem for the time being, They will be implemented with open palm in mind. The only difference between open palm
    and open palm swipe is that one requires movement to activate. Generally These will have similar data to compare between the gestures
    which are used. (see each description)
    
    * #### Tangent Plane  
    The tangent Plane is a plane which contacts the surface of the earth at the point where the pin is placed. When toggled on it will 
    display a plane in the form of a bounded circular disc which represents the larger infinite plane.  
    The toggling will activate when the classifier.position for the palm of a hand comes within close proximity or contact with the 
    position of the pin on the surface of the Earth.  
    The angle of the hands rotation represented by the classifier.direction should be tangent to the surface of the Earth. In this case 
    this means that the palm is facing the surface of the earth.  
    The gesture used here will be the Open Palm.
    
    * #### Terminator  
    The terminator is the circle which represents the area where a star directly above the earth at the pin's location could shine 
    light to. This circle is centered on the pin and beyond the edge of the circle, the light from the star would not be seen.  
    This will toggle the terminator when the classifier.position for the palm of a hand comes in close proximity or contact with the pin.
    The classifier.Direction will need to be used to detect when the palm of the hand is facing up and straight away from the earth.
    Essentially 180 degrees difference when compared to the tangent plane direction.  
    The gesture used here will be the open palm.
    
    * #### Latitude  
    Latitude in this case is specific to the latitude line which intersects the pin. When these increase in value the position 
    described is farther from the equator. The latitude line intersecting the pin is rendered around the entire world when it is toggled on. 
    to toggle this, the proximity between the classifier.position of the hand and the position of the pin on the surface of the planet will
    need to be very close or touching.  
    The Palm of the hand will need to be facing at a 90 degree angle to the surface where the pin touches the planet. This means that the
    rotation of the hand will have the palm facing neither directly at or directly away from the earth but instead it will be orieented in
    a way which has the hand stacking the fingers on top of one another, usually the most comfortable position for this has the pinky at the
    bottom. The hand will also need to be approximately parallel with latitude lines.
    The prefered gesture for this is the open palm.
    
    * #### Longitude
    Longitude, in this case, is specific to the longitude line which intersects the pin. When these increase in value the position described is getting farther East or West from the Greenwich Meridian. The longitude line for the pin is rendered around the entire planet when this is toggled on.  
    To toggle this, the proximity between the classifier.position of the hand and the position of the pin must be very close or touching.  
    The Palm of the hand will need to be facing at a 90 degree angle to the surface of the Earth. This means that the rotation of the hand will have the palm facing neither directly at or directly away from the earth but instead it will be oriented in a way which has the hand stacking the fingers on top of one another, usually the most comfortable postion for this has the pinky at the bottom. The hand wil also need to be approximately parallel with the longitude lines.  
    The prefered gesture for this is the open palm.  

    * #### Starlight Rays  
    Starlight rays are the representation of a stair's light if it were to be positioned in such a way that a line that extended straight up from the point where a pin touched the Earth's surface were to collide with a star. 
    The idea with this is to look at the proximity of the pin position and classifier.position. Then use a two handed gesture, in this case a double pinch, to toggle through the states of the starlight rays. This would look something like gripping the pin with one hand while pinching with the other and moving the hand not on the pin away from the other hand. Movement of the previously described hand closer to the hand on the pin could lead to going back through the toggles in the opposition order.  
    
    * #### Great Arc
    A great arc is the shortest distance between two points on a sphere where the distance is measured along the surface of that sphere.  
    The gesture for visually representing a great arc requires close proximity between the clssifier.position and the first pin on the surface. Then when the proximity has been achieved the gesture must be held until close proximity to a different point has happened in order for the functionality to be achieved. In this case a point gesture will be used and the proximity between the finger tip of the index finger and the point will be the additional check on top of the gesture. This will then lead into the next functionality called great circles.
    * #### Great Circle  
    A great circle is the circle along the surface of a sphere. In this case we are forming these through the use of two point on the surface of the sphere. The idea with this is that it will be a continuation of the great arc. When a great arc is formed, if you do not cancel the gesture and instead bring your finger back to the original pin, then a great circle will appear that interects both of the two pins which were involved in its creation. See great arc above for notes on the gesture.  
    
---
    
### Double Pinch to scale  
Scaling in this scene refers to the dynamic alteration of the environment's scale. When the scale is made smaller, the surroundings appear to get larger relative to the user and vice versa.  
This gesture will require the user to perform the two handed gesture in mid air without a pin near one of the hands. Then moving the hands closer will make the objects smaller and moving their hands farther apart will make the objects larger. 
This needs to be exclusive with the starlight rays toggle. To do this the functionality could be limited if near the pins or in genreal when near the surface of the Earth.
    
---
    
### Pinch to Move  
Pinching to move within the context of geometer's planetarium will generally be the pins themselves being moved from one location to another along the Earth's surface.  
This will require close proximity between the classifier.position and pin position while a gesture is performed, in this case a pinch. 
It will work with both hands. Also when the gesture is no longer detected the assumption will be that the user has decided to place down the object. This will put it in the closest position on the earth surface to the hand performing the gesture.  
A case which may need to be handled is distance during the gesture's activation. It can be assumed that if a person has brought their hand far enough away from the pin while trying to move it that they are not actually trying to move the pin. Therefore it may be worth it to play with a tolerance value which will stop the grasp if the hand is too far from the pin during the enacting of the gesture.  

---

### Rotate Earth  
Rotating the Earth will ideally require a gesture with a velocity. However velocity is currently not properly tracked for vive pro and so a different gesture will be slotted in. The gesture will be performed at the surface of the Earth and the placeholder will have a set speed for the rotation of the Earth.  
This gesture will be open palm. The gesture will require proximity to the surface of the Earth. Due to this gesture choice, there needs to be an exclusive relationship with the other gestures that uses open palm but requires proximity with a pin. This could be in the form of requiring the initial activation to be away from pin positions. But once the gesture begins it must be cancelled (no longer performed) to stop the rotation. This choice for needing to be cancelled would allow the hand to pass through pins which are rotating with the Earth without stopping if one of the pins gets too close to the hand.

---

### Push Earth  
Pushing the Earth would ideally require a gesture with a velocity. However velocity is currently not properly tracked for vive pro and so a different gesture will be slotted in. The gesture will be performed at the surface of the Earth and the placeholder will have a set speed for the movement of the user relative to the Earth.  
This gesture will be thumbs up. The gesture will require proximity to the surface of the Earth. Due to the gesture choice it will be in need of exclusivity with the global latitude and longitude actions.

---

### Global Latitude  
The global latitude refers to the latitude lines along the entirety of the surface of the Earth and the projections of latitude beyond the Earth. This therefore represents four states: No lines, Just Latitude lines on the Earth, Just the projections of the Latitude lines beyond Earth, and both of the Latitude lines on the Earth and Beyond the Earth active.  
The classifier gesture that activates this will be aligned with the floor horizontally. In this case the gesture to be used is the thumbs up. The thumb should be pointed sideways.  
To toggle through the 4 states, there needs to be a cooldown between each toggle. This will allow a person to activate the gesture and then wait while it is still activated in order to get to other states, if the desired state is not attained with the first toggle from that activation of the gesture. This cooldown will stop the toggling from needing 4 activations of the gesture to get through all of the states. This also stops the gesture from making the lines flash wildly as each state is achieved every new frame when the check is made.

---

### Global Longitude  
The global longitude refers to the longitude lines along the entirety of the surface of the Earth and the projections of longitude beyond the Earth. This therefore represents four states: No lines, Just Longitude lines on the Earth, Just the projections of the Longitude lines beyond Earth, and both of the Longitude lines on the Earth and Beyond the Earth active.  
The classifier gesture that activates this will be aligned with the floor vertically. In this case the gesture to be used is the thumbs up. The thumb should be pointed upwards.  
To toggle through the 4 states, there needs to be a cooldown between each toggle. This will allow a person to activate the gesture and then wait while it is still activated in order to get to other states, if the desired state is not attained with the first toggle from that activation of the gesture. This cooldown will stop the toggling from needing 4 activations of the gesture to get through all of the states. This also stops the gesture from making the lines flash wildly as each state is achieved every new frame when the check is made.
