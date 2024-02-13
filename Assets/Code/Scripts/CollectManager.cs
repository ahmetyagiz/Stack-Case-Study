using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CollectManager : MonoBehaviour
{
    [SerializeField] private bool inArea;
    [SerializeField] private Transform collectedObjectsParent;
    [SerializeField] private List<Transform> backpackTransforms = new List<Transform>();

    [Header("Collected Object")]
    private Transform selectedBackpackTransform;
    private Transform selectedAreaSlotTransform;
    private CollectableMovement selectedCollectableObject;

    // Giving
    private Transform selectedGiveObject;
    private Transform selectedGiveAreaSlot;

    #region OnTriggerEnter OnTriggerExit
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CollectArea"))
        {
            StartCoroutine(CollectRoutine(other.transform));
        }

        if (other.CompareTag("GiveArea"))
        {
            StartCoroutine(GiveRoutine(other.transform));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CollectArea"))
        {
            StopCoroutine(CollectRoutine(other.transform));
            inArea = false;
        }

        if (other.CompareTag("GiveArea"))
        {
            StartCoroutine(GiveRoutine(other.transform));
            inArea = false;
        }
    }

    #endregion

    #region Collect Object
    IEnumerator CollectRoutine(Transform collectAreaParent)
    {
        inArea = true;

        while (true)
        {
            // if leave collect area "OR" if backpack full, break
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
                    selectedBackpackTransform.GetComponent<SlotChecker>().isSlotFull = true;
                    break;
                }
            }

            yield return new WaitForSeconds(0.1f);

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

                    // set new parent
                    selectedCollectableObject.transform.SetParent(collectedObjectsParent);

                    // example: brick move
                    selectedCollectableObject.JumpToSelectedTransform();

                    break;
                }
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    #endregion

    #region Give Object

    IEnumerator GiveRoutine(Transform giveAreaParent)
    {
        inArea = true;

        while (true)
        {
            // if leave give area "OR" if backpack empty "OR" if give area full, break
            if (inArea == false || collectedObjectsParent.childCount == 0 || giveAreaParent.GetChild(giveAreaParent.childCount-1).GetComponent<SlotChecker>().isSlotFull == true)
            {
                break;
            }

            // give slot emptiness check
            for (int i = 0; i < giveAreaParent.childCount; i++)
            {
                if (giveAreaParent.GetChild(i).GetComponent<SlotChecker>().isSlotFull == false)
                {
                    selectedGiveAreaSlot = giveAreaParent.transform.GetChild(i);
                    selectedGiveAreaSlot.GetComponent<SlotChecker>().isSlotFull = true;
                    
                    break;
                }
            }

            for (int i = collectedObjectsParent.childCount - 1; i >= 0; i--)
            {
                // clean backpack slot
                backpackTransforms[i].GetComponent<SlotChecker>().isSlotFull = false;

                selectedGiveObject = collectedObjectsParent.GetChild(i);
                Destroy(selectedGiveObject.GetComponent<CollectableMovement>());

                selectedGiveObject.SetParent(null);

                selectedGiveObject.DORotateQuaternion(Quaternion.identity, 0.5f);
                selectedGiveObject.DOJump(selectedGiveAreaSlot.position, 1, 1, 0.5f).OnComplete(() =>
                {
                    selectedGiveObject.SetPositionAndRotation(selectedGiveAreaSlot.position, Quaternion.identity);
                });
                yield return new WaitForSeconds(0.2f);
                break;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    #endregion
}