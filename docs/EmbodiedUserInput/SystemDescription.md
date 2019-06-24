Gestures are currently being redone so that they are no longer defined by the hardware being used but instead are defined by a set of definitions which then can use input from all supported hardware. 

To track the progress on this issue [go here](https://github.com/maine-imre/handwaver/issues/14)

---

## Table of Contents
* [Layers](https://github.com/maine-imre/handwaver/wiki/Gestures#layers)
* [Input](https://github.com/maine-imre/handwaver/wiki/Gestures#input)
* [Hardware](https://github.com/maine-imre/handwaver/wiki/Gestures#hardware)
* [Definitions for Gestures](https://github.com/maine-imre/handwaver/wiki/Gestures#definitions-for-gestures)
     * [Grasp](https://github.com/maine-imre/handwaver/wiki/Gestures#grasp)

---

## Layers
There are 3 layers to the gesture system.
* The base layer, called Interface Layer, adds support for hardware by using their specific APIs to grab the input from the sources and feed it into the next layers by adding their inputs to the relevant parts of the Imre gestures struct
* The middle layer, called Classifer Layer, defines states of the body and then checks to see if data on what a person in the environment is doing matches any of those states.
* The Top Layer, called Action Layer, uses the states found by the Classifier Layer to determine what functionality should be executed.

---

## Input
Input is handled in [this file](https://github.com/maine-imre/handwaver/blob/feature/gesture-abstraction/Assets/Scripts/EmbodiedInput/BodyInputDataSystem.cs) where the APIs from the various hardwares are fed into a data structure [defined here,](https://github.com/maine-imre/handwaver/blob/feature/gesture-abstraction/Assets/Scripts/EmbodiedInput/BodyInput.cs) which holds the data in a general way that the rest of the system can use. 

---

## Hardware
* **OSVR**  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Allows easy setup and use of multiple VR hardware brands and types.
* **Aristo**  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Vive Pro hand tracking
* **Leap Motion**  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Hand tracking with a small peripheral device

---

## Definitions for Gestures
### Grasp  

* **Definition**  
This is a state of the hand where both the thumb and index finger have their fingertips brought together. The other fingers do not have a specific state that is required. There is a "closeness" value which looks at how close the fingertips have to be before the grasp is initiated. Tweaking this value changes how easy or how hard it is to grasp something by changing the precision of the grasping and how large the radius is that grasping occurs within.  
* **Visual Feedback**  
 Performing the "grasp" (pinch) gesture when there is no function attached to the context of where the grasp is happening will turn the index finger and thumb green. This green will be less visible than if the grasp was operating on an object or had some sort of function attached to its current context
---
---
updated 6/10/2019
