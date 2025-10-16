using UnityEngine;

public class GerenciadorDeNotas : MonoBehaviour
{
    public float posicaoAcerto = -3f; // Posição Y da HitLine
    public float margemPerfeito = 0.2f; // Distância para acerto perfeito
    public float margemBom = 0.5f; // Distância para acerto bom
    
    void Update()
    {
        // Detecta as setas do teclado
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            VerificarAcerto("NotaA"); // Seta Esquerda
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            VerificarAcerto("NotaS"); // Seta Baixo
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            VerificarAcerto("NotaJ"); // Seta Cima
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            VerificarAcerto("NotaK"); // Seta Direita
        }
    }
    
    void VerificarAcerto(string tagNota)
    {
        // Procura todas as notas com essa tag
        GameObject[] notas = GameObject.FindGameObjectsWithTag(tagNota);
        
        float melhorDistancia = float.MaxValue;
        GameObject notaMaisProxima = null;
        
        // Encontra a nota mais próxima da linha
        foreach (GameObject nota in notas)
        {
            float distancia = Mathf.Abs(nota.transform.position.y - posicaoAcerto);
            
            if (distancia < melhorDistancia)
            {
                melhorDistancia = distancia;
                notaMaisProxima = nota;
            }
        }
        
        // Verifica se acertou
        if (notaMaisProxima != null && melhorDistancia <= margemBom)
        {
            if (melhorDistancia <= margemPerfeito)
            {
                Debug.Log("PERFEITO! ⭐");
            }
            else
            {
                Debug.Log("BOM! ✓");
            }
            
            Destroy(notaMaisProxima);
        }
        else
        {
            Debug.Log("ERROU! ✗");
        }
    }
}