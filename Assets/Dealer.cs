using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dealer : MonoBehaviour
{
    [SerializeField] public CardPouch TargetPouch;
    private bool isDealing = new bool();

    public bool IsDealing
    {
        get => isDealing;
        set => isDealing = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        IsDealing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDealing)
        {
            drawCard();
        }
    }


    void drawCard()
    {
        Cards card = new Cards();
        TargetPouch.deal(card);
        isDealing = false;
    }
}
