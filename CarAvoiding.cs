using UnityEngine;

public class CarAvoiding : MonoBehaviour
{
    public float NPCSpeed = 3f;
    public float stoppingDistance = 2f;

    private Transform closestCar;

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
            transform.position += direction * NPCSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "car")
        {
            closestCar = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Car") || other.CompareTag("Car"))
        {
            closestCar = null;
        }
    }
}
