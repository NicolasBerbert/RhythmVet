using UnityEngine;
using UnityEngine.UI;

public class GerenciadorDeNotas : MonoBehaviour
{
    public float posicaoAcerto = -3f;
    public float margemPerfeito = 0.2f;
    public float margemBom = 0.5f;
    
    public Text feedbackTexto;
    private float tempoFeedback = 0f;
    
    void Update()
    {
        if (tempoFeedback > 0)
        {
            tempoFeedback -= Time.deltaTime;
            if (tempoFeedback <= 0)
            {
                feedbackTexto.text = "";
            }
        }
        
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            VerificarAcerto("NotaA");
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            VerificarAcerto("NotaS");
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            VerificarAcerto("NotaJ");
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            VerificarAcerto("NotaK");
        }
    }
    
    void VerificarAcerto(string tagNota)
    {
        GameObject[] notas = GameObject.FindGameObjectsWithTag(tagNota);
        
        float melhorDistancia = float.MaxValue;
        GameObject notaMaisProxima = null;
        
        foreach (GameObject nota in notas)
        {
            float distancia = Mathf.Abs(nota.transform.position.y - posicaoAcerto);
            
            if (distancia < melhorDistancia)
            {
                melhorDistancia = distancia;
                notaMaisProxima = nota;
            }
        }
        
        if (notaMaisProxima != null && melhorDistancia <= margemBom)
        {
            if (melhorDistancia <= margemPerfeito)
            {
                MostrarFeedback("PERFEITO!", Color.yellow);
            }
            else
            {
                MostrarFeedback("BOM!", Color.green);
            }
            
            Destroy(notaMaisProxima);
        }
        else
        {
            MostrarFeedback("ERROU!", Color.red);
        }
    }
    
    void MostrarFeedback(string mensagem, Color cor)
    {
        feedbackTexto.text = mensagem;
        feedbackTexto.color = cor;
        tempoFeedback = 0.5f;
        
        Debug.Log(mensagem);
    }
}