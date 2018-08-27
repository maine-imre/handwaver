using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if Unity_Editor
using UnityEditor;
using System;

[InitializeOnLoad]

/// <summary>
/// This script checks to find if dependency packages exist in order to exclude them from our public source.
/// The main contributor(s) to this script is CB
/// Status: WORKING
/// </summary>
public class checkPackageDependencies : MonoBehaviour {

    static checkPackageDependencies()
    {
        checkMixCast();

        checkUnityStandardAssets();

        checkTextMeshPro();

        checkLeapMotion();

        checkFileBrowser();

        checkPoolManager();

        checkWireframeShader();

        checkZFBrowser();

        checkPUN();

        checkInstantVR();

        checkViveport();

        checkSteamworks();

//ASSETS USED WITH ATTRIBUTION.
//NEEDS DEBUG LOGS
//https://sketchfab.com/models/157de95fa4014050a969a8361a83d366#download
    }

    private static void checkSteamworks()
    {
    }

    private static void checkViveport()
    {
    }

    private static void checkInstantVR()
    {
    }

    private static void checkPUN()
    {
    }

    private static void checkZFBrowser()
    {
        if (AssetDatabase.FindAssets("ZFBrowser").Length == 0)
        {
            Debug.LogWarning("Please Download ZF Browser.  http://u3d.as/oaF");
        }
    }

    private static void checkWireframeShader()
    {
        if (AssetDatabase.FindAssets("JOLIX").Length == 0)
        {
            Debug.LogWarning("Please Download WireframeShader.  https://assetstore.unity.com/packages/vfx/shaders/directx-11/wireframe-shader-directx-11-94741");
        }
    }

    private static void checkPoolManager()
    {
        if (AssetDatabase.FindAssets("PathalogicalGames").Length == 0)
        {
            Debug.LogWarning("Please Download PoolManager.  http://u3d.as/1Z4");
        }
    }

    private static void checkFileBrowser()
    {
        if (AssetDatabase.FindAssets("StandaloneFileBrowser").Length == 0)
        {
            Debug.LogWarning("Please Download File Browser.  https://github.com/gkngkc/UnityStandaloneFileBrowser");
        }
    }

    private static void checkLeapMotion()
    {
        if (AssetDatabase.FindAssets("LeapMotion").Length == 0)
        {
            Debug.LogWarning("Please Download Leap Motion Orion.  https://github.com/leapmotion/UnityModules/releases");
        }
    }

    private static void checkTextMeshPro()
    {
        if (AssetDatabase.FindAssets("TextMeshPro").Length == 0)
        {
            Debug.LogWarning("Please Download TextMeshPro.");
        }
    }

    private static void checkUnityStandardAssets()
    {
        if (AssetDatabase.FindAssets("StandardAssets").Length == 0)
        {
            Debug.LogWarning("Please Download Unity Standard Assets.");
        }
    }

    private static void checkMixCast()
    {
        if(AssetDatabase.FindAssets("MixCast").Length == 0)
        {
            Debug.LogWarning("Please Download MixCast SDK.  www.mixcast.me");
        }
    }
}
#endif