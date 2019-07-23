using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace IMRE.HandWaver.Space.BigBertha
{
    /// <summary>
    ///     This script does ___.
    ///     The main contributor(s) to this script is TB
    ///     Status: WORKING
    /// </summary>
    public class skyboxGenerator : MonoBehaviour
    {
        public Material skybox;
        public Vector2 skyboxDimensions = new Vector2(1800, 3600);
        public List<float[]> starData;

        public List<Vector3> starPosData;

        // Use this for initialization
        private void Start()
        {
            //Debug.Log(convertCItoRGB(1.5f));
            starPosData = new List<Vector3>();
            starData = new List<float[]>();
            var rawStarData =
                new StreamReader(@"Assets/Big-Birtha/StellarData/hygdata_v3.csv");
            rawStarData.ReadLine();

            while (!rawStarData.EndOfStream)
            {
                var line = rawStarData.ReadLine();
                var lineValues = line.Split(',');
                if (lineValues[6] != "")
                {
                    //if(lineValues[6] == "Betelgeuse" || lineValues[6] == "Rigel" || lineValues[6] == "Bellatrix" || lineValues[6] == "Mintaka" || lineValues[6] == "Alnilam" || lineValues[6] == "Alnitak" || lineValues[6] == "Saiph"){
                    //if(lineValues[6] == "Betelgeuse") {
                    //Debug.Log(lineValues[6]);
                    starPosData.Add(new Vector3(float.Parse(lineValues[17]), float.Parse(lineValues[18]),
                        float.Parse(lineValues[19])));
                    if (lineValues[16] != "")
                        starData.Add(new[] {float.Parse(lineValues[13]), float.Parse(lineValues[16])});
                    else
                        starData.Add(new[] {float.Parse(lineValues[13]), 0.0f});
                }
            }

            var coordinates = getImageCoordinates(Mathf.RoundToInt(skyboxDimensions.x),
                Mathf.RoundToInt(skyboxDimensions.y));
            generateImage(Mathf.RoundToInt(skyboxDimensions.x),
                Mathf.RoundToInt(skyboxDimensions.y), coordinates);
        }

        private float[][] getImageCoordinates(int height, int width)
        {
            var output = new float[starPosData.Count][];
            for (var i = 0; i < starPosData.Count; i++)
            {
                var angle =
                    Quaternion.FromToRotation(Vector3.right, starPosData[i]);

                var longitude = Vector3.SignedAngle(Vector3.right,
                    Vector3.ProjectOnPlane(starPosData[i], Vector3.up).normalized,
                    Vector3.up);
                //int latitude = Mathf.RoundToInt(((Vector3.SignedAngle(Vector3.right, Vector3.ProjectOnPlane(starData[i],Vector3.forward).normalized, Vector3.forward)+180)));

                var latitude = Vector3.Angle(
                    Vector3.ProjectOnPlane(starPosData[i], Vector3.up).normalized,
                    starPosData[i].normalized);
                if (latitude != 0)
                {
                    if (Vector3.Dot(Vector3.up, starPosData[i]) > 0f)
                    {
                        //then it is closer to the north pole
                    }
                    else
                    {
                        latitude = latitude * -1;
                        //then it is closer to the south pole
                    }
                }

                //Debug.Log("Break");
                //Debug.Log(longitude);
                //Debug.Log(latitude);
                //Debug.Log(starPosData[i]);
                output[i] = new[] {longitude, latitude};
            }

            return output;
        }

        private void generateImage(int height, int width, float[][] data)
        {
            var imageBytes = new Color[width * height];
            for (var i = 0; i < imageBytes.Length; i++) imageBytes[i] = Color.black;
            var a = 0;
            var latitudeScale = height / 180f;
            var longitudeScale = width / 360f;
            var brightnesses = new List<double>();
            for (var i = 0; i < data.Length; i++)
            {
                int[] thisStar =
                {
                    Mathf.RoundToInt(data[i][0] * longitudeScale) + width / 2,
                    Mathf.RoundToInt(data[i][1] * latitudeScale) + height / 2
                };
                if (thisStar[0] >= width - 5 || thisStar[1] >= height - 5)
                {
                    a += 1;
                }
                else
                {
                    //values are arbitrary based on the difference between the brightness of the brightest and darkest visible star
                    var brightness = (magnitudeDifference(starData[i][0]) - 1 - 630.9573444802) /
                                     (1 - 630.9573444802);
                    brightnesses.Add(brightness);
                    if (brightness > 1)
                    {
                        Debug.Log("Brightness too high!");
                        Debug.Log(brightness);
                        brightness = 1;
                    }
                    else if (brightness < 0)
                    {
                        //Debug.Log("Brightness too low!");
                        //Debug.Log(brightness);
                        brightness = 0;
                    }

                    imageBytes[thisStar[0] + thisStar[1] * width] = ReturnBrighter(
                        BrightnessADJ(convertCItoRGB(starData[i][1]), brightness),
                        imageBytes[thisStar[0] + thisStar[1] * width]);
                    //Debug.Log(imageBytes[thisStar[0]+(thisStar[1]*width)]);
                    //imageBytes[thisStar[0]+(thisStar[1]*width)] = new Color(255,201,156);
                }
            }

            //Debug.Log(brightnesses.ToArray()[15]);
            //Debug.Log(a);
            var image = new Texture2D(width, height);
            image.SetPixels(imageBytes, 0);
            image.Apply();
            skybox.mainTexture = image;
            //System.IO.File.WriteAllBytes(@"Assets/Textures/orion.png",image.EncodeToPNG());
        }

        // Update is called once per frame
        private void Update()
        {
        }

        private double magnitudeDifference(float a)
        {
            //Appearent brightness compared to sirius
            return Mathd.Pow(10, 0.4f * (a - -1.44f));
        }

        private Color ReturnBrighter(Color a, Color b)
        {
            if (a.a + a.r + a.g + a.b > b.a + b.r + b.g + b.b)
                return a;
            return b;
        }

        private Color BrightnessADJ(Color baseColor, double brightness)
        {
            //-27 to 25
            return new Color(baseColor.r * (float) brightness, baseColor.g * (float) brightness,
                baseColor.b * (float) brightness, baseColor.a * (float) brightness);
        }

        private Color convertCItoRGB(float colorIndex)
        {
            //Debug.Log(convertCItoK(colorIndex));
            //return convertKtoRGB(convertCItoK(colorIndex));
            return convertKtoRGB(3590f);
        }

        private float convertCItoK(float colorIndex)
        {
            return 4600f * (1f / (0.92f * colorIndex + 1.7f) + 1f / (0.92f * colorIndex + 0.62f));
        }

        //http://www.tannerhelland.com/4435/convert-temperature-rgb-algorithm-code/
        private Color convertKtoRGB(float k)
        {
            var outputColor = new Color(0, 0, 0);
            k = k / 100f;

            //Calculate Red Value
            if (k <= 66)
            {
                outputColor.r = 255;
            }
            else
            {
                var tmp = Mathf.RoundToInt(329.698727446f * Mathf.Pow(k - 60, -0.1332047592f));
                if (tmp < 0)
                    tmp = 0;
                else if (tmp > 255) tmp = 255;
                outputColor.r = tmp;
            }

            //Calculate Green Value
            if (k <= 66)
            {
                var tmp = Mathf.RoundToInt(99.4708025861f * Mathf.Log(k) - 161.1195681661f);
                if (tmp < 0)
                    tmp = 0;
                else if (tmp > 255) tmp = 255;
                outputColor.g = tmp;
            }
            else
            {
                var tmp = Mathf.RoundToInt(288.1221695283f * Mathf.Pow(k - 60, -0.0755148492f));
                if (tmp < 0)
                    tmp = 0;
                else if (tmp > 255) tmp = 255;
                outputColor.g = tmp;
            }

            //Calculate Blue Value
            if (k >= 66)
            {
                outputColor.b = 255;
            }
            else if (k <= 19)
            {
                outputColor.b = 0;
            }
            else
            {
                var tmp = Mathf.RoundToInt(138.5177312231f * Mathf.Log(k) - 305.0447927307f);
                if (tmp < 0)
                    tmp = 0;
                else if (tmp > 255) tmp = 255;
                outputColor.b = tmp;
            }

            return outputColor;
        }
    }
}