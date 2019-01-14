using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace IMRE.HandWaver.ScaleStudy
{
    /// <summary>
    /// A script to control the scale and dimension study.
    /// See Dimmel's proposal.
    /// </summary>
    public class ScaleAndDimensionExperimentControl : MonoBehaviour
    {

        public bool vr_supported = true;

        public int condition = 0;
        public int stage = 0;

        public int idx = 0;

        public GameObject[] allObjs = new GameObject[18];

        #region  NetVars
        public GameObject squareNet;
        public GameObject triangleNet;

        public GameObject cubeNet;
        public GameObject tetrahedronNet;

        public GameObject hyperCubeNet;
        public GameObject hyperTetrahedronNet;
        #endregion

        #region XC_Vars
        public GameObject annulusXC;
        public GameObject circleXC;

        public GameObject torusXC;
        public GameObject sphereXC;

        public GameObject hyperTorusXC;
        public GameObject hyperSphereXC;
        #endregion

        #region VolumeVars
        public GameObject similarSquares;
        public GameObject similarTriangles;

        public GameObject similarTetrahedron;
        public GameObject similarCubes;

        public GameObject similarHyperCubes;
        public GameObject similarHyperTetrahedrons;
        public KeyCode listenForKey = KeyCode.RightArrow;
        #endregion


        public void Awake()
        {
            allObjs = new GameObject[18] { squareNet, triangleNet, cubeNet, tetrahedronNet,
            hyperCubeNet, hyperTetrahedronNet, annulusXC, circleXC, torusXC, sphereXC,
            hyperTorusXC, hyperSphereXC, similarSquares, similarTriangles, similarTetrahedron,
            similarCubes, similarHyperCubes, similarHyperTetrahedrons };
            allObjs.ToList().ForEach(p => p.SetActive(false));
            initializeStage(condition, stage, idx);
        }

        private void Update()
        {
            if (Input.GetKeyDown(listenForKey))
            {
                AdvanceStage();
            }
        }

        public void AdvanceStage()
        {
            if (idx < 2)
            {
                idx++;
            }
            else
            {
                idx = 0;
                if (stage < 2)
                {
                    stage++;
                }
                else
                {
                    condition++;
                    stage = 0;
                }

            }
            initializeStage(condition, stage, idx);
        }

        public void initializeStage(int cond, int cStage, int substage)
        {
            allObjs.ToList().ForEach(p => p.SetActive(false));
            switch (cStage)
            {
                case 0:
                    switch (substage)
                    {
                        case 0:
                            if (condition == 0)
                            {
                                squareNet.SetActive(true);
                            }
                            else
                            {
                                triangleNet.SetActive(true);
                            }
                            break;
                        case 1:
                            if (condition == 0)
                            {
                                cubeNet.SetActive(true);
                            }
                            else
                            {
                                tetrahedronNet.SetActive(true);
                            }
                            break;
                        case 2:
                            if (condition == 0)
                            {
                                hyperCubeNet.SetActive(true);
                            }
                            else
                            {
                                hyperTetrahedronNet.SetActive(true);
                            }
                            break;
                        default:
                            substage = 0;
                            break;
                    }
                    break;
                case 1:
                    switch (substage)
                    {
                        case 0:
                            if (condition == 0)
                            {
                                circleXC.SetActive(true);
                            }
                            else
                            {
                                annulusXC.SetActive(true);
                            }
                            break;
                        case 1:
                            if (condition == 0)
                            {
                                sphereXC.SetActive(true);
                            }
                            else
                            {
                                torusXC.SetActive(true);
                            }
                            break;
                        case 2:
                            if (condition == 0)
                            {
                                hyperSphereXC.SetActive(true);
                            }
                            else
                            {
                                hyperTorusXC.SetActive(true);
                            }
                            break;
                        default:
                            substage = 0;
                            break;
                    }
                    break;
                case 2:
                    switch (substage)
                    {
                        case 0:
                            if (condition == 0)
                            {
                                similarSquares.SetActive(true);
                            }
                            else
                            {
                                similarTriangles.SetActive(true);
                            }
                            break;
                        case 1:
                            if (condition == 0)
                            {
                                similarTetrahedron.SetActive(true);
                            }
                            else
                            {
                                similarCubes.SetActive(true);
                            }
                            break;
                        case 2:
                            if (condition == 0)
                            {
                                similarHyperCubes.SetActive(true);
                            }
                            else
                            {
                                similarHyperTetrahedrons.SetActive(true);
                            }
                            break;
                        default:
                            substage = 0;
                            break;
                    }
                    break;
                default:
                    cStage = 0;
                    break;
            }
        }
    }
}