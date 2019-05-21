using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.HandWaver.VariedPerspective
{

    public class perspectiveManager : MonoBehaviour
    {
        
        public enum perspectiveMode
        {
            None, 
            OnlyMain, 
            OnlyOrtho
        }
        
        private static Camera _mainCamera = Camera.main;
        public Camera orthoCamera;
        public perspectiveMode _currentMode;

        public perspectiveMode currentMode
        {
            get => _currentMode;
            set
            {
                _currentMode = value;
                
            }
        }
        
        


    }
}