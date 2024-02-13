using DG.Tweening;
using UnityEngine;

public class CollectableMovement : MonoBehaviour
{
    private bool isFollowing;
    private bool isLooking;
    private GameObject player;
    public Transform selectedTransform;

    void Update()
    {
        FollowToSelectedTransform();
        LookToSelectedTransform();
    }

    public void JumpToSelectedTransform()
    {
        // playera bakmak icin erisim
        player = GameObject.FindGameObjectWithTag("Player");

        // oyuncuya dogru bakiyor
        isLooking = true;

        // ziplama ve bitince takibe basliyor
        transform.DOJump(selectedTransform.transform.position, 1, 1, 0.15f).OnComplete(() =>
        {
            isFollowing = true;
        });
    }

    void FollowToSelectedTransform()
    {
        if (isFollowing)
        {
            // move
            selectedTransform.transform.position = new Vector3(selectedTransform.position.x, selectedTransform.position.y, selectedTransform.position.z);
            transform.position = Vector3.Lerp(transform.position, selectedTransform.transform.position, Time.deltaTime * 30);
        }
    }

    void LookToSelectedTransform()
    {
        if (isLooking)
        {
            // Look at the player
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
        }
    }

    //void CollectedObjectToPlayer() // dalga hareketi vs.
    //{
    //    Interpolate between the previous position and the target position
    //   Vector3 newPosition = Vector3.Lerp(transform.position, previousPosition, Time.deltaTime * mySpeed);

    //    Update the position of the object
    //    transform.position = newPosition;

    //    Store the current position as the previous position for the next frame

    //   previousPosition = transform.position;

    //    Look at the player
    //    transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
    //}
}