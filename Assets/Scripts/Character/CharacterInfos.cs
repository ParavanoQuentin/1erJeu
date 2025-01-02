using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterInfos : MonoBehaviour
{
    private GameManager manager;
    public int moneyCount = 0;
    [SerializeField] private TextMeshProUGUI moneyTxt;

    private void Start()
    {
        manager = GameManager.GetInstance();
    }

    private void Update()
    {
        moneyTxt.text = moneyCount.ToString();
    }
}
