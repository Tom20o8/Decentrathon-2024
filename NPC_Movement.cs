using System.Collections;
using UnityEngine;

public class NPC_Movement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float moveDistance = 5f;

    private Transform tr;
    private Vector3 startPos;
    private Vector3 targetPos;
    private Vector3 startRot;
    private Vector3 targetRot;
    private bool movingOneDir = true;
    private Animator animator_;

    void Start()
    {
        animator_ = GetComponent<Animator>();
        tr = GetComponent<Transform>();
        startRot = tr.eulerAngles;
        startPos = tr.position;
        targetPos = startPos + tr.forward * moveDistance;
        targetRot = startRot + new Vector3(0f, 180f, 0f);
        StartCoroutine(Movement());
        animator_.SetTrigger("WalkTrig");
    }

    private IEnumerator Movement()
    {
        while (true)
        {
            while (Vector3.Distance(tr.position, targetPos) > 0.1f)
            {
                tr.position = Vector3.MoveTowards(tr.position, targetPos, moveSpeed * Time.deltaTime);
                yield return null;
            }
            
            movingOneDir = !movingOneDir;
            if (movingOneDir)
            {
                tr.rotation = Quaternion.Euler(startRot);
                targetPos = startPos + tr.forward * moveDistance;
            }
            else
            {
                tr.rotation = Quaternion.Euler(targetRot);
                targetPos = startPos;
            }
        }
    }
}
