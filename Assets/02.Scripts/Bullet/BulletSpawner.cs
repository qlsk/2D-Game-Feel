using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    private float _spawnTimer = 0f;
    [SerializeField] private float _spawnTime;
    [SerializeField] private Bullet _bulletPrefab;
    private void Start()
    {
        Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
    }

    private void Update()
    {
        _spawnTimer += Time.deltaTime;
        if (_spawnTimer >= _spawnTime)
        {
            Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
            _spawnTimer = 0f;
        }
    }
}