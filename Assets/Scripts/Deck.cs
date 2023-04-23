using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
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

    public int[] values = new int[52];
    int cardIndex = 0;    
       
    private void Awake()
    {    
        InitCardValues();        

    }

    private void Start()
    {
        ShuffleCards();
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

    void StartGame()
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
            }

            else if (dealer.GetComponent<CardHand>().points == 21)
            {
                finalMessage.text = "HAS PERDIDO";
            }
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

    }

    void PushDealer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        dealer.GetComponent<CardHand>().Push(faces[cardIndex],values[cardIndex]);
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

    public void Hit()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */
        
        //Repartimos carta al jugador
        PushPlayer();

        /*TODO:
         * Comprobamos si el jugador ya ha perdido y mostramos mensaje
         */      

    }

    public void Stand()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */

        /*TODO:
         * Repartimos cartas al dealer si tiene 16 puntos o menos
         * El dealer se planta al obtener 17 puntos o más
         * Mostramos el mensaje del que ha ganado
         */                
         
    }

    public void PlayAgain()
    {
        hitButton.interactable = true;
        stickButton.interactable = true;
        finalMessage.text = "";
        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();          
        cardIndex = 0;
        ShuffleCards();
        StartGame();
    }
    
}
