using System;
using UnityEngine;

namespace LonelyIsland.Misc
{
    public class SerializedVector3
    {
        public SerializedVector3() { }
        public SerializedVector3(float X, float Y, float Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        public SerializedVector3(Vector3 vector)
        {
            X = vector.x;
            Y = vector.y;
            Z = vector.z;
        }

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector3 Vector3 { get { return new Vector3(X, Y, Z); } }
        
    }
}
