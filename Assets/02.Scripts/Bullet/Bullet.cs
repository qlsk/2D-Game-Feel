using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;

    private void Start()
    {
    }

    private void Update()
    {
        transform.Translate(_moveSpeed * Time.deltaTime * Vector3.left);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!other.gameObject.CompareTag("Player"))
            {
                return;
            }

            if (other.gameObject.TryGetComponent(out PlayerHit playerHit))
            {
                playerHit.Hit();
            }
            Destroy(gameObject);
        }
    }
}