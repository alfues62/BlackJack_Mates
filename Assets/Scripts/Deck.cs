using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    [Header("Referencias")]
    public Sprite[] faces;
    public GameObject dealer;
    public GameObject player;
    public Button hitButton;
    public Button stickButton;
    public Button playAgainButton;
    public Text finalMessage;
    public Text probMessage;
    public Text mensajePuntos;
    public Text mensajeDealerPts;
    public Button b10;
    public Button b20;
    public Button b50;
    public Button b100;
    public Text bank;
    public Text bets;
    public int bankValue;
    public int bet;

    public int[] values = new int[52];
    int cardIndex = 0;    
       
    private void Awake()
    {    
        InitCardValues();        

    }

    private void Start()
    {
        ShuffleCards();
        bank.text = "Tienes " + bankValue + " €";
        StartGame();        
    }

    private void InitCardValues()
    {

        for (int i = 0; i < values.Length; i++)
        {
            if (i == 0 || i == 13 || i == 26 || i == 39)
            {
                values[i] = 11;
            }
            else if (i < 10)
            {
                values[i] = i + 1;
            }
            else if (i < 13)
            {
                values[i] = 10;
            }
            else if (i < 23)
            {
                values[i] = i - 12;
            }
            else if (i < 26)
            {
                values[i] = 10;
            }
            else if (i < 36)
            {
                values[i] = i - 25;
            }
            else if (i < 39)
            {
                values[i] = 10;
            }
            else if (i < 48)
            {
                values[i] = i - 38;
            }
            else
            {
                values[i] = 10;
            }
        }
    }

    private void ShuffleCards()
    {
        for (int i = 0; i < values.Length; i++)
        {
            var j = Random.Range(0, 52);
            var temp_value = values[i];
            var temp_faces = faces[i];

            values[i] = values[j];  // Elegimos una cara y un valor aleatorios.
            values[j] = temp_value;
            faces[i] = faces[j];
            faces[j] = temp_faces;

        }

    }

    void PushDealer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        dealer.GetComponent<CardHand>().Push(faces[cardIndex], values[cardIndex]);
        cardIndex++;
    }

    void PushPlayer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        player.GetComponent<CardHand>().Push(faces[cardIndex], values[cardIndex]/*,cardCopy*/);
        cardIndex++;
        CalculateProbabilities();
    }

    void StartGame()
    {
        if(bankValue >= 10)
        {
            for (int i = 0; i < 2; i++)
            {
                PushPlayer();
                PushDealer();
                hitButton.interactable = false;
                stickButton.interactable = false;

                mensajePuntos.text = "Tienes " + player.GetComponent<CardHand>().points + " puntos";
                if (player.GetComponent<CardHand>().points == 21)
                {
                    finalMessage.text = "HAS GANADO SIN SIQUIERA JUGAR";
                    dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);
                    hitButton.interactable = false;
                    stickButton.interactable = false;
                    b10.interactable = false;
                    b20.interactable = false;
                    b50.interactable = false;
                    b100.interactable = false;
                }
                else if (dealer.GetComponent<CardHand>().points == 21)
                {
                    finalMessage.text = "HAS PERDIDO";
                    dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);
                    b10.interactable = false;
                    b20.interactable = false;
                    b50.interactable = false;
                    b100.interactable = false;
                }
            }
        }
        else
        {
            finalMessage.text = "¡Ya no tienes Dinero! Pulsa Para Jugar de Nuevo";
            b10.interactable = false;
            b20.interactable = false;
            b50.interactable = false;
            b100.interactable = false;
            hitButton.interactable = false;
            stickButton.interactable = false;
            bankValue = 1000;
        }

    }

    private void CalculateProbabilities()
    {
        int playerPoints = player.GetComponent<CardHand>().points;
        int dealerPoints = dealer.GetComponent<CardHand>().points;

        if(playerPoints == 21)
        {
            probMessage.text = "100%";
        }
        else
        {
            float probability = GetWinProbability(playerPoints, dealerPoints);
            probMessage.text = Mathf.RoundToInt(probability * 100f) + "%";
        }
    }

    private float GetWinProbability(int playerPoints, int dealerPoints)
    {
        int playerTotal = 21 - playerPoints;
        if (playerTotal <= 0)
        {
            return 0f; // si ya tenemos 21 o mas será 0 la probabilidad
        }

        int dealerTotal = 21 - dealerPoints;

        int cardsLeft = 52 - cardIndex;
        int winningCards = 0;

        for (int i = 0; i < cardsLeft; i++)
        {
            int cardValue = values[cardIndex + i];
            if (cardValue == 11 && playerTotal < 11)
            {
                cardValue = 1;
            }

            if (playerPoints + cardValue <= 21)
            {
                if (cardValue == 11 && dealerTotal < 11)
                {
                    cardValue = 1;
                }

                if (dealerPoints + cardValue <= 21)
                {
                    winningCards++;
                }
            }
        }

        return 1f - ((float)winningCards / cardsLeft); // Retorna la probabilidad invertida
    }

    public void x10()
    {
        bet = 10;
        bankValue -= bet;
        bank.text = "Tienes " + bankValue + " €";
        b10.interactable = false;
        b20.interactable = false;
        b50.interactable = false;
        b100.interactable = false;
        hitButton.interactable = true;
        stickButton.interactable = true;
        bets.text = "";
    }

    public void x20()
    {
        bet = 20;
        bankValue -= bet;
        bank.text = "Tienes " + bankValue + " €";
        b10.interactable = false;
        b20.interactable = false;
        b50.interactable = false;
        b100.interactable = false;
        hitButton.interactable = true;
        stickButton.interactable = true;
        bets.text = "";
    }

    public void x50()
    {
        bet = 50;
        bankValue -= bet;
        bank.text = "Tienes " + bankValue + " €";
        b10.interactable = false;
        b20.interactable = false;
        b50.interactable = false;
        b100.interactable = false;
        hitButton.interactable = true;
        stickButton.interactable = true;
        bets.text = "";
    }

    public void x100()
    {
        bet = 100;
        bankValue -= bet;
        bank.text = "Tienes " + bankValue + " €";
        b10.interactable = false;
        b20.interactable = false;
        b50.interactable = false;
        b100.interactable = false;
        hitButton.interactable = true;
        stickButton.interactable = true;
        bets.text = "";
    }

    public void Hit()
    {
        PushPlayer();
        mensajePuntos.text = "Tienes " + player.GetComponent<CardHand>().points + " puntos";

        if (player.GetComponent<CardHand>().points > 21)
        {
            dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);
            finalMessage.text = "Superaste los 21 Puntos. Perdiste " + (bet).ToString() + " €.";
            hitButton.interactable = false;
            stickButton.interactable = false;
        }

    }

    public void Stand()
    {
        dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);
        hitButton.interactable = false;
        stickButton.interactable = false;

        while (dealer.GetComponent<CardHand>().points < 17)
        {
            PushDealer();
        }
        int dealerPoints = dealer.GetComponent<CardHand>().points;
        int playerPoints = player.GetComponent<CardHand>().points;
        mensajeDealerPts.text = "El dealer ha conseguido " + dealerPoints + " puntos";

        if (dealerPoints == 21)
        {
            finalMessage.text = "El dealer hizo BlackJack. ¡ "+ (bet).ToString() + " € Menos !";
            bankValue -= bet * 2;
            bank.text = "Tienes " + bankValue + " €";
        }
        else if (playerPoints == 21)
        {
            finalMessage.text = "¡¡FELICIDADES!! ¡¡Has hecho BlackJack!! ¡ "+ (bet * 2).ToString() + " € Más ! ";
            bankValue += bet * 2;
            bank.text = "Tienes " + bankValue + " €";
        }
        else if (dealerPoints > 21)
        {
            finalMessage.text = "El dealer se pasa ¡Ganas " + (bet * 2).ToString() + " € !.";
            bankValue += bet * 2;
            bank.text = "Tienes " + bankValue + " €";
        }
        else if (dealerPoints == playerPoints)
        {
            finalMessage.text = "¡Empatasteis!";
            bankValue += bet;
            bank.text = "Tienes " + bankValue + " €";
        }
        else if (dealerPoints > playerPoints)
        {
            finalMessage.text = "Te han superado ¡Has Perdido "+ (bet).ToString() + " € !";
            bankValue -= bet * 2;
            bank.text = "Tienes " + bankValue + " €";
        }
        else
        {
            finalMessage.text = "Has ganado " + (bet * 2).ToString() + " € a la banca. Enhorabuena.";
            bankValue += bet * 2;
            bank.text = "Tienes " + bankValue + " €";
        }

    }

    public void PlayAgain()
    {
        b10.interactable = true;
        b20.interactable = true;
        b50.interactable = true;
        b100.interactable = true;
        finalMessage.text = "";
        mensajeDealerPts.text = "";
        bank.text = "Tienes " + bankValue.ToString() + " €";
        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();
        cardIndex = 0;
        ShuffleCards();
        StartGame();
    }
    
}
