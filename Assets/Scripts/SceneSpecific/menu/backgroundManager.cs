using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IMRE.HandWaver.Menu
{
    public class backgroundManager : MonoBehaviour
         {
             public int bgSceneCount;
             void Start()
             {
                 SceneManager.LoadSceneAsync("bg" + Random.Range(0, bgSceneCount), LoadSceneMode.Additive);
             }
             
         }
}