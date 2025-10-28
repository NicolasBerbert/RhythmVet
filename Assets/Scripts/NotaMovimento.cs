using UnityEngine;

public class NotaMovimento : MonoBehaviour
{
    public float velocidade = 3f;
    private bool foiAcertada = false;
    
    void Update()
    {
        transform.position += Vector3.down * velocidade * Time.deltaTime;
        
        // Se passou da linha e não foi acertada
        if (transform.position.y < -4f && !foiAcertada)
        {
            NotaPerdida();
        }
    }
    
    void NotaPerdida()
    {
        // Busca o sistema de jogo
        SistemaDeJogo sistema = FindObjectOfType<SistemaDeJogo>();
        
        if (sistema != null)
        {
            sistema.NotaPerdida();
        }
        
        Destroy(gameObject);
    }
    
    public void MarcarComoAcertada()
    {
        foiAcertada = true;
    }
}