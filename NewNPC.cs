using System.Collections;
using UnityEngine;

public class NPC_Behavior : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float idleTime = 5f;
    [SerializeField] private float moveTime = 5f;
    [SerializeField] private float rotationSpeed = 5f;

    private Transform tr;

    void Start()
    {
        animator = GetComponent<Animator>();
        tr = GetComponent<Transform>();
        StartCoroutine(Movement());
    }

    private IEnumerator Movement()
    {
        while (true)
        {
            if (Random.value < 0.5f)
            {
                animator.SetTrigger("IdleTrig");
                yield return Idle();
            }
            else
            {
                animator.SetTrigger("WalkTrig");
                Vector3 randomDir = GetRandomDir();
                yield return MoveInDir(randomDir);
            }
            yield return null;
        }
    }
    private Vector3 GetRandomDir()
    {
        float randomAngle = Random.Range(0f, 360f);
        return new Vector3(Mathf.Cos(randomAngle), 0f, Mathf.Sin(randomAngle)).normalized;
    }

    private IEnumerator MoveInDir(Vector3 direction)
    {
        float elapsedTime = 0f;

        while (elapsedTime < moveTime)
        {
            tr.position += direction * moveSpeed * Time.deltaTime;
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                tr.rotation = Quaternion.Slerp(tr.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator Idle()
    {
        yield return new WaitForSeconds(idleTime);
    }
}
