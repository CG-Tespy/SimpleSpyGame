using UnityEngine;

namespace RPG
{
    public static class VectorExtensions
    {
        public static bool Equals(this Vector3 thisVec,
            Vector3 otherVec,
            float marginOfError)
        {
            float xDiff = Mathf.Abs(thisVec.x - otherVec.x);
            float yDiff = Mathf.Abs(thisVec.y -  otherVec.y);
            float zDiff = Mathf.Abs(thisVec.z - otherVec.z);
            return xDiff <= marginOfError &&
                yDiff <= marginOfError &&
                zDiff <= marginOfError;
        }

        public static bool Equals(this Vector2 thisVec,
            Vector2 otherVec,
            float marginOfError)
        {
            float xDiff = Mathf.Abs(thisVec.x - otherVec.x);
            float yDiff = Mathf.Abs(thisVec.y - otherVec.y);
            return xDiff <= marginOfError &&
                yDiff <= marginOfError;
        }

        public static Vector3 RandBetween(Vector3 firstVec, Vector3 secondVec)
        {
            float x = Random.Range(firstVec.x, secondVec.x),
                y = Random.Range(firstVec.y, secondVec.y),
                z = Random.Range(firstVec.z, secondVec.z);

            return new Vector3(x, y, z);
        }
    }
}