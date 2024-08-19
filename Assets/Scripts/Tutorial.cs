using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject tutorialPanel01;
    [SerializeField] private GameObject tutorialPanel02;
    [SerializeField] private GameObject tutorialPanel03;
    [SerializeField] private GameObject tutorialPanel04;
    
    private GameObject _currentPanel;

    public void StartTutorial()
    {
        tutorialPanel01.SetActive(true);
        _currentPanel = tutorialPanel01;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            GoToNextPanel();
    }

    private void GoToNextPanel()
    {
        var nextPanel = GetNextPanel(_currentPanel);
        if (nextPanel is null)
        {
            tutorialPanel01.SetActive(false);
            tutorialPanel02.SetActive(false);
            tutorialPanel03.SetActive(false);
            tutorialPanel04.SetActive(false);
            GameManager.Instance.StartGame();
            return;
        }
        _currentPanel.SetActive(false);
        nextPanel.SetActive(true);
        _currentPanel = nextPanel;
    }
    
    private GameObject GetNextPanel(GameObject currentPanel)
    {
        if (currentPanel == tutorialPanel01)
            return tutorialPanel02;
        if (currentPanel == tutorialPanel02)
            return tutorialPanel03;
        if (currentPanel == tutorialPanel03)
            return tutorialPanel04;
        return null;
    }
}
