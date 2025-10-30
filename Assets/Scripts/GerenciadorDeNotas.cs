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
    
    private List<string> inputsDoFrame = new List<string>();
    
    void Start()
    {
        sistemaDeJogo = GetComponent<SistemaDeJogo>();
    }
    
    void Update()
    {
        // Coleta TODOS os inputs do frame
        inputsDoFrame.Clear();
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            inputsDoFrame.Add("NotaA");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            inputsDoFrame.Add("NotaS");
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            inputsDoFrame.Add("NotaJ");
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            inputsDoFrame.Add("NotaK");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            inputsDoFrame.Add("NotaL");
        }
        
        // Processa TODOS os inputs de uma vez
        if (inputsDoFrame.Count > 0)
        {
            ProcessarInputs();
        }
        
        // Atualiza o feedback
        if (tempoFeedback > 0)
        {
            tempoFeedback -= Time.deltaTime;
            if (tempoFeedback <= 0)
            {
                feedbackTexto.text = "";
            }
        }
    }
    
    void ProcessarInputs()
    {
        List<string> resultados = new List<string>();
        List<GameObject> notasParaDestruir = new List<GameObject>();
        
        // Para cada input, encontra a melhor nota correspondente
        foreach (string tagNota in inputsDoFrame)
        {
            GameObject[] notas = GameObject.FindGameObjectsWithTag(tagNota);
            
            if (notas.Length == 0)
            {
                // Nenhuma nota dessa tecla na tela
                resultados.Add("ERROU");
                if (sistemaDeJogo != null)
                {
                    sistemaDeJogo.Errou();
                }
                continue;
            }
            
            float melhorDistancia = float.MaxValue;
            GameObject notaMaisProxima = null;
            
            // Encontra a nota mais próxima (que não está marcada para destruição)
            foreach (GameObject nota in notas)
            {
                if (notasParaDestruir.Contains(nota)) continue; // Pula notas já processadas
                
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
                NotaMovimento notaMov = notaMaisProxima.GetComponent<NotaMovimento>();
                
                if (melhorDistancia <= margemPerfeito)
                {
                    resultados.Add("PERFEITO");
                    if (sistemaDeJogo != null)
                    {
                        sistemaDeJogo.AcertoPerfeito();
                    }
                }
                else
                {
                    resultados.Add("BOM");
                    if (sistemaDeJogo != null)
                    {
                        sistemaDeJogo.AcertoBom();
                    }
                }
                
                if (notaMov != null)
                {
                    notaMov.MarcarComoAcertada();
                }
                
                // Marca para destruir depois
                notasParaDestruir.Add(notaMaisProxima);
            }
            else
            {
                resultados.Add("ERROU");
                if (sistemaDeJogo != null)
                {
                    sistemaDeJogo.Errou();
                }
            }
        }
        
        // Destroi todas as notas marcadas DE UMA VEZ
        foreach (GameObject nota in notasParaDestruir)
        {
            Destroy(nota);
        }
        
        // Mostra o feedback combinado
        MostrarFeedbackCombinado(resultados);
    }
    
    void MostrarFeedbackCombinado(List<string> resultados)
    {
        if (resultados.Count == 0) return;
        
        // Conta os tipos de resultado
        int perfeitos = 0;
        int bons = 0;
        int erros = 0;
        
        foreach (string resultado in resultados)
        {
            if (resultado == "PERFEITO") perfeitos++;
            else if (resultado == "BOM") bons++;
            else if (resultado == "ERROU") erros++;
        }
        
        // Monta a mensagem
        string mensagem = "";
        Color cor = Color.white;
        
        if (resultados.Count > 1)
        {
            // Múltiplos inputs
            if (erros == 0 && perfeitos > 0)
            {
                mensagem = "PERFEITO x" + resultados.Count + "!";
                cor = Color.yellow;
            }
            else if (erros == 0 && bons > 0)
            {
                mensagem = "BOM x" + resultados.Count + "!";
                cor = Color.green;
            }
            else if (perfeitos > 0 || bons > 0)
            {
                int acertos = perfeitos + bons;
                mensagem = string.Format("{0}/{1} ACERTOS!", acertos, resultados.Count);
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
        if (feedbackTexto != null)
        {
            feedbackTexto.text = mensagem;
            feedbackTexto.color = cor;
            tempoFeedback = 0.5f;
        }
        
        Debug.Log(mensagem + " (Total inputs: " + inputsDoFrame.Count + ")");
    }
}
