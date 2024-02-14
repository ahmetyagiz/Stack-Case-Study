using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TransformerManager : MonoBehaviour
{
    [SerializeField] private GameObject referenceObj;
    [SerializeField] private Transform getSlotParent;
    [SerializeField] private Transform spawnSlotParent;

    //public int counter;
    public bool isGettingObjects;

    void Update()
    {
        // eger devam eden alýnma islemi yoksa
        if (!isGettingObjects)
        {
            for (int i = 0; i < getSlotParent.childCount; i++)
            {
                if (getSlotParent.GetChild(i).GetComponent<SlotChecker>().isSlotFull == true)
                {
                    Debug.Log("Makineye çekebileceðim obje var");
                    StartCoroutine(GetRoutine());

                    isGettingObjects = true;
                    break;
                }
            }
        }
    }

    Transform selectedObject;

    IEnumerator GetRoutine()
    {
        SlotChecker selectedSlotChecker = null;
        int counter = 0;

        while (true)
        {
            counter = getSlotParent.childCount - 1;

            for (int i = counter; i >= 0; i--)
            {
                if (getSlotParent.GetChild(i).GetComponent<SlotChecker>().isSlotFull == true)
                {
                    selectedSlotChecker = getSlotParent.GetChild(i).GetComponent<SlotChecker>();
                    if (getSlotParent.GetChild(i).GetChild(0) != null)
                    {
                        selectedObject = getSlotParent.GetChild(i).GetChild(0);
                    }
                    break;
                }
            }

            // eðer objenin devam eden bir tweeni yoksa
            if (selectedObject != null)
            {
                Invoke(nameof(SpawnObject), 1f);

                if (DOTween.IsTweening(selectedObject.transform) == false)
                {
                    selectedSlotChecker.isSlotFull = false;
                    selectedObject.SetParent(null);
                    selectedObject.transform.DOJump(transform.position, 1, 1, 0.25f).SetEase(Ease.Linear);
                }
            }

            if (getSlotParent.GetChild(0).GetComponent<SlotChecker>().isSlotFull == false || spawnSlotParent.GetChild(spawnSlotParent.childCount - 1).GetComponent<SlotChecker>().isSlotFull == true)
            {
                isGettingObjects = false;

                break;
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    void SpawnObject()
    {
        for (int i = 0; i < spawnSlotParent.childCount; i++)
        {
            if (spawnSlotParent.GetChild(i).GetComponent<SlotChecker>().isSlotFull == false)
            {
                SlotChecker selectedSlotChecker = spawnSlotParent.GetChild(i).GetComponent<SlotChecker>();

                GameObject spawnedObj = Instantiate(referenceObj, transform.position, referenceObj.transform.rotation, spawnSlotParent.GetChild(i));
                spawnedObj.transform.DOJump(spawnSlotParent.GetChild(i).position, 1, 1, 0.5f).SetEase(Ease.Linear);
                selectedSlotChecker.isSlotFull = true;
                break;
            }
        }
    }
}