//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DataInfo;

public class Drop : MonoBehaviour,IDragHandler
{
    public void OnDrag(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            Drag.draggingItem.transform.SetParent(this.transform);
            Item item = Drag.draggingItem.GetComponent<ItemInfo>().itemData;
            GameMgr.instance.AddItem(item);
        }
    }

   
}
