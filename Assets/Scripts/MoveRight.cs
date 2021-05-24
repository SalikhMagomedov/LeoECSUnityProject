using UnityEngine;

public class MoveRight : MonoBehaviour
{
    [SerializeField] private float speed;

    private void Update()
    {
        transform.Translate(Vector3.right * (speed * Time.deltaTime));
    }
}