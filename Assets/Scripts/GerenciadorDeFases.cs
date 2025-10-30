using UnityEngine;

public class GerenciadorDeFases : MonoBehaviour
{
    public static GerenciadorDeFases Instance;
    
    public string faseAtual = "Fase1"; // Fase selecionada
    
    void Awake()
    {
        // Singleton - mantém entre cenas
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void SelecionarFase(string nomeFase)
    {
        faseAtual = nomeFase;
        Debug.Log("Fase selecionada: " + nomeFase);
    }
    
    public string[] ListarFasesDisponiveis()
    {
        // Carrega todos os charts da pasta Resources/Charts
        TextAsset[] charts = Resources.LoadAll<TextAsset>("Charts");
        
        string[] nomesFases = new string[charts.Length];
        
        for (int i = 0; i < charts.Length; i++)
        {
            nomesFases[i] = charts[i].name;
        }
        
        Debug.Log("Fases encontradas: " + charts.Length);
        return nomesFases;
    }
}