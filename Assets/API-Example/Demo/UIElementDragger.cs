using UnityEngine;
using UnityEngine.EventSystems;

namespace agora_utilities
{
    public class UIElementDragger : EventTrigger
    {
        float lastClick = 0f;
        float interval = 0.4f;
        public override void OnPointerClick(PointerEventData eventData)
        {
            if ((lastClick + interval) > Time.time)
            {   //is a double click 

                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            else
            {//is a single click 
                lastClick = Time.time;
            }
        }

        public override void OnDrag(PointerEventData eventData)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            base.OnDrag(eventData);
        }
    }
}