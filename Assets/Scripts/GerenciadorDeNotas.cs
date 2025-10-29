using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GerenciadorDeNotas : MonoBehaviour
{
    public float posicaoAcerto = -3f;
    public float margemPerfeito = 0.2f;
    public float margemBom = 0.5f;
    
    public Text feedbackTexto;
    private float tempoFeedback = 0f;
    
    private SistemaDeJogo sistemaDeJogo;
    
    // NOVO - Sistema de feedback acumulado
    private List<string> feedbacksAcumulados = new List<string>();
    private float tempoUltimoInput = 0f;
    private float janelaCombinacao = 0.05f; // 50ms para considerar inputs "simultâneos"
    
    void Start()
    {
        sistemaDeJogo = GetComponent<SistemaDeJogo>();
    }
    
    void Update()
    {
        // Limpa feedbacks acumulados se passou tempo suficiente
        if (Time.time - tempoUltimoInput > janelaCombinacao && feedbacksAcumulados.Count > 0)
        {
            MostrarFeedbackCombinado();
            feedbacksAcumulados.Clear();
        }
        
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
        tempoUltimoInput = Time.time;
        
        GameObject[] notas = GameObject.FindGameObjectsWithTag(tagNota);
        
        if (notas.Length == 0)
        {
            feedbacksAcumulados.Add("ERROU");
            if (sistemaDeJogo != null)
            {
                sistemaDeJogo.Errou();
            }
            return;
        }
        
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
                feedbacksAcumulados.Add("PERFEITO");
                if (sistemaDeJogo != null)
                {
                    sistemaDeJogo.AcertoPerfeito();
                }
            }
            else
            {
                feedbacksAcumulados.Add("BOM");
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
            feedbacksAcumulados.Add("ERROU");
            if (sistemaDeJogo != null)
            {
                sistemaDeJogo.Errou();
            }
        }
    }
    
    void MostrarFeedbackCombinado()
    {
        if (feedbacksAcumulados.Count == 0) return;
        
        // Conta quantos de cada tipo
        int perfeitos = 0;
        int bons = 0;
        int erros = 0;
        
        foreach (string feedback in feedbacksAcumulados)
        {
            if (feedback == "PERFEITO") perfeitos++;
            else if (feedback == "BOM") bons++;
            else if (feedback == "ERROU") erros++;
        }
        
        // Monta a mensagem
        string mensagem = "";
        Color cor = Color.white;
        
        if (feedbacksAcumulados.Count > 1)
        {
            // Múltiplos inputs simultâneos
            if (erros == 0 && perfeitos > 0)
            {
                mensagem = "PERFEITO x" + feedbacksAcumulados.Count + "!";
                cor = Color.yellow;
            }
            else if (erros == 0 && bons > 0)
            {
                mensagem = "BOM x" + feedbacksAcumulados.Count + "!";
                cor = Color.green;
            }
            else if (perfeitos > 0 || bons > 0)
            {
                mensagem = string.Format("{0} ACERTOS!", (perfeitos + bons));
                cor = Color.cyan;
            }
            else
            {
                mensagem = "ERROU x" + erros;
                cor = Color.red;
            }
        }
        else
        {
            // Um único input
            if (perfeitos > 0)
            {
                mensagem = "PERFEITO!";
                cor = Color.yellow;
            }
            else if (bons > 0)
            {
                mensagem = "BOM!";
                cor = Color.green;
            }
            else
            {
                mensagem = "ERROU!";
                cor = Color.red;
            }
        }
        
        MostrarFeedback(mensagem, cor);
    }
    
    void MostrarFeedback(string mensagem, Color cor)
    {
        feedbackTexto.text = mensagem;
        feedbackTexto.color = cor;
        tempoFeedback = 0.5f;
        
        Debug.Log(mensagem);
    }
}