using UnityEngine;
using UnityEngine.UI;

public class SistemaDeJogo : MonoBehaviour
{
    public int vidaMaxima = 100;
    public int vidaAtual;
    
    public int pontos = 0;
    public int combo = 0;
    public int comboMaximo = 0;
    
    public Text textoVida;
    public Text textoPontos;
    public Text textoCombo;
    
    public Slider barraVida;
    
    void Start()
    {
        vidaAtual = vidaMaxima;
        AtualizarUI();
    }
    
    public void AcertoPerfeito()
    {
        combo += 2;
        pontos += 100 * (1 + combo / 10);
        RecuperarVida(2);
        AtualizarComboMaximo();
        AtualizarUI();
    }
    
    public void AcertoBom()
    {
        combo += 1;
        pontos += 50 * (1 + combo / 10);
        RecuperarVida(1);
        AtualizarComboMaximo();
        AtualizarUI();
    }
    
    public void Errou()
    {
        combo = 0;
        PerderVida(10);
        AtualizarUI();
    }
    
    public void NotaPerdida()
    {
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
        Debug.Log("GAME OVER! Pontuacao final: " + pontos);
        Time.timeScale = 0;
    }
}