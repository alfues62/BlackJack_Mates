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
        /*TODO:
         * Asignar un valor a cada una de las 52 cartas del atributo "values".
         * En principio, la posición de cada valor se deberá corresponder con la posición de faces. 
         * Por ejemplo, si en faces[1] hay un 2 de corazones, en values[1] debería haber un 2.
         */

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
        /*TODO:
         * Barajar las cartas aleatoriamente.
         * El método Random.Range(0,n), devuelve un valor entre 0 y n-1
         * Si lo necesitas, puedes definir nuevos arrays.
         */

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
                /*TODO:
                 * Si alguno de los dos obtiene Blackjack, termina el juego y mostramos mensaje
                 */

                mensajePuntos.text = "Tienes " + player.GetComponent<CardHand>().points + " puntos";
                //
                if (player.GetComponent<CardHand>().points == 21)
                {
                    finalMessage.text = "HAS GANADO SIN SIQUIERA JUGAR";
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
        /*TODO:
         * Calcular las probabilidades de:
         * - Teniendo la carta oculta, probabilidad de que el dealer tenga más puntuación que el jugador
         * - Probabilidad de que el jugador obtenga entre un 17 y un 21 si pide una carta
         * - Probabilidad de que el jugador obtenga más de 21 si pide una carta          
         */

        int playerPoints = player.GetComponent<CardHand>().points;
        if (playerPoints <= 11)
        {
            probMessage.text = "0%";
        }
        else if (playerPoints == 12)
        {
            probMessage.text = "31%";
        }
        else if (playerPoints == 13)
        {
            probMessage.text = "39%";
        }
        else if (playerPoints == 14)
        {
            probMessage.text = "56%";
        }
        else if (playerPoints == 15)
        {
            probMessage.text = "58%";
        }
        else if (playerPoints == 16)
        {
            probMessage.text = "62%";
        }
        else if (playerPoints == 17)
        {
            probMessage.text = "69%";
        }
        else if (playerPoints == 18)
        {
            probMessage.text = "77%";
        }
        else if (playerPoints == 19)
        {
            probMessage.text = "85%";
        }
        else if (playerPoints == 20)
        {
            probMessage.text = "92%";
        }
        else
        {
            probMessage.text = "100%";
        }

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
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */

        //Repartimos carta al jugador
        PushPlayer();
        mensajePuntos.text = "Tienes " + player.GetComponent<CardHand>().points + " puntos";


        /*TODO:
         * Comprobamos si el jugador ya ha perdido y mostramos mensaje
         */
        if (player.GetComponent<CardHand>().points > 21)
        {
            finalMessage.text = "Superaste los 21 Puntos. Perdiste.";
            hitButton.interactable = false;
            stickButton.interactable = false;
        }

    }

    public void Stand()
    {/*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */


        /*TODO:
         * Repartimos cartas al dealer si tiene 16 puntos o menos
         * El dealer se planta al obtener 17 puntos o más
         * Mostramos el mensaje del que ha ganado
         */

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
            finalMessage.text = "El dealer hizo BlackJack.";
        }
        else if (playerPoints == 21)
        {
            finalMessage.text = "¡¡FELICIDADES!! ¡¡Has hecho BlackJack!! ";
            bankValue += bet * 2;
            bank.text = "Tienes " + bankValue + " €";
        }
        else if (dealerPoints > 21)
        {
            finalMessage.text = "El dealer se pasa ¡Ganas!.";
            bankValue += bet * 2;
            bank.text = "Tienes " + bankValue + " €";
        }
        else if (dealerPoints == playerPoints)
        {
            finalMessage.text = "¡Empatasteis!";
            bankValue += bet * 2;
            bank.text = "Tienes " + bankValue + " €";
        }
        else if (dealerPoints > playerPoints)
        {
            finalMessage.text = "Te han superado ¡Has Perdido!";
            bankValue += bet * 2;
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
