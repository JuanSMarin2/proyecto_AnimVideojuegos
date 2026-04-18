using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Clases.Clase_2.Scripts
{
    internal class Game : MonoBehaviour
    {
        private static Game instance;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void CreateGame()
        {
            GameObject gameGo = new GameObject("Game");
            instance = gameGo.AddComponent<Game>();
            DontDestroyOnLoad(gameGo);
        }

        public static Game Instance
        {
            get
            {
                if (instance == null)
                {
                    CreateGame();
                }
                return instance;
            }

        }

        private CharacterState playerOne;

        public CharacterState PlayerOne => playerOne;

        private void Awake()
        {
            CreatePlayer();
        }
        private void CreatePlayer()
        {
            GameObject playerGo = new GameObject("Player 1");
            playerOne = playerGo.AddComponent<CharacterState>();
            DontDestroyOnLoad(playerGo);

        }
    }
}
