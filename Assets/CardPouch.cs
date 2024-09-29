using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CardPouch : MonoBehaviour
{
    [SerializeField] private GameObject Owner;
    [SerializeField] private GameObject Dealer;
    private Dealer _dealer;
    private List<Cards> Collection = new List<Cards>();
    private bool isThereNewCard, drawCard;
    public GameObject Owner1
    {
        get => Owner;
        set => Owner = value;
    }

    public void deal(Cards cards)
    {
        Collection.Add(cards);
        isThereNewCard = true;
    }

    public void discard()
    {
        Cards target = Collection[Collection.Count - 1];
        Collection.Remove(target);
    }
    
    public bool IsThereNewCard
    {
        get => isThereNewCard;
        set => isThereNewCard = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        _dealer = Dealer.GetComponent<Dealer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isThereNewCard)
        {
            isThereNewCard = false;
        }

        if (drawCard)
        {
            DrawCall();
        }
        
    }

    private void undo(Cards cards)
    {
        
    }

    private void applyEffect(Cards cards)
    {
        switch (cards.ID1)
        {
            case 0:
                
                break;
        }
        
            
        
    }
    public void drawCardDetect(InputAction.CallbackContext context)
    {
        float input = context.ReadValue<float>();
        if (input > 0)
        {
            drawCard = true;
        }
    }

    private void DrawCall()
    {
        _dealer.IsDealing = true;
        drawCard = false;
    }
}
