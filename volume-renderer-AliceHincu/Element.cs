using System;
using System.Drawing.Printing;
using System.IO;

namespace rt
{
    public class Element
    {
/*        private const string FileName = "assets/vertebra.dat";
        public static int maxObjX = 47;
        public static int maxObjY = 512;
        public static int maxObjZ = 512;*/

        private const string FileName = "assets/head.dat";
        public static int maxObjX = 181;
        public static int maxObjY = 217;
        public static int maxObjZ = 181;

        public byte[,,] obj3D { get; } = new byte[maxObjX, maxObjY, maxObjZ]; // this is the 3D matrix - used for coloring - it stores voxels

        public Element()
        {
            Initialize();
        }

        private void Initialize()
        {
            // read the file (which has bytes inside) as a 3d matrix
            using (FileStream fs = File.OpenRead(FileName))
            {
                var br = new BinaryReader(fs);
                for (int i = 0; i < maxObjX; i++)
                    for (int j = 0; j < maxObjY; j++)
                        for (int k = 0; k < maxObjZ; k++)
                            obj3D[i, j, k] = br.ReadByte();
                br.Close();
            }

            
        }
        
        public bool IsPointInside(Vector position)
        {
            // it has to be between the minimum and maximum coordinates...min coords = (0,0,0), max coords = (maxObjX, maxObjY, maxObjZ)
            return Convert.ToInt32(position.X) > 0 && Convert.ToInt32(position.Y) > 0 && Convert.ToInt32(position.Z) > 0 &&
                   Convert.ToInt32(position.X) < maxObjX && Convert.ToInt32(position.Y) < maxObjY && Convert.ToInt32(position.Z) < maxObjZ;
        }

        public int GetValue(Vector position)
        {
            // get the value from the 3d matrix. Return 0 if position is not inside element
            if (!IsPointInside(position))
                return 0;
            return obj3D[Convert.ToInt32(position.X), Convert.ToInt32(position.Y), Convert.ToInt32(position.Z)];
        }
        
        
    }
}