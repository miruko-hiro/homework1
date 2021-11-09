using UnityEngine;

namespace UI
{
    public class TranslatorWorldToScreen
    {
        public Vector3 WorldToScreenSpace(Vector2 worldPos, Camera cam, RectTransform area)
        {
            Vector2 screenPoint = cam.WorldToScreenPoint(worldPos);
 
            Vector2 screenPos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(area, screenPoint, cam, out screenPos))
            {
                return screenPos;
            }
 
            return screenPoint;
        }
    }
}