using UnityEngine;
using UnityEngine.UI;

public class GerenciadorDeNotas : MonoBehaviour
{
    public float posicaoAcerto = -3f;
    public float margemPerfeito = 0.2f;
    public float margemBom = 0.5f;
    
    public Text feedbackTexto;
    private float tempoFeedback = 0f;
    
    private SistemaDeJogo sistemaDeJogo;
    
    void Start()
    {
        sistemaDeJogo = GetComponent<SistemaDeJogo>();
    }
    
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
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            VerificarAcerto("NotaA");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            VerificarAcerto("NotaS");
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            VerificarAcerto("NotaJ");
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            VerificarAcerto("NotaK");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            VerificarAcerto("NotaL");
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
            NotaMovimento notaMov = notaMaisProxima.GetComponent<NotaMovimento>();
            
            if (melhorDistancia <= margemPerfeito)
            {
                MostrarFeedback("PERFEITO!", Color.yellow);
                if (sistemaDeJogo != null)
                {
                    sistemaDeJogo.AcertoPerfeito();
                }
            }
            else
            {
                MostrarFeedback("BOM!", Color.green);
                if (sistemaDeJogo != null)
                {
                    sistemaDeJogo.AcertoBom();
                }
            }
            
            if (notaMov != null)
            {
                notaMov.MarcarComoAcertada();
            }
            
            Destroy(notaMaisProxima);
        }
        else
        {
            MostrarFeedback("ERROU!", Color.red);
            if (sistemaDeJogo != null)
            {
                sistemaDeJogo.Errou();
            }
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