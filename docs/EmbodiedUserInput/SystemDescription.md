Gestures are currently being redone so that they are no longer defined by the hardware being used but instead are defined by a set of definitions which then can use input from all supported hardware. 

To track the progress on this issue [go here](https://github.com/maine-imre/handwaver/issues/14)

---

## Table of Contents
* [Layers](https://github.com/maine-imre/handwaver/blob/feature/gesture-abstraction/docs/EmbodiedUserInput/SystemDescription.md#layers)
* [Input](https://github.com/maine-imre/handwaver/blob/feature/gesture-abstraction/docs/EmbodiedUserInput/SystemDescription.md#input)
* [Hardware](https://github.com/maine-imre/handwaver/blob/feature/gesture-abstraction/docs/EmbodiedUserInput/SystemDescription.md#hardware)
* [Classifying the Data](https://github.com/maine-imre/handwaver/blob/feature/gesture-abstraction/docs/EmbodiedUserInput/SystemDescription.md#classifying-the-data)
* [Definitions for Gestures](https://github.com/maine-imre/handwaver/blob/feature/gesture-abstraction/docs/EmbodiedUserInput/SystemDescription.md#definitions-for-gestures)
     * [Grasp](https://github.com/maine-imre/handwaver/blob/feature/gesture-abstraction/docs/EmbodiedUserInput/SystemDescription.md#grasp)
     * [Double Grasp](https://github.com/maine-imre/handwaver/blob/feature/gesture-abstraction/docs/EmbodiedUserInput/SystemDescription.md#double-grasp)
     * [Point](https://github.com/maine-imre/handwaver/blob/feature/gesture-abstraction/docs/EmbodiedUserInput/SystemDescription.md#point)
     * [Thumbs Up](https://github.com/maine-imre/handwaver/blob/feature/gesture-abstraction/docs/EmbodiedUserInput/SystemDescription.md#thumbs-up)
     * [Open Palm](https://github.com/maine-imre/handwaver/blob/feature/gesture-abstraction/docs/EmbodiedUserInput/SystemDescription.md#open-palm)
     * [Open Palm Push](https://github.com/maine-imre/handwaver/blob/feature/gesture-abstraction/docs/EmbodiedUserInput/SystemDescription.md#open-palm-push)
     * [Open Palm Swipe](https://github.com/maine-imre/handwaver/blob/feature/gesture-abstraction/docs/EmbodiedUserInput/SystemDescription.md#open-palm-Swipe)

---

## Layers
There are 3 layers to the gesture system.
* The base layer, called Interface Layer, adds support for hardware by using their specific APIs to grab the input from the sources and feed it into the next layers by adding their inputs to the relevant parts of the Imre gestures struct
* The middle layer, called Classifer Layer, defines states of the body and then checks to see if data on what a person in the environment is doing matches any of those states.
* The Top Layer, called Action Layer, uses the states found by the Classifier Layer to determine what functionality should be executed.

---

## Input
* **General Description:**  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Input is handled in [this file](https://github.com/maine-imre/handwaver/blob/feature/gesture-abstraction/Assets/Scripts/EmbodiedInput/BodyInputDataSystem.cs) where the APIs from the various hardwares are fed into a data structure, [defined here,](https://github.com/maine-imre/handwaver/blob/feature/gesture-abstraction/Assets/Scripts/EmbodiedInput/BodyInput.cs) which holds the data in a general way that the rest of the system can use. 

* **The Data Structure**  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; The generalized data is stored within a struct called **BodyInput** that is made up of other structs called **BodyComponents**. There are two additional structs within BodyInput called hands and fingers. A bodyComponent is made of a float 3x3 which holds three float3s that are for position, direction, and velocity. BodyInput has several instances of bodyComponent called head, neck, chest, waist, and 4 arrays of bodyComponents which represent the right and left leg as well as the right and left arm. There are then the references to the right and left hands.  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; The **hands** are made of two bodyComponents representing the palm and wrist, two float values called pinchStrength and PinchTolerance. There is a boolean value called isLeft which chooses the chirality as either left or right. There is an array of 5 fingers where index 0 is the thumb, 1 is the index finger, 2 is the middle finger, 3 is the ring finger, and 4 is the pinky. Finally there is a boolean called isPinching which keeps track of whether a hand is pinching or not by checking if the pinchStrength value is equal to or greater than the pinchTolerance value.  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; The **fingers** are made of an array of 4 bodyComponents which refer to the three joints in a finger and the finger tip. There is also a float3 which keeps track of the direction and a boolean called isExtended which is true when the finger is extended.
* **Velocity within Body Input**  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Velocity is determined by holding the most recent ten instances of BodyInput within the BodyInputSystem where the data is taken from the APIs. These ten BodyInput instances are used to see changes in the position over time and therefore are able to determine a velocity. This is useful for gestures which involve moving a body part.


---

## Hardware  
This sections notes which individual implementations we have added to our data structure by generalizing the data passed by the specific APIs.
* **OSVR**  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Allows easy setup and use of multiple VR hardware brands and types.
* **Aristo**  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Vive Pro hand tracking
* **Leap Motion**  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Hand tracking with a small peripheral device

---

## Classifying the Data
* A new type of classifier is assigned based on what gesture is to be detected. Then the data is used to determine if any of the gestures are active. Then the booleans which keep track of what stage in the functionality of a gesture the user is in are updated based on the previous state of the gesture(s).

---

## Definitions for Gestures
### Grasp  

* **Definition**  
Grasping is enacted during near contact or contact between the fingertips of the index finger and the thumb. The other fingers do not have a specific state that is required. There is a tolerance value which looks at how close the fingertips have to be before the grasp is initiated. Tweaking this value changes how easy or how hard it is to grasp something by changing the precision of the grasping and how large the radius is that grasping occurs within.  
* **Visual Feedback**  
Performing the "grasp" (pinch) gesture when there is no function attached to the context of where the grasp is happening will turn the index finger and thumb green. This green will be less visible than if the grasp was operating on an object or had some sort of function attached to its current context.  
 
### Double Grasp  
* **Definition**  
A double grasp is enacted during near contact or contact between the fingertips of the index finger and the thumb that share a hand while the other hand simultaneously achieves this state. The same tolerance value is used to determine a double grasp as with the single grasp. Activating this gesture is therefore also activating the single grasp gesture on each hand.
 
### Point  
* **Definition**  
Pointing is enacted when the index finger is directed at something. This gesture is recognized by having the pinky, ring finger and middle finger not extended while the index finger is extended. The thumb's extension state is not tracked for pointing.  

### Thumbs Up  
* **Definition**  
A thumbs up is enacted by extending the thumb while the other fingers are not extended. Usually this would require the thumb to be pointing at a certain angle, however it is not neccessary to do anything except extend only the thumb to activate this gesture.  

### Open Palm
* **Definition**  
An open palm is enacted by extending all fingers.  

### Open Palm Push  
* **Definition**  
An open palm push is enacted by extending all fingers and then moving the hand as if you were applying pressure to something.

### Open Palm Swipe
* **Definition**  
An open palm swipe is enacted by extending all fingers and then moving the hand as if you were chopping something.

---
