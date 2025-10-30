using UnityEngine;
using UnityEngine.UI;

public class UIConsultorio : MonoBehaviour
{
    public GameObject painelSelecaoFases;
    public GameObject botaoFasePrefab;
    public Transform contentContainer;
    
    // NOVO - Sistema de Moedas
    public Text textoMoedas;
    
    private GerenciadorDeFases gerenciadorFases;
    
    void Start()
    {
        if (painelSelecaoFases != null)
        {
            painelSelecaoFases.SetActive(false);
        }
        
        // NOVO - Atualiza moedas ao iniciar
        AtualizarMoedas();
    }
    
    // NOVA FUNÇÃO
    public void AtualizarMoedas()
    {
        int moedas = PlayerPrefs.GetInt("WoofCoins", 0);
        
        if (textoMoedas != null)
        {
            textoMoedas.text = moedas.ToString();
        }
        
        Debug.Log("Woof Coins atuais: " + moedas);
    }
    
    public void AbrirSelecaoFases()
    {
        if (painelSelecaoFases != null)
        {
            painelSelecaoFases.SetActive(true);
        }
        
        gerenciadorFases = FindObjectOfType<GerenciadorDeFases>();
        if (gerenciadorFases == null)
        {
            GameObject obj = new GameObject("GerenciadorDeFases");
            gerenciadorFases = obj.AddComponent<GerenciadorDeFases>();
        }
        
        PopularListaFases();
    }
    
    public void FecharSelecaoFases()
    {
        if (painelSelecaoFases != null)
        {
            painelSelecaoFases.SetActive(false);
        }
    }
    
    void PopularListaFases()
    {
        foreach (Transform child in contentContainer)
        {
            Destroy(child.gameObject);
        }
        
        string[] fases = gerenciadorFases.ListarFasesDisponiveis();
        
        if (fases.Length == 0)
        {
            Debug.LogWarning("Nenhuma fase encontrada em Resources/Charts/");
            return;
        }
        
        foreach (string nomeFase in fases)
        {
            GameObject botao = Instantiate(botaoFasePrefab, contentContainer);
            
            Text textoB = botao.GetComponentInChildren<Text>();
            if (textoB != null)
            {
                textoB.text = nomeFase;
            }
            
            Button btn = botao.GetComponent<Button>();
            string faseCapturada = nomeFase;
            btn.onClick.AddListener(() => SelecionarESair(faseCapturada));
        }
    }
    
    void SelecionarESair(string nomeFase)
    {
        gerenciadorFases.SelecionarFase(nomeFase);
        
        GerenciadorDeCenas gerenciadorCenas = FindObjectOfType<GerenciadorDeCenas>();
        if (gerenciadorCenas != null)
        {
            gerenciadorCenas.IrGameplay();
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay");
        }
    }
    
    // NOVA FUNÇÃO - Para debug/testes
    public void AdicionarMoedasTeste(int quantidade)
    {
        int moedas = PlayerPrefs.GetInt("WoofCoins", 0);
        PlayerPrefs.SetInt("WoofCoins", moedas + quantidade);
        PlayerPrefs.Save();
        AtualizarMoedas();
    }
    
    // NOVA FUNÇÃO - Resetar moedas
    public void ResetarMoedas()
    {
        PlayerPrefs.SetInt("WoofCoins", 0);
        PlayerPrefs.Save();
        AtualizarMoedas();
    }

    void OnEnable()
{
    AtualizarMoedas();
}
}