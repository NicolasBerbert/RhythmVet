using UnityEngine;

public class NotaMovimento : MonoBehaviour
{
    public float velocidade = 3f;
    
    void Update()
    {
        // Move a nota para baixo
        transform.position += Vector3.down * velocidade * Time.deltaTime;
        
        // Destroi a nota se passar muito da linha
        if (transform.position.y < -5f)
        {
            Destroy(gameObject);
        }
    }
}