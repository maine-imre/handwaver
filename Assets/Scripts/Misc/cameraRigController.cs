using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IMRE.HandWaver
{
/// <summary>
/// Move the payer with the keyboard.
/// May be extended to align physical and virtual spaces for networking.
/// </summary>
	public class cameraRigController : MonoBehaviour
	{
		/// <summary>
		/// True if the scene is to be transformed by the keyboard.
		/// </summary>
		public bool _controlledByKeyboard = false;

		/// <summary>
		/// True if the scene is to be transformed by the controller.
		/// </summary>
		public bool _controlledByController = false;

		private bool _controllersConnected = false;

		/// <summary>
		/// List of all transforms to be moved using the method selected.
		/// </summary>
		public List<Transform> targetTransforms;

		/// <summary>
		/// Dictionary of the target transforms and their initial rotation
		/// </summary>
		public Dictionary<Transform, Quaternion> targetTransformRotations;

		/// <summary>
		/// Dictionary of the target transforms and their initial position
		/// </summary>
		public Dictionary<Transform, Vector3> targetTransformPositions;

		/// <summary>
		/// left XR Controller
		/// </summary>
		private Leap.Unity.Interaction.InteractionXRController left;

		/// <summary>
		/// Right XR Controller
		/// </summary>
		private Leap.Unity.Interaction.InteractionXRController right;

		/// <summary>
		/// initial position of the controller
		/// </summary>
		private Transform _initPos;

		/// <summary>
		/// property of <see cref="_initPos"/>
		/// </summary>
		private Transform initPos
		{
			get
			{
				if(_initPos == null)
				{
					if (Input.GetKey(KeyCode.Joystick1Button2))
					{
						initPos = leapHandDataLogger.ins.currHands.LOVR.transform;
						targetTransformRotations = new Dictionary<Transform, Quaternion>();
						targetTransforms.ForEach(t => targetTransformRotations.Add(t, t.transform.rotation));
						targetTransforms.ForEach(t => targetTransformPositions.Add(t, t.transform.position));
					}
					else if (Input.GetKey(KeyCode.Joystick1Button0))
					{
						initPos = leapHandDataLogger.ins.currHands.ROVR.transform;
						targetTransformRotations = new Dictionary<Transform, Quaternion>();
						targetTransforms.ForEach(t => targetTransformRotations.Add(t, t.transform.rotation));
						targetTransforms.ForEach(t => targetTransformPositions.Add(t, t.transform.position));
					}
				}
				return _initPos;
			}
			set
			{
				_initPos = value;
			}
		}


		/// <summary>
		/// This is a combination boolean to see if the user wants to move the scene during this frame.
		/// </summary>
		private bool currentlyControlled
		{
			get
			{
				return _controlledByController  && _controllersConnected|| _controlledByKeyboard;
			}

		}

		private void Start()
		{
			if (leapHandDataLogger.ins == null || (leapHandDataLogger.ins.currHands.LOVR == null && leapHandDataLogger.ins.currHands.ROVR == null))
				_controllersConnected = false;
			else
			{
				left = leapHandDataLogger.ins.currHands.LOVR;
				right = leapHandDataLogger.ins.currHands.ROVR;
				_controllersConnected = true;
			}
		}

		/// <summary>
		/// Called every frame to handle input
		/// </summary>
		private void Update()
		{
			if(Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl))
			{
				if (Input.GetKeyDown(KeyCode.C))
					_controlledByController = !_controlledByController;
				else if (Input.GetKeyDown(KeyCode.K))
					_controlledByKeyboard = !_controlledByKeyboard;

			}

			if (!currentlyControlled)
				return;

			if (_controlledByKeyboard) {
				if (Input.GetKey(KeyCode.W))                                                    //forward
				{
					targetTransforms.ForEach(t => t.position += Vector3.forward * 0.01f);
				}
				else if (Input.GetKey(KeyCode.S))                                               //backward
				{
					targetTransforms.ForEach(t => t.position += Vector3.back * 0.01f);

				}

				if (Input.GetKey(KeyCode.A))                                                    //left
				{
					targetTransforms.ForEach(t => t.position += Vector3.left * 0.01f);

				}
				else if (Input.GetKey(KeyCode.D))                                               //right
				{
					targetTransforms.ForEach(t => t.position += Vector3.right * 0.01f);

				}

				if (Input.GetKey(KeyCode.Q))                                                    //rotate clockwise
				{
					targetTransforms.ForEach(t => t.rotation *= Quaternion.AngleAxis(.2f, Vector3.down));

				}
				else if (Input.GetKey(KeyCode.E))                                               //rotate counterclockwise
				{
					targetTransforms.ForEach(t => t.rotation *= Quaternion.AngleAxis(.2f, Vector3.up));

				}
			}

			if (_controlledByController)
			{

				if (Input.GetKeyDown(KeyCode.Joystick1Button2))                                 //setupn initial values left
				{
					initPos = left.transform;
					targetTransformRotations = new Dictionary<Transform, Quaternion>();
					targetTransforms.ForEach(t => targetTransformRotations.Add(t,t.transform.rotation));
					targetTransforms.ForEach(t => targetTransformPositions.Add(t, t.transform.position));
				}
				else if (Input.GetKey(KeyCode.Joystick1Button2))                                 //update on the fly left
				{
					targetTransforms.ForEach(t => t.position = left.position - initPos.transform.position + targetTransformPositions[t]);
					targetTransforms.ForEach(t => t.rotation = left.rotation *Quaternion.Inverse(initPos.transform.rotation) * targetTransformRotations[t]);
				}
				else if (Input.GetKeyDown(KeyCode.Joystick1Button0))                            //setup initial values right
				{
					initPos = right.transform;
					targetTransformRotations = new Dictionary<Transform, Quaternion>();
					targetTransforms.ForEach(t => targetTransformRotations.Add(t, t.transform.rotation));
					targetTransforms.ForEach(t => targetTransformPositions.Add(t, t.transform.position));
				}
				else if (Input.GetKey(KeyCode.Joystick1Button0))                                //update on the fly right
				{
					targetTransforms.ForEach(t => t.position = right.position - initPos.transform.position + targetTransformPositions[t]);
					targetTransforms.ForEach(t => t.rotation = right.rotation * Quaternion.Inverse(initPos.transform.rotation) * targetTransformRotations[t]);
				}
			}




		}
		
	}
}