using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CollectManager : MonoBehaviour
{
    [SerializeField] private bool inCollectArea;
    [SerializeField] private bool inGiveArea;
    [SerializeField] private bool inTrashArea;
    [SerializeField] private Transform collectedObjectsParent;
    [SerializeField] private List<Transform> backpackTransforms = new List<Transform>();

    [Header("Collected Object")]
    private Transform selectedBackpackTransform;
    private Transform selectedAreaSlotTransform;
    private CollectableMovement selectedCollectableObject;
    public float almaAraligi;
    // Giving

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

        if (other.CompareTag("TrashCan"))
        {
            StartCoroutine(TrashRoutine(other.transform));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CollectArea"))
        {
            StopCoroutine(CollectRoutine(other.transform));
            inCollectArea = false;
        }

        if (other.CompareTag("GiveArea"))
        {
            StopCoroutine(GiveRoutine(other.transform));
            inGiveArea = false;
        }

        if (other.CompareTag("TrashCan"))
        {
            StopCoroutine(TrashRoutine(other.transform));
            inTrashArea = false;
        }
    }

    #endregion

    #region Collect Object
    IEnumerator CollectRoutine(Transform collectAreaParent)
    {
        inCollectArea = true;

        while (true)
        {
            // if leave collect area "OR" if backpack full, break
            if (inCollectArea == false || backpackTransforms[backpackTransforms.Count - 1].GetComponent<SlotChecker>().isSlotFull == true)
            {
                break;
            }

            // backpack emptiness check
            for (int i = 0; i < backpackTransforms.Count; i++)
            {
                if (backpackTransforms[i].GetComponent<SlotChecker>().isSlotFull == false)
                {
                    selectedBackpackTransform = backpackTransforms[i];
                    
                    break;
                }
            }

            // collect area existence check
            for (int i = collectAreaParent.childCount - 1; i >= 0; i--)
            {
                if (collectAreaParent.GetChild(i).GetComponent<SlotChecker>().isSlotFull == true)
                {
                    // selected slot
                    selectedAreaSlotTransform = collectAreaParent.GetChild(i);
                    // slot's object
                    selectedCollectableObject = collectAreaParent.GetChild(i).GetChild(0).GetComponent<CollectableMovement>();

                    // clean collect area slot
                    selectedAreaSlotTransform.GetComponent<SlotChecker>().isSlotFull = false;

                    // assign backpack transform
                    selectedCollectableObject.selectedTransform = selectedBackpackTransform;

                    // set new parent
                    selectedCollectableObject.transform.SetParent(collectedObjectsParent);

                    // example: brick move
                    selectedCollectableObject.JumpToSelectedTransform();
                    selectedBackpackTransform.GetComponent<SlotChecker>().isSlotFull = true;

                    break;
                }
            }

            yield return new WaitForSeconds(almaAraligi);
        }
    }

    #endregion

    #region Give Object

    IEnumerator GiveRoutine(Transform giveAreaParent)
    {
        inGiveArea = true;
        Transform selectedGiveObject;
        Transform selectedGiveAreaSlot = null;

        while (true)
        {
            // if leave give area "OR" if backpack empty "OR" if give area full, break
            if (inGiveArea == false || collectedObjectsParent.childCount == 0 || giveAreaParent.GetChild(giveAreaParent.childCount-1).GetComponent<SlotChecker>().isSlotFull == true)
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
                backpackTransforms[i].GetComponent<SlotChecker>().isSlotFull = false;
                selectedGiveObject = collectedObjectsParent.GetChild(i);

                if (!selectedGiveObject.CompareTag("Brick"))
                {
                    break;
                }

                Destroy(selectedGiveObject.GetComponent<CollectableMovement>());

                selectedGiveObject.DORotateQuaternion(Quaternion.identity, 0.5f);
                selectedGiveObject.DOJump(selectedGiveAreaSlot.position, 1, 1, 0.5f).OnComplete(() =>
                {
                    selectedGiveObject.SetPositionAndRotation(selectedGiveAreaSlot.position, Quaternion.identity);
                });

                selectedGiveObject.SetParent(selectedGiveAreaSlot);

                break;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    #endregion

    #region Give Trash Object

    IEnumerator TrashRoutine(Transform giveAreaParent)
    {
        inTrashArea = true;

        while (true)
        {
            // if leave give area, break
            if (inTrashArea == false)
            {
                break;
            }

            if (collectedObjectsParent.childCount > 0)
            {
                // give slot emptiness check
                for (int i = collectedObjectsParent.childCount - 1; i >= 0; i--)
                {
                    if (backpackTransforms[i].GetComponent<SlotChecker>().isSlotFull == true)
                    {
                        backpackTransforms[i].GetComponent<SlotChecker>().isSlotFull = false;
                        Transform selectedObject = collectedObjectsParent.GetChild(i);

                        Destroy(selectedObject.GetComponent<CollectableMovement>());

                        selectedObject.SetParent(null);
                        selectedObject.DORotateQuaternion(Quaternion.identity, 0.3f);
                        selectedObject.DOJump(giveAreaParent.parent.position, 1, 1, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
                        {
                            selectedObject.gameObject.SetActive(false);
                        });
                        break;
                    }
                }
            }
            yield return new WaitForSeconds(0.15f);
        }
    }

    #endregion
}