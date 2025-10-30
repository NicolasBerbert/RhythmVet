using UnityEngine;
using System.Collections.Generic;

public class ChartLoader : MonoBehaviour
{
    public GameObject notaPrefab;
    public float alturaSpawn = 6f;
    public AudioSource audioSource;
    
    // NOVO - Calcula automaticamente baseado na velocidade da nota
    private float tempoDeQueda = 3f; // Quanto tempo a nota leva para cair
    
    private ChartData chartAtual;
    private List<NotaData> notasRestantes = new List<NotaData>();
    private bool chartCarregado = false;
    private float tempoInicio;
    public float offsetTiming = 0f; 
    private float[] posicoesX = new float[] { -3f, -1.5f, 0f, 1.5f, 3f };
    private Dictionary<string, string> teclasParaTags = new Dictionary<string, string>()
    {
        { "A", "NotaA" },
        { "S", "NotaS" },
        { "J", "NotaJ" },
        { "K", "NotaK" },
        { "L", "NotaL" }
    };
    private Dictionary<string, int> teclasParaIndices = new Dictionary<string, int>()
    {
        { "A", 0 },
        { "S", 1 },
        { "J", 2 },
        { "K", 3 },
        { "L", 4 }
    };
    private Color[] cores = new Color[] { Color.red, Color.blue, Color.green, Color.yellow, Color.magenta };
    
    void Start()
{
    CalcularTempoDeQueda();
    
    // MODIFICADO: Carrega a fase selecionada
    GerenciadorDeFases gerenciador = FindObjectOfType<GerenciadorDeFases>();
    
    string faseParaCarregar = "Fase1"; // Padrão
    
    if (gerenciador != null)
    {
        faseParaCarregar = gerenciador.faseAtual;
    }
    
    CarregarChart(faseParaCarregar);
}
    
    void CalcularTempoDeQueda()
    {
        // Pega a velocidade do prefab
        if (notaPrefab != null)
        {
            NotaMovimento movimento = notaPrefab.GetComponent<NotaMovimento>();
            if (movimento != null)
            {
                // Distância da spawn até a linha de acerto
                float distancia = alturaSpawn - (-3f); // alturaSpawn até a hitline em Y=-3
                tempoDeQueda = distancia / movimento.velocidade;
                
                Debug.Log("Tempo de queda calculado: " + tempoDeQueda.ToString("F2") + " segundos");
            }
        }
    }
    
    public void CarregarChart(string nomeFase)
    {
        TextAsset chartJson = Resources.Load<TextAsset>("Charts/" + nomeFase);
        
        if (chartJson == null)
        {
            Debug.LogError("Chart não encontrado: " + nomeFase);
            return;
        }
        
        chartAtual = JsonUtility.FromJson<ChartData>(chartJson.text);
        
        Debug.Log("Chart carregado: " + chartAtual.nomeFase + " com " + chartAtual.notas.Count + " notas");
        
        notasRestantes = new List<NotaData>(chartAtual.notas);
        
        if (!string.IsNullOrEmpty(chartAtual.nomeMusica))
        {
            AudioClip musica = Resources.Load<AudioClip>("Music/" + chartAtual.nomeMusica);
            
            if (musica != null && audioSource != null)
            {
                audioSource.clip = musica;
                audioSource.Play();
                Debug.Log("Música iniciada: " + chartAtual.nomeMusica);
            }
        }
        
        SistemaDeJogo sistema = GetComponent<SistemaDeJogo>();
        if (sistema != null)
        {
            sistema.duracaoFase = chartAtual.duracaoTotal;
        }
        
        tempoInicio = Time.time;
        chartCarregado = true;
    }
    
    void Update()
    {
        if (!chartCarregado || notasRestantes.Count == 0)
        {
            return;
        }
        
        float tempoAtual = Time.time - tempoInicio;
        
        // CORRIGIDO: Spawnamos com antecedência igual ao tempo de queda
        // Assim a nota chega EXATAMENTE no momento que o jogador clicou
        
        for (int i = notasRestantes.Count - 1; i >= 0; i--)
        {
            NotaData nota = notasRestantes[i];
            
            // Spawna quando: tempo atual >= (tempo da nota - tempo de queda)
            if (tempoAtual >= nota.tempo - tempoDeQueda + offsetTiming)
            {
                SpawnarNota(nota);
                notasRestantes.RemoveAt(i);
            }
        }
    }
    
    void SpawnarNota(NotaData nota)
{
    if (!teclasParaIndices.ContainsKey(nota.tecla))
    {
        Debug.LogWarning("Tecla inválida: " + nota.tecla);
        return;
    }
    
    int indice = teclasParaIndices[nota.tecla];
    Vector3 posicao = new Vector3(posicoesX[indice], alturaSpawn, 0);
    
    GameObject notaObj = Instantiate(notaPrefab, posicao, Quaternion.identity);
    
    notaObj.tag = teclasParaTags[nota.tecla];
    notaObj.name = teclasParaTags[nota.tecla];
    
    // ADICIONE ESTE DEBUG
    Debug.Log("Nota spawnada: " + nota.tecla + " com tag " + notaObj.tag + " no tempo " + nota.tempo.ToString("F2"));
    
    SpriteRenderer sprite = notaObj.GetComponent<SpriteRenderer>();
    if (sprite != null)
    {
        sprite.color = cores[indice];
    }
}
}