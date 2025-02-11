using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Referencia al panel de Game Over en la UI
    [SerializeField]
    private GameObject panelGameOver;

    // M�todo que se ejecuta cuando el jugador muere
    public void GameOver()
    {
        // Muestra el panel de Game Over
        panelGameOver.SetActive(true);

        // Libera el cursor para que el jugador pueda moverlo en la pantalla de Game Over
        Cursor.lockState = CursorLockMode.Confined;
    }

    // M�todo para recargar el nivel y reiniciar el juego
    public void LoadSceneLevel()
    {
        // Carga la escena "Level01", reiniciando el nivel
        SceneManager.LoadScene("Level01");
    }
}
