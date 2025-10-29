using UnityEngine;
using UnityEngine.SceneManagement;

public class GerenciadorDeCenas : MonoBehaviour
{
    public void IrParaCena(string nomeCena)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(nomeCena);
    }
    
    public void IrMenuInicial()
    {
        IrParaCena("MenuInicial");
    }
    
    public void IrConsultorio()
    {
        IrParaCena("Consultorio");
    }
    
    public void IrGameplay()
    {
        IrParaCena("Gameplay");
    }
    
    public void IrLoja()
    {
        IrParaCena("Loja");
    }
    
    public void IrGacha()
    {
        IrParaCena("Gacha");
    }
    
    public void SairDoJogo()
    {
        Debug.Log("Saindo do jogo...");
        Application.Quit();
    }
}