using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class sceneChangerMenuItem : MonoBehaviour
{
	/// Example Function to add further menu items
	//[MenuItem("Load Scene/Content Layer/<SCENENAME>")]
    //static void open<SCENENAME>()
    //{
    //	EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
    //	EditorSceneManager.OpenScene("Assets/Scenes/ContentLayers/<SCENENAME>.unity");

    //}
    [MenuItem("Load Scene/Base Scene/HandWaver Base")]
    private static void OpenHWBaseScene()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/Scenes/BaseScenes/HandWaverBase.unity");
    }

    [MenuItem("Load Scene/Base Scene/Geometers Plantarium Base")]
    private static void OpenGPBaseScene()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/Scenes/BaseScenes/GeometersPlanetariumBase.unity");
    }

    [MenuItem("Load Scene/Base Scene/Network Base")]
    private static void OpenNetworkBase()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/Scenes/BaseScenes/networkingBase.unity");
    }

    [MenuItem("Load Scene/Content Layer/RSDES")]
    private static void openRSDES()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/Scenes/ContentLayers/GeometersPlanetariumContent/RSDES.unity");
    }

    [MenuItem("Load Scene/Content Layer/Lattice Land")]
    private static void openLatticeLand()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/Scenes/ContentLayers/LatticeLand.unity");
    }

    [MenuItem("Load Scene/Content Layer/Shearing Lab")]
    private static void openShearing()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/Scenes/ContentLayers/ShearingLab.unity");
    }

    [MenuItem("Load Scene/Content Layer/Three Torus")]
    private static void openTorus()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/Scenes/ContentLayers/ThreeTorus.unity");
    }
}