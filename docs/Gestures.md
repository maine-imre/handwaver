  We extend the gesture system that Leap Motion built in their [Paint3D](https://github.com/leapmotion/Paint) demo.

  LeapMotion's structure divides gestures into two parts, OneHandedGestures and TwoHandedGestures.
  By default, each of these supports LeapMotion hands.
  We extend this to include OSVR overrides for each of the gestures.

  An abstraction level provides links to the OSVR as well as a structure for audio, visual and tactile feedback.
  A second level defines specific gestures (e.g. Point, Swipe, OpenPalm) each of which is paired with an OSVR override (e.g. Trigger, Grip).
  A third level implements these specific gestures in user-interface applications.
