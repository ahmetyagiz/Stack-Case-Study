using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField] private GameObject referenceObj;
    [SerializeField] private Transform slotParent;
    private int counter;
    private SlotChecker selectedSlotChecker;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (counter < slotParent.childCount)
        {
            if (slotParent.GetChild(counter).GetComponent<SlotChecker>().isSlotFull == false)
            {
                selectedSlotChecker = slotParent.GetChild(counter).GetComponent<SlotChecker>();

                GameObject spawnedObj = Instantiate(referenceObj, transform.position, Quaternion.identity, slotParent.GetChild(counter));
                spawnedObj.transform.DOJump(slotParent.GetChild(counter).position, 1, 1, 0.25f).OnComplete(() =>
                {
                    counter = 0;
                    selectedSlotChecker.isSlotFull = true;
                });

                yield return new WaitForSeconds(0.26f);
            }
            else
            {
                counter++;
            }


            if (counter == slotParent.childCount)
            {
                counter = 0;
            }

            // inf loop protect
            yield return new WaitForEndOfFrame();
        }
    }
}