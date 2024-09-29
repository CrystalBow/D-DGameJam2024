using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CardPouch : MonoBehaviour
{
    [SerializeField] private GameObject Owner;
    [SerializeField] private GameObject Dealer;
    private ModularMover _mover;
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
        _mover = Owner.GetComponent<ModularMover>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isThereNewCard)
        {
            applyEffect(Collection[Collection.Count-1]);
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
        print(cards.ID1.ToString());
        switch (cards.ID1)
        {
            case 0:
                _mover.NumMaxJumps += 1;
                break;
            case 1:
                _mover.DashAvailiable = true;
                break;
            case 2:
                _mover.JumpPower += 3;
                break;
            case 3:
                _mover.JumpPower -= 2;
                break;
            case 4:
                _mover.Speed += 3;
                _mover.RunSpeed += 5;
                _mover.MaxWalk1 += 2;
                _mover.MaxRun1 += 3;
                break;
            case 5:
                _mover.Contigencies += 1;
                break;
            case 6:
                if (Collection.Count > 1)
                {
                    undo(Collection[Collection.Count - 2]);
                    Collection.Remove(cards);
                }
                break; 
            case 7:
                break;
            case 8:
                _mover.Smartness *= -1;
                break;
            case 9:
                DrawCall();
                applyEffect(Collection[Collection.Count - 1]);
                DrawCall();
                break;
            case 10:
                _mover.slip();
                break;
            case 11:
                _mover.Speed -= 2;
                _mover.RunSpeed -= 3;
                _mover.MaxWalk1 -= 1;
                _mover.MaxRun1 -= 2;
                break;
            default:
                print("No Effect");
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
