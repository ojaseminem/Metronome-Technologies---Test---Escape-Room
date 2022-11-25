using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager instance;
    private void Awake() => instance = this;
    

    #endregion

    #region Public Variables

    [SerializeField] private GameObject readMe;
    [SerializeField] private TextMeshProUGUI remarkText;
    [SerializeField] private ThiefController thiefController;
    [SerializeField] private CopSpawner copSpawner;

    #endregion

    private void Start()
    {
        EnableReadMe();
    }

    public void EnableReadMe()
    {
        StartCoroutine(CompleteReadMe());
        IEnumerator CompleteReadMe()
        {
            readMe.SetActive(true);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            readMe.SetActive(false);
        }
    }

    public void SpawnCops()
    {
        copSpawner.SpawnCops();
        StartCoroutine(EnableMove());
        IEnumerator EnableMove()
        {
            yield return new WaitForSeconds(.5f);
            thiefController.canMove = true;
        }
    }

    public void ResetCops()
    {
        remarkText.transform.parent.gameObject.SetActive(false);
        copSpawner.ResetCops();
        thiefController.canMove = false;
    }

    public void Remark(string remark, Color color)
    {
        remarkText.transform.parent.gameObject.SetActive(true);
        remarkText.color = color;
        remarkText.text = remark;
    }
}