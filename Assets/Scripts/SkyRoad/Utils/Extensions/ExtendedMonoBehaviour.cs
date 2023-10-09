using UnityEngine;

namespace SkyRoad.Utils.Extensions
{
    public class ExtendedMonoBehaviour : MonoBehaviour
    {
        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public Vector3 Scale
        {
            get => transform.localScale;
            set => transform.localScale = value;
        }
        
        public Quaternion Rotation
        {
            get => transform.rotation;
            set => transform.rotation = value;
        }

        public float PosZ
        {
            get => transform.position.z;
            set => transform.SetZ(value);
        }

        public float PosX
        {
            get => transform.position.x;
            set => transform.SetX(value);
        }

        public float PosY
        {
            get => transform.position.y;
            set => transform.SetY(value);
        }
    }
}