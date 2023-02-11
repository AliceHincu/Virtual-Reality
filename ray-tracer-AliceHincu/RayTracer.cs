using System;
using System.Runtime.InteropServices;

namespace rt
{
    class RayTracer
    {
        private Geometry[] geometries;
        private Light[] lights;

        public RayTracer(Geometry[] geometries, Light[] lights)
        {
            this.geometries = geometries;
            this.lights = lights;
        }

        private double ImageToViewPlane(int n, int imgSize, double viewPlaneSize)
        {
            var u = n * viewPlaneSize / imgSize;
            u -= viewPlaneSize / 2;
            return u;
        }

        private Intersection FindFirstIntersection(Line ray, double minDist, double maxDist)
        {
            var intersection = new Intersection();

            foreach (var geometry in geometries)
            {
                var intr = geometry.GetIntersection(ray, minDist, maxDist);

                if (!intr.Valid || !intr.Visible) continue;

                if (!intersection.Valid || !intersection.Visible)
                {
                    intersection = intr;
                }
                else if (intr.T < intersection.T)
                {
                    intersection = intr;
                }
            }

            return intersection;
        }

        private bool IsLit(Vector point, Light light)
        {
            // ADD CODE HERE: Detect whether the given point has a clear line of sight to the given light
            Line lightray = new Line(light.Position, point);
            double dist2point = Math.Sqrt(Math.Pow((light.Position.X - point.X), 2) + Math.Pow((light.Position.Y - point.Y), 2) + Math.Pow((light.Position.Z - point.Z), 2));
            Intersection intersection = FindFirstIntersection(lightray, 0, dist2point - 0.5);

            if (!intersection.Valid || !intersection.Visible)
            {
                return true;
            }
            return false;
        }

        public void Render(Camera camera, int width, int height, string filename)
        {
            var background = new Color();
            var image = new Image(width, height);

            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    // ADD CODE HERE: Implement pixel color calculation
                    Vector pixelPoint = camera.Position + camera.Direction * camera.ViewPlaneDistance
                        + camera.Up * ImageToViewPlane(j, height, camera.ViewPlaneHeight)
                        + (camera.Up ^ camera.Direction).Normalize() * ImageToViewPlane(i, width, camera.ViewPlaneWidth);

                    Line ray = new Line(camera.Position, pixelPoint);

                    Intersection intersection = FindFirstIntersection(ray, camera.FrontPlaneDistance, camera.BackPlaneDistance);
                    if (!intersection.Valid || !intersection.Visible)
                    {
                        image.SetPixel(i, j, background);
                    }
                    else
                    {
                        Vector litPoint = intersection.Position;
                        Geometry geometry = intersection.Geometry;
                        Color pixelColor = new Color();

                        foreach (Light light in lights)
                        {
                            Color colorForLight = geometry.Material.Ambient * light.Ambient;

                            if (IsLit(litPoint, light))
                            {
                                Vector N = geometry.Normal(litPoint);
                                Vector T = (light.Position - litPoint).Normalize();

                                if (N * T > 0)
                                {
                                    colorForLight += geometry.Material.Diffuse * light.Diffuse * (N * T);
                                }

                                Vector E = (camera.Position - litPoint).Normalize();
                                Vector R = N * (N * T) * 2 - T;

                                if (E * R > 0)
                                {
                                    colorForLight += geometry.Material.Specular * light.Specular * Math.Pow(E * R, geometry.Material.Shininess);
                                }

                                colorForLight *= light.Intensity;
                            }

                            pixelColor += colorForLight;
                        }

                        image.SetPixel(i, j, pixelColor);
                    }
                }
            }

            image.Store(filename);
        }
    }
}