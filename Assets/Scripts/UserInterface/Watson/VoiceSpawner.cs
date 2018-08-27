///**
//HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
//(C) University of Maine
//See license info in readme.md.
//www.imrelab.org
//**/

//using IBM.Watson.DeveloperCloud.DataTypes;
//using UnityEngine;
//using UnityEngine.UI;
//using IBM.Watson.DeveloperCloud.Widgets;

//#pragma warning disable 414

//namespace IMRE.HandWaver
//{
//    / <summary>
//    / Simple widget for displaying the Natural Language Classification in the UI.
//    / </summary>

//	public class VoiceSpawner : Widget
//	{
//		public WatsonGameManager gameMan;

//		#region Inputs
//		[SerializeField]
//		private Input m_ClassInput = new Input("ClassInput", typeof(ClassifyResultData), "OnClassInput");
//        #endregion

//        #region Widget interface
//        / <exclude />
//        protected override string GetName()
//		{
//			return "ClassDisplay";
//		}
//		#endregion

//		#region Private Data
//		[SerializeField]
//		private Text m_ClassDisplay = null;
//		#endregion

//		#region Event Handlers
//		private void OnClassInput(Data data)
//		{
//			ClassifyResultData results = (ClassifyResultData)data;
//			Debug.Log(string.Format("Top class: {0} ({1:0.00})", results.Result.top_class, results.Result.topConfidence));
//			gameMan.handleWatsonCommand(results);
//		}
//		#endregion
//	}
//}
