using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Final_NPC_Movement : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float idleTime = 3f;
    [SerializeField] private float moveTime = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float stoppingDistance = 20f;

    private Transform tr;
    private Transform closestCar;
    void Start()
    {
        animator = GetComponent<Animator>();
        tr = GetComponent<Transform>();
        StartCoroutine(Movement());
    }
    private void FixedUpdate() 
    {
        if (closestCar != null)
        {
            MoveAwayFromCar();
        }
    }

    private void MoveAwayFromCar()
    {
        Vector3 NPCPosition = transform.position;
        Vector3 carPosition = closestCar.position;
        Vector3 direction = NPCPosition - carPosition;
        direction.y = 0;
        float distance = direction.magnitude;
        if (distance < stoppingDistance)
        {
            direction.Normalize();
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            transform.position += direction * runSpeed * Time.deltaTime;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "car" || other.gameObject.tag == "Car")
        {
            animator.SetTrigger("RunTrig");
            closestCar = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Car") || other.CompareTag("Car"))
        {
            animator.SetTrigger("IdleTrig");
            closestCar = null;
        }
    }
    private IEnumerator Movement()
    {
        while (true)
        {
            if (Random.value < 0.5f && closestCar == null)
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
        if (closestCar != null){
            yield break;
        }
    }
}
