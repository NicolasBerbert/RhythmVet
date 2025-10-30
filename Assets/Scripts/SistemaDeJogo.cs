using UnityEngine;
using UnityEngine.UI;

public class SistemaDeJogo : MonoBehaviour
{
    [Header("Sistema de Vida e Pontos")]
    public int vidaMaxima = 100;
    public int vidaAtual;
    public int pontos = 0;
    public int combo = 0;
    public int comboMaximo = 0;
    
    [Header("Timer")]
    public float duracaoFase = 60f;
    private float tempoRestante;
    public Text textoTimer;
    
    [Header("Estatísticas")]
    public int acertosPerfeitos = 0;
    public int acertosBons = 0;
    public int erros = 0;
    public int notasPerdidas = 0;
    
    [Header("UI Principal")]
    public Text textoVida;
    public Text textoPontos;
    public Text textoCombo;
    public Slider barraVida;
    
    [Header("Painel Game Over")]
    public GameObject painelGameOver;
    public Text textoPontuacaoFinal;
    public Text textoComboMaximoFinal;
    
    [Header("Painel Vitória")]
    public GameObject painelVitoria;
    public Text textoPontuacaoVitoria;
    public Text textoComboMaximoVitoria;
    public Text textoEstatisticas;
    public Text textoMoedasGanhas;
    
    private bool jogoAtivo = true;
    
    void Start()
    {
        vidaAtual = vidaMaxima;
        tempoRestante = duracaoFase;
        jogoAtivo = true;
        AtualizarUI();
        
        if (painelGameOver != null)
        {
            painelGameOver.SetActive(false);
        }
        
        if (painelVitoria != null)
        {
            painelVitoria.SetActive(false);
        }
    }
    
    void Update()
    {
        // ATALHO DE DEV - Aperte V para vitória instantânea
        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("Atalho DEV: Forçando Vitória!");
            Vitoria();
            return;
        }
        
        // ATALHO DE DEV - Aperte G para Game Over instantâneo
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("Atalho DEV: Forçando Game Over!");
            GameOver();
            return;
        }
        
        if (jogoAtivo)
        {
            tempoRestante -= Time.deltaTime;
            AtualizarTimer();
            
            if (tempoRestante <= 0)
            {
                Vitoria();
            }
        }
    }
    
    void AtualizarTimer()
    {
        if (textoTimer != null)
        {
            int minutos = Mathf.FloorToInt(tempoRestante / 60);
            int segundos = Mathf.FloorToInt(tempoRestante % 60);
            textoTimer.text = string.Format("{0:00}:{1:00}", minutos, segundos);
        }
    }
    
    public void AcertoPerfeito()
    {
        if (!jogoAtivo) return;
        
        acertosPerfeitos++;
        combo += 2;
        pontos += 100 * (1 + combo / 10);
        RecuperarVida(2);
        AtualizarComboMaximo();
        AtualizarUI();
    }
    
    public void AcertoBom()
    {
        if (!jogoAtivo) return;
        
        acertosBons++;
        combo += 1;
        pontos += 50 * (1 + combo / 10);
        RecuperarVida(1);
        AtualizarComboMaximo();
        AtualizarUI();
    }
    
    public void Errou()
    {
        if (!jogoAtivo) return;
        
        erros++;
        combo = 0;
        PerderVida(10);
        AtualizarUI();
    }
    
    public void NotaPerdida()
    {
        if (!jogoAtivo) return;
        
        notasPerdidas++;
        combo = 0;
        PerderVida(5);
        AtualizarUI();
    }
    
    void RecuperarVida(int quantidade)
    {
        vidaAtual += quantidade;
        if (vidaAtual > vidaMaxima)
        {
            vidaAtual = vidaMaxima;
        }
    }
    
    void PerderVida(int quantidade)
    {
        vidaAtual -= quantidade;
        if (vidaAtual <= 0)
        {
            vidaAtual = 0;
            GameOver();
        }
    }
    
    void AtualizarComboMaximo()
    {
        if (combo > comboMaximo)
        {
            comboMaximo = combo;
        }
    }
    
    void AtualizarUI()
    {
        if (textoVida != null)
        {
            textoVida.text = "Vida: " + vidaAtual;
        }
        
        if (textoPontos != null)
        {
            textoPontos.text = "Pontos: " + pontos;
        }
        
        if (textoCombo != null)
        {
            textoCombo.text = "Combo: x" + combo;
        }
        
        if (barraVida != null)
        {
            barraVida.value = vidaAtual;
        }
    }
    
    void GameOver()
    {
        jogoAtivo = false;
        Debug.Log("GAME OVER! Pontuacao final: " + pontos);
        
        if (painelGameOver != null)
        {
            painelGameOver.SetActive(true);
        }
        
        if (textoPontuacaoFinal != null)
        {
            textoPontuacaoFinal.text = "Pontuação: " + pontos;
        }
        
        if (textoComboMaximoFinal != null)
        {
            textoComboMaximoFinal.text = "Combo Máximo: x" + comboMaximo;
        }
        
        Time.timeScale = 0;
    }
    
    void Vitoria()
    {
        jogoAtivo = false;
        Debug.Log("VITORIA! Pontuacao: " + pontos);
        
        // Calcula moedas ganhas (1 moeda a cada 100 pontos)
        int moedasGanhas = Mathf.FloorToInt(pontos / 100f);
        
        // Salva as moedas
        int moedasAtuais = PlayerPrefs.GetInt("WoofCoins", 0);
        PlayerPrefs.SetInt("WoofCoins", moedasAtuais + moedasGanhas);
        PlayerPrefs.Save();
        
        Debug.Log("Moedas ganhas: " + moedasGanhas + " | Total: " + PlayerPrefs.GetInt("WoofCoins"));
        
        // Ativa o painel de vitória
        if (painelVitoria != null)
        {
            painelVitoria.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Painel de vitória não conectado!");
        }
        
        // Atualiza textos
        if (textoPontuacaoVitoria != null)
        {
            textoPontuacaoVitoria.text = "Pontuação: " + pontos;
        }
        
        if (textoComboMaximoVitoria != null)
        {
            textoComboMaximoVitoria.text = "Combo Máximo: x" + comboMaximo;
        }
        
        // Estatísticas detalhadas
        if (textoEstatisticas != null)
        {
            int totalAcertos = acertosPerfeitos + acertosBons;
            int totalNotas = totalAcertos + erros + notasPerdidas;
            float acuracia = totalNotas > 0 ? (totalAcertos * 100f / totalNotas) : 0;
            
            textoEstatisticas.text = string.Format(
                "Acertos Perfeitos: {0}\nAcertos Bons: {1}\nErros: {2}\nNotas Perdidas: {3}\n\nAcurácia: {4:F1}%",
                acertosPerfeitos, acertosBons, erros, notasPerdidas, acuracia
            );
        }
        
        // Moedas ganhas
        if (textoMoedasGanhas != null)
        {
            textoMoedasGanhas.text = "Woof Coins: +" + moedasGanhas;
        }
        
        // Para o jogo
        Time.timeScale = 0;
    }
    
    public void ReiniciarJogo()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay");
    }
}