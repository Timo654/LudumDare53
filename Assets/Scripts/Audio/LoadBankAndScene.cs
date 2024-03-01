using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadBankAndScene : MonoBehaviour
{
    [FMODUnity.BankRef]
    public List<string> banks;
    public static event Action OnBanksLoaded;

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
        OnBanksLoaded?.Invoke();
    }
}
