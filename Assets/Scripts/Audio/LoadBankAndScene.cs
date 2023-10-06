using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadBankAndScene : MonoBehaviour
{
    [FMODUnity.BankRef]
    public List<string> banks;

    private void Awake()
    {
        LoadBanks();
    }

    public void LoadBanks()
    {
        foreach (string b in banks)
        {
            FMODUnity.RuntimeManager.LoadBank(b, true);
            Debug.Log("Loaded bank " + b);
        }
        StartCoroutine(CheckBanksLoaded());
    }

    IEnumerator CheckBanksLoaded()
    {
        while (!FMODUnity.RuntimeManager.HaveAllBanksLoaded)
        {
            yield return null;
        }
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
