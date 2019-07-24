## Geometer's Planetarium Embodied User Input Actions

- [Pointing to Select Location on Earth](Geometer'sPlanetariumEmbodiedUserInputActions.md#Point-to-Select-a-Location-on-the-Earth)
    - [Gestures at the pin](Geometer'sPlanetariumEmbodiedUserInputActions.md#Gestures-at-the-Pins)
    - **Open Palm toggles at pins**
        - [tangent plane](Geometer'sPlanetariumEmbodiedUserInputActions.md#Tangent-Plane) 
        - terminator, 
        - latitude, 
        - longitude
    - Double Pinch and Stretch to cycle starlight at pin
    - **Pin selection / connections**
        - Great Arc
        - Great Circle
    - Double Pinch to Scale (refactor)
    - Pinch to move (new base class, similar to grasp, refactor physics system)
    - Swipe to Rotate Earth
    - Global Latitude
    - Global Longitude
    - Push Earth
    
    ---
    
    ### Point to Select a Location on the Earth  
    This is the action of activating a comparison of the position of a classifier.position value and the closest point on the surface of 
    the Earth. The position of the index finger's finger tip will be used in this case for the classifier.position value.
    The gesture of choice to activate this comparison is the point gesture. The position comparison will not place a pin on the planet's 
    surface until the distance between the index finger and Earth's surface is small enough to essentially be touching. This distance will
    be determined by a tolerance value.  
      
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
    rotation of the hand will have the palm facing neither directly at or directly away from the earth but instead it will be oreiented in
    a way which has the hand stacking the fingers on top of one another, usually the most comfortable position for this has the pinky at the
    bottom.
    The prefered gesture for this is the open palm.
    
    * #### Longitude
    Longitude in this case is specific to the longitude line which intersects the pin. When these increase in value the position
