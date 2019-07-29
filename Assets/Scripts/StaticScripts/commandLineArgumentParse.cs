/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

#if StandaloneWindows64
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using BlueprintReality.MixCast;
using System.Linq;
using IMRE.HandWaver.HWIO;
using IMRE.HandWaver.Kernel;
using UnityEngine.Events;
using WebSocketSharp;

namespace IMRE.HandWaver
{
	public enum mixCastTargetMode { primaryMonitor, secondaryMonitor, primaryAlt};
	
	/// <summary>
	/// Parses command line arguments that control the scene loaded and mixcast state.
	/// Depreciated.
	/// </summary>
	public static class commandLineArgumentParse 
	{
		public static string[] commandLineArguments = System.Environment.GetCommandLineArgs();
		private static bool _logOverride = false;
		public static UnityEvent logStateChange = new UnityEvent();

		public static bool logOverride
		{
			get
			{
				return _logOverride;
			}

			set
			{
				_logOverride = value;
				if(logStateChange.GetPersistentEventCount() != 0)
					logStateChange.Invoke();
			}
		}

		public static int monitorCountArgument()
		{
			int result = 1;

			foreach (string argument in commandLineArguments)
			{
				if (argument.ToLower().Contains("-dualmonitor"))
				{
					result = 2;
				}else if (argument.ToLower().Contains("-triplemonitor"))
				{
					result = 3;
				}
			}
			//this call of sceneStart() is placed here just so that its called on start of application; meaning no impact on the function.
			sceneStart();
			return result;
		}

		internal static void sceneStart()
		{
			// Sentinel value to signify if we found anything
			int loadHW = -1;
			
			foreach (string argument in commandLineArguments)
			{
				
				if (argument.ToLower().Contains("-l"))
				{
					//index of the argument that is directly after "-l" unless it is the last argument in which case we just leave it at the flag value.
					loadHW =
 Array.IndexOf(commandLineArguments, "-l") < commandLineArguments.Length ? Array.IndexOf(commandLineArguments, "-l")+1 : -1;		
					
				}
				
				// If argument is -sid
				//	Attempt to use the next argument as a sessionID
				if (argument.ToLower().Contains("-sid") && Array.IndexOf(commandLineArguments, "-sid")+1 < commandLineArguments.Length)
				{
					HandWaverServerSocket.overrideSID =
 commandLineArguments[Array.IndexOf(commandLineArguments, "-sid")+1];
				}
                
            }

			//If a -L flag was found
			if (loadHW != -1)
			{
				// Find the path to a file provided
				string path = commandLineArguments[loadHW];
				if (path.IsNullOrEmpty())
				{
					//we somehow didnt find a path
					Debug.LogError("No Path found! Be sure to provide a full path ending with .hw");
					return;
				}
				try
				{
					XMLManager.ins.LoadGeoObjs(path);
				}
				catch
				{
					// File does not exist or doesnt load properly
					Debug.LogError("Could not load save from file: "+path);					
				}
			}
		}

		public static bool logCheck()
		{
			return (commandLineArguments.Any(c => c.ToLower().Contains("-logging")) || logOverride);
		}

		/*public static mixCastTargetMode mixCastTarget()
		{
			mixCastTargetMode result = mixCastTargetMode.primaryMonitor;

			foreach (string argument in commandLineArguments)
			{
				if (argument.ToLower().Contains("-mixcast2"))
				{
					result = mixCastTargetMode.secondaryMonitor;
				}else if (argument.ToLower().Contains("-mixcast1"))
				{
					result = mixCastTargetMode.primaryAlt;
				}
			}

			return result;
		}*/

	}
}
#endif