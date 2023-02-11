using System;
using System.Collections.Generic;
using System.Drawing;

namespace rt
{
    class RayTracer
    {
        private Element element;
        private double searchStep = 1;
        private double maxSearchDistance = 1000;
        private Color[] rgba = new Color[]{
                new Color(0.0, 0.0, 0.0, 0.0),
                /*new Color(153/255, 153/255, 0.0, 0.2), // dark yellow*/  // uncomment for cool effect
                new Color(1.0, 1.0, 0.0, 0.5), // normal yellow
                new Color(1.0, 0.0, 1.0, 0.5),
                new Color(0.0, 1.0, 0.0, 0.5)
            };
        private Dictionary<int, int> colorMap = new Dictionary<int, int>
            {
                {0, 13},
                /*{1, 60},*/  // this too
                {1, 100},
                {2, 200},
                {3, 256}
            };

        public RayTracer(Element element)
        {
            this.element = element;
        }

        private double ImageToViewPlane(int n, int imgSize, double viewPlaneSize)
        {
            var u = n * viewPlaneSize / imgSize;
            u -= viewPlaneSize / 2;
            return u;
        }

        public void Render(Camera camera, int width, int height, string filename, bool withColor)
        {
            var background = new Color(0, 0, 0, 1.0);
            var image = new Image(width, height);
            Console.WriteLine("Started rendering");
            /*Element.maxObjX = 160;*/  // get rid of weird thing from brain

            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    Vector parallelVector = camera.Up ^ camera.Direction;
                    Vector viewPlanePoint = camera.Position +
                                  camera.Direction * camera.ViewPlaneDistance +
                                  camera.Up * ImageToViewPlane(j, height, camera.ViewPlaneHeight) +
                                  parallelVector * ImageToViewPlane(i, width, camera.ViewPlaneWidth);
                    Vector x0 = camera.Position;

                    Line line = new Line(x0, viewPlanePoint);  // the line between the camera and the viewplane

                    // Initialize the color and opacity
                    Color finalColor = new Color();
                    image.SetPixel(i, j, background);

                    // searchDistance = the "t" variable from the line equation (x = at+b) 
                    for (double searchDistance = 0; searchDistance < maxSearchDistance; searchDistance += searchStep)
                    {
                        Vector position = line.CoordinateToPosition(searchDistance); // get current position of t in space

                        // if the point is inside the element
                        if (element.IsPointInside(position))
                        {
                            // After you figure out if you are in the element, the next question is “what’s the value here?”
                            // We get the value from the data matrix
                            double value = element.GetValue(position);

                            Color color = new Color();

                            // if the value is smaller than 0.05 -> it has the background color
                            if (value / 255.0 >= 0.15)
                            {
                                if (withColor)
                                {
                                    foreach (var key in colorMap.Keys)
                                    {
                                        if (value <= colorMap[key])
                                        {
                                            color = rgba[key];
                                            break;
                                        }
                                    }
                                    
                                } else {
                                    color = new Color(value / 255.0, value / 255.0, value / 255.0, 0.5);
                                    
                                }

                                // update the color and the opacity
                                finalColor = MixColors(color, finalColor);
                                image.SetPixel(i, j, finalColor);

                                // if the opacity is >= 1 -> there is no need to continue, we can stop
                                if (finalColor.Alpha >= 1)
                                    break;
                            }
                        }

                    }
                }
            }

            image.Store(filename);
        }

        private Color MixColors(Color color, Color finalColor)
        {
            /*a01 = (1 - a0)·a1 + a0
            r01 = ((1 - a0)·a1·r1 + a0·r0) / a01
            g01 = ((1 - a0)·a1·g1 + a0·g0) / a01
            b01 = ((1 - a0)·a1·b1 + a0·b0) / a01*/
            var newAlpha = (1 - finalColor.Alpha) * color.Alpha + finalColor.Alpha;
            var newRed = ((1 - finalColor.Alpha) * color.Alpha * color.Red + finalColor.Alpha * finalColor.Red) / newAlpha;
            var newGreen = ((1 - finalColor.Alpha) * color.Alpha * color.Green + finalColor.Alpha * finalColor.Green) / newAlpha;
            var newBlue = ((1 - finalColor.Alpha) * color.Alpha * color.Blue + finalColor.Alpha * finalColor.Blue) / newAlpha;

            // update the color and the opacity using the formula discussed in the course
            return new Color(newRed, newGreen, newBlue, newAlpha);
        }
    }
}