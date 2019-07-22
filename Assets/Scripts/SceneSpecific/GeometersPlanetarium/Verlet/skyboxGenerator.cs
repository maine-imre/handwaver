namespace IMRE.HandWaver.Space.BigBertha
{
	/// <summary>
	///     This script does ___.
	///     The main contributor(s) to this script is TB
	///     Status: WORKING
	/// </summary>
	public class skyboxGenerator : UnityEngine.MonoBehaviour
    {
        public UnityEngine.Material skybox;
        public UnityEngine.Vector2 skyboxDimensions = new UnityEngine.Vector2(1800, 3600);
        public System.Collections.Generic.List<float[]> starData;

        public System.Collections.Generic.List<UnityEngine.Vector3> starPosData;

        // Use this for initialization
        private void Start()
        {
            //Debug.Log(convertCItoRGB(1.5f));
            starPosData = new System.Collections.Generic.List<UnityEngine.Vector3>();
            starData = new System.Collections.Generic.List<float[]>();
            System.IO.StreamReader rawStarData =
                new System.IO.StreamReader(@"Assets/Big-Birtha/StellarData/hygdata_v3.csv");
            rawStarData.ReadLine();

            while (!rawStarData.EndOfStream)
            {
                string line = rawStarData.ReadLine();
                string[] lineValues = line.Split(',');
                if (lineValues[6] != "")
                {
                    //if(lineValues[6] == "Betelgeuse" || lineValues[6] == "Rigel" || lineValues[6] == "Bellatrix" || lineValues[6] == "Mintaka" || lineValues[6] == "Alnilam" || lineValues[6] == "Alnitak" || lineValues[6] == "Saiph"){
                    //if(lineValues[6] == "Betelgeuse") {
                    //Debug.Log(lineValues[6]);
                    starPosData.Add(new UnityEngine.Vector3(float.Parse(lineValues[17]), float.Parse(lineValues[18]),
                        float.Parse(lineValues[19])));
                    if (lineValues[16] != "")
                        starData.Add(new[] {float.Parse(lineValues[13]), float.Parse(lineValues[16])});
                    else
                        starData.Add(new[] {float.Parse(lineValues[13]), 0.0f});
                }
            }

            float[][] coordinates = getImageCoordinates(UnityEngine.Mathf.RoundToInt(skyboxDimensions.x),
                UnityEngine.Mathf.RoundToInt(skyboxDimensions.y));
            generateImage(UnityEngine.Mathf.RoundToInt(skyboxDimensions.x),
                UnityEngine.Mathf.RoundToInt(skyboxDimensions.y), coordinates);
        }

        private float[][] getImageCoordinates(int height, int width)
        {
            float[][] output = new float[starPosData.Count][];
            for (int i = 0; i < starPosData.Count; i++)
            {
                UnityEngine.Quaternion angle =
                    UnityEngine.Quaternion.FromToRotation(UnityEngine.Vector3.right, starPosData[i]);

                float longitude = UnityEngine.Vector3.SignedAngle(UnityEngine.Vector3.right,
                    UnityEngine.Vector3.ProjectOnPlane(starPosData[i], UnityEngine.Vector3.up).normalized,
                    UnityEngine.Vector3.up);
                //int latitude = Mathf.RoundToInt(((Vector3.SignedAngle(Vector3.right, Vector3.ProjectOnPlane(starData[i],Vector3.forward).normalized, Vector3.forward)+180)));


                float latitude = UnityEngine.Vector3.Angle(
                    UnityEngine.Vector3.ProjectOnPlane(starPosData[i], UnityEngine.Vector3.up).normalized,
                    starPosData[i].normalized);
                if (latitude != 0)
                {
                    if (UnityEngine.Vector3.Dot(UnityEngine.Vector3.up, starPosData[i]) > 0f)
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
            UnityEngine.Color[] imageBytes = new UnityEngine.Color[width * height];
            for (int i = 0; i < imageBytes.Length; i++) imageBytes[i] = UnityEngine.Color.black;
            int a = 0;
            float latitudeScale = height / 180f;
            float longitudeScale = width / 360f;
            System.Collections.Generic.List<double> brightnesses = new System.Collections.Generic.List<double>();
            for (int i = 0; i < data.Length; i++)
            {
                int[] thisStar =
                {
                    UnityEngine.Mathf.RoundToInt(data[i][0] * longitudeScale) + width / 2,
                    UnityEngine.Mathf.RoundToInt(data[i][1] * latitudeScale) + height / 2
                };
                if (thisStar[0] >= width - 5 || thisStar[1] >= height - 5)
                {
                    a += 1;
                }
                else
                {
                    //values are arbitrary based on the difference between the brightness of the brightest and darkest visible star
                    double brightness = (magnitudeDifference(starData[i][0]) - 1 - 630.9573444802) /
                                        (1 - 630.9573444802);
                    brightnesses.Add(brightness);
                    if (brightness > 1)
                    {
                        UnityEngine.Debug.Log("Brightness too high!");
                        UnityEngine.Debug.Log(brightness);
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
            UnityEngine.Texture2D image = new UnityEngine.Texture2D(width, height);
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
            return UnityEngine.Mathd.Pow(10, 0.4f * (a - -1.44f));
        }

        private UnityEngine.Color ReturnBrighter(UnityEngine.Color a, UnityEngine.Color b)
        {
            if (a.a + a.r + a.g + a.b > b.a + b.r + b.g + b.b)
                return a;
            return b;
        }

        private UnityEngine.Color BrightnessADJ(UnityEngine.Color baseColor, double brightness)
        {
            //-27 to 25
            return new UnityEngine.Color(baseColor.r * (float) brightness, baseColor.g * (float) brightness,
                baseColor.b * (float) brightness, baseColor.a * (float) brightness);
        }

        private UnityEngine.Color convertCItoRGB(float colorIndex)
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
        private UnityEngine.Color convertKtoRGB(float k)
        {
            UnityEngine.Color outputColor = new UnityEngine.Color(0, 0, 0);
            k = k / 100f;

            //Calculate Red Value
            if (k <= 66)
            {
                outputColor.r = 255;
            }
            else
            {
                int tmp = UnityEngine.Mathf.RoundToInt(329.698727446f * UnityEngine.Mathf.Pow(k - 60, -0.1332047592f));
                if (tmp < 0)
                    tmp = 0;
                else if (tmp > 255) tmp = 255;
                outputColor.r = tmp;
            }

            //Calculate Green Value
            if (k <= 66)
            {
                int tmp = UnityEngine.Mathf.RoundToInt(99.4708025861f * UnityEngine.Mathf.Log(k) - 161.1195681661f);
                if (tmp < 0)
                    tmp = 0;
                else if (tmp > 255) tmp = 255;
                outputColor.g = tmp;
            }
            else
            {
                int tmp = UnityEngine.Mathf.RoundToInt(288.1221695283f * UnityEngine.Mathf.Pow(k - 60, -0.0755148492f));
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
                int tmp = UnityEngine.Mathf.RoundToInt(138.5177312231f * UnityEngine.Mathf.Log(k) - 305.0447927307f);
                if (tmp < 0)
                    tmp = 0;
                else if (tmp > 255) tmp = 255;
                outputColor.b = tmp;
            }

            return outputColor;
        }
    }
}