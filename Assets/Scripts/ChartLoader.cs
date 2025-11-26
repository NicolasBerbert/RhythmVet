using UnityEngine;
using System.Collections.Generic;

public class ChartLoader : MonoBehaviour
{
    public GameObject notaPrefab;
    public float alturaSpawn = 6f;
    public AudioSource audioSource;
    [Header("Ajuste de Timing")]
    [Tooltip("Ajuste fino do timing. Negativo = notas adiantadas, Positivo = notas atrasadas")]
    public float offsetTiming = 0f; // Comece com 0 e ajuste aos poucos
    
    private float tempoDeQueda = 3f;
    
    private ChartData chartAtual;
    private List<NotaData> notasRestantes = new List<NotaData>();
    private bool chartCarregado = false;
    private float tempoInicio;
    
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
    
    // CORES PADRÃO - ESCOLHA UMA OPÇÃO:
    
    // Opção 1: Cores diretas (RGB)
    private Color[] cores = new Color[] 
    { 
        new Color(1f, 0.7f, 0.8f),    // Rosa pastel - A
        new Color(0.7f, 0.85f, 1f),   // Azul bebê - S
        new Color(0.7f, 1f, 0.8f),    // Verde menta - J
        new Color(1f, 0.95f, 0.7f),   // Amarelo suave - K
        new Color(0.95f, 0.8f, 1f)    // Lavanda - L
    };
    
    // OU Opção 2: Usar HexToColor (comente a linha acima e descomente estas):
    /*
    private Color[] cores;
    
    void Awake()
    {
        cores = new Color[] 
        { 
            HexToColor("#FFB3D1"),  // Rosa - A
            HexToColor("#B3D9FF"),  // Azul - S
            HexToColor("#B3FFCC"),  // Verde - J
            HexToColor("#FFF3B3"),  // Amarelo - K
            HexToColor("#F3CCFF")   // Roxo - L
        };
    }
    */
    
    void Start()
    {
        CalcularTempoDeQueda();
        
        GerenciadorDeFases gerenciador = FindObjectOfType<GerenciadorDeFases>();
        
        string faseParaCarregar = "Fase1";
        
        if (gerenciador != null)
        {
            faseParaCarregar = gerenciador.faseAtual;
        }
        
        CarregarChart(faseParaCarregar);
    }
    
    void CalcularTempoDeQueda()
    {
        if (notaPrefab != null)
        {
            NotaMovimento movimento = notaPrefab.GetComponent<NotaMovimento>();
            if (movimento != null)
            {
                float distancia = alturaSpawn - (-3f);
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
    
    for (int i = notasRestantes.Count - 1; i >= 0; i--)
    {
        NotaData nota = notasRestantes[i];
        
        // MODIFICADO: Adiciona o offset
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
        Vector3 posicao = new Vector3(posicoesX[indice], alturaSpawn, -5f);
        
        GameObject notaObj = Instantiate(notaPrefab, posicao, Quaternion.identity);
        
        notaObj.tag = teclasParaTags[nota.tecla];
        notaObj.name = teclasParaTags[nota.tecla];
        
        SpriteRenderer sprite = notaObj.GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            sprite.color = cores[indice];
        }
    }
    
    // FUNÇÃO HexToColor DEVE ESTAR AQUI DENTRO DA CLASSE!
    Color HexToColor(string hex)
    {
        hex = hex.Replace("#", "");
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        return new Color32(r, g, b, 255);
    }
    
} // ← FIM DA CLASSE - HexToColor deve estar ANTES daqui!