using UnityEngine;

namespace GameMechanics.Behaviors
{
    public class TurnBehavior
    {
        public Quaternion GetRotationRelativeToAnotherObject(Vector3 positionRotatedObject, Vector3 positionRelativeToAnotherObject)
        {
            Vector3 diff = positionRelativeToAnotherObject - positionRotatedObject;
            diff.Normalize();
            float rotZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            
            return Quaternion.Euler(0f, 0f, rotZ - 90);
        }
    }
}
