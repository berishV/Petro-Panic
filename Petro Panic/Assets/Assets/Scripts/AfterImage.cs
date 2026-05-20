using UnityEngine;
public class AfterImage : MonoBehaviour
{
    public GameObject ghostPrefab;
    public float spawnRate = 0.05f;
    private float timer;

    void Update()
    {
        float movimientoX = Input.GetAxisRaw("Horizontal");
        float movimientoY = Input.GetAxisRaw("Vertical");

        if (movimientoX != 0 || movimientoY != 0)
        {
            timer += Time.deltaTime;

            if (timer >= spawnRate)
            {
                timer = 0;

                Instantiate(ghostPrefab, transform.position, transform.rotation);
            }
        }
    }
}