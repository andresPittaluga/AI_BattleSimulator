using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance;

    [SerializeField] GameObject _team1;
    [SerializeField] GameObject _team2;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);
    }

    public void TeamWin(TypeEvent team)
    {
        if (team == TypeEvent.Team1)
        {
            _team2.SetActive(true);
        }
        else if (team == TypeEvent.Team2)
        {
            _team1.SetActive(true);
        }
        else Debug.Log("Manda un equipo valido!");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) ResetScene();
    }
    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
