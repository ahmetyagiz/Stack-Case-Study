using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class CollectManager : MonoBehaviour
{
    [SerializeField] private bool inArea;

    [SerializeField] private List<Transform> backpackTransforms = new List<Transform>();

    [Header("Collected Object")]
    private Transform selectedBackpackTransform;
    private Transform selectedAreaSlotTransform;
    private CollectableMovement selectedCollectableObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CollectArea"))
        {
            StartCoroutine(CollectRoutine(other.transform));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CollectArea"))
        {
            StopCoroutine(CollectRoutine(other.transform));
            inArea = false;
        }
    }

    IEnumerator CollectRoutine(Transform collectAreaParent)
    {
        inArea = true;

        while (true)
        {
            // collect area leave check "OR" is backpack full, break
            if (inArea == false || backpackTransforms[backpackTransforms.Count - 1].GetComponent<SlotChecker>().isSlotFull == true)
            {
                break;
            }

            // backpack emptiness check
            for (int i = 0; i < backpackTransforms.Count; i++)
            {
                if (backpackTransforms[i].GetComponent<SlotChecker>().isSlotFull == false)
                {
                    selectedBackpackTransform = backpackTransforms[i];
                    backpackTransforms[i].GetComponent<SlotChecker>().isSlotFull = true;
                    break;
                }
            }

            yield return new WaitForSeconds(0.075f);

            // collect area existence check
            for (int i = collectAreaParent.childCount - 1; i > 0; i--)
            {
                if (collectAreaParent.GetChild(i).GetComponent<SlotChecker>().isSlotFull == true)
                {
                    // selected slot
                    selectedAreaSlotTransform = collectAreaParent.GetChild(i);
                    // slot's object
                    selectedCollectableObject = collectAreaParent.GetChild(i).GetChild(0).GetComponent<CollectableMovement>();

                    // clean slot 
                    selectedAreaSlotTransform.GetComponent<SlotChecker>().isSlotFull = false;

                    // assign backpack transform
                    selectedCollectableObject.selectedTransform = selectedBackpackTransform;

                    // leave the slot
                    selectedCollectableObject.transform.parent = null;

                    // example: brick move
                    selectedCollectableObject.JumpToSelectedTransform();

                    break;
                }
            }

            yield return new WaitForSeconds(0.075f);
        }
    }


}

//IEnumerator CollectRoutine()
// {
//for (int i = 0; i < carryManager.childCount; i++)
//{
//    // eðer elim boþsa
//    if (carryManager.GetChild(i).GetComponent<SlotChecker>().isSlotFull == false)
//    {
//        if (inArea == false)
//        {
//            break;
//        }
//        carryManager.GetChild(i).GetComponent<SlotChecker>().isSlotFull = true;
//        collectAreaParent.GetChild(i).GetChild(0).GetComponent<CollectableMovement>().isMoving = true;

//        verticalOffset += 0.3f;
//        collectAreaParent.GetChild(i).GetChild(0).GetComponent<CollectableMovement>().myVerticalOffset = verticalOffset;

//        if (i == 0)
//        {
//            continue;
//        }
//        collectAreaParent.GetChild(i).GetChild(0).GetComponent<CollectableMovement>().previousPosition = carryManager.GetChild(i - 1).GetChild(0).transform.position;

//        collectAreaParent.GetChild(i).GetComponent<SlotChecker>().isSlotFull = false;
//        //i = 0;
//        yield return new WaitForSeconds(0.25f);
//    }
//}
// }
