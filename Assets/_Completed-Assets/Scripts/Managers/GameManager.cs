using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

namespace Complete
{
    public class GameManager : MonoBehaviour
    {
        public int m_NumRoundsToWin = 20;            // The number of rounds a single player has to win to win the game.
        public float m_StartDelay = 3f;             // The delay between the start of RoundStarting and RoundPlaying phases.

        public float m_EndDelay = 3f;               // The delay between the end of RoundPlaying and RoundEnding phases.
        public CameraControl m_CameraControl;       // Reference to the CameraControl script for control during different phases.
        public Text m_MessageText;                  // Reference to the overlay Text to display winning text, etc.
        public GameObject m_TankPrefab;             // Reference to the prefab the players will control.
        public UIPlayerList m_PlayerList;
        public List<TankManager> m_Tanks;
        public Transform[] spawnPoints;

        private int m_RoundNumber;                  // Which round the game is currently on.
        private WaitForSeconds m_StartWait;         // Used to have a delay whilst the round starts.
        private WaitForSeconds m_EndWait;           // Used to have a delay whilst the round or game ends.
        private TankManager m_RoundWinner;          // Reference to the winner of the current round.  Used to make an announcement of who won.
        private TankManager m_GameWinner;           // Reference to the winner of the game.  Used to make an announcement of who won.

        private ClientManager clientManager;        // ���������Ӵ�����
        private MessageManager msgManager;          // ��Ϣ������
        private PlayerManager playerManager;        // ��Ҵ�����

        private List<BaseManager> ManagerList = new List<BaseManager>(); // �������б�

        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.Find("GameManager").GetComponent<GameManager>();
                }
                return _instance;
            }
        }

        private void Start()
        {
            InitManager();
            // Create the delays so they only have to be made once.
            m_StartWait = new WaitForSeconds(m_StartDelay);
            m_EndWait = new WaitForSeconds(m_EndDelay);

            //SpawnAllTanks();


            // Once the tanks have been created and the camera is using them as targets, start the game.
            StartCoroutine(GameLoop());
        }

        /*
         * ��ʼ��������
         */
        private void InitManager()
        {
            clientManager = new ClientManager();
            msgManager = MessageManager.Instance;
            playerManager = PlayerManager.Instance;

            clientManager.ClientConnected += ClientConnected; // ע��ͻ��������¼�
            clientManager.ReceiveEvent += ReceiveEvent;       // ע�������Ϣ�¼�
            msgManager.SendEvent += SendMessage;              // ע����Ϣ�����¼�

            msgManager.OnInit();
            playerManager.OnInit();
            clientManager.OnInit();
        }

        private void SendMessage(object sender, ProtocalData e)
        {
            clientManager.Send(e);
        }

        private void ClientConnected(object sender, EventArgs e)
        {
            playerManager.CreateSyncPlayerInfo();
            ManagerList.Add(clientManager);
            ManagerList.Add(msgManager);
            ManagerList.Add(playerManager);
        }

        /// <summary>
        /// ������Ϣ�¼�������
        /// </summary>
        /// <param name="sender">�����¼�����</param>
        /// <param name="e">����</param>
        private void ReceiveEvent(object sender, ProtocalData e)
        {
            msgManager.Add(e);
        }

        private void Update()
        {
            ManagerList.ForEach(p => p.Update());
        }

        /// <summary>
        /// ����Tank
        /// </summary>
        /// <param name="tank"></param>
        /// <param name="playerNum"></param>
        /// <param name="color"></param>
        /// <param name="name"></param>
        /// <param name="localId"></param>
        public GameObject AddTank(int playerNum, Color color, string name, string localId, bool isLocalPlayer)
        {
            System.Random r = new System.Random();
            Transform t = spawnPoints[playerNum];
            TankManager tm = new TankManager
            {
                m_Instance = Instantiate(m_TankPrefab, t.position, t.rotation) as GameObject,
                m_PlayerNumber = playerNum,
                m_PlayerColor = color,
                m_playerName = name,
                m_LocalPlayerId = localId,
                m_SpawnPoint = t,
                isLocalPlayer = isLocalPlayer
            };
            tm.Setup();

            m_Tanks.Add(tm);

            SetCameraTargets();
            return tm.m_Instance;
        }

        /// <summary>
        /// ����Tank
        /// </summary>
        /// <param name="tank"></param>
        /// <param name="playerNum"></param>
        /// <param name="color"></param>
        /// <param name="name"></param>
        /// <param name="localId"></param>
        public GameObject AddTank(int playerNum, Color color, string name, string localId, Vector3 position, Quaternion rotation)
        {
            TankManager tm = new TankManager
            {
                m_Instance = Instantiate(m_TankPrefab, position, rotation) as GameObject,
                m_PlayerNumber = playerNum,
                m_PlayerColor = color,
                m_playerName = name,
                m_LocalPlayerId = localId,
                isLocalPlayer = false
            };
            tm.m_SpawnPoint = tm.m_Instance.transform;
            tm.Setup();

            m_Tanks.Add(tm);

            SetCameraTargets();
            return tm.m_Instance;
        }

        public void RemoveTank(string playerId)
        {
            lock (m_Tanks)
            {
                var tm = m_Tanks.FirstOrDefault(p => p.m_LocalPlayerId == playerId);

                if (tm != null)
                {
                    m_Tanks.Remove(tm);
                    SetCameraTargets();
                }
            }
        }

        private void SpawnAllTanks()
        {

            // For all the tanks...
            for (int i = 0; i < m_Tanks.Count; i++)
            {
                // ... create them, set their player number and references needed for control.
                m_Tanks[i].m_Instance =
                    Instantiate(m_TankPrefab, m_Tanks[i].m_SpawnPoint.position, m_Tanks[i].m_SpawnPoint.rotation) as GameObject;
                m_Tanks[i].m_PlayerNumber = i + 1;
                if (i == 0)
                {
                    m_Tanks[i].m_Instance.tag = "Player";
                }
                m_Tanks[i].Setup();
            }

        }


        private void SetCameraTargets()
        {
            // Create a collection of transforms the same size as the number of tanks.
            Transform[] targets = new Transform[m_Tanks.Count];

            // For each of these transforms...
            for (int i = 0; i < targets.Length; i++)
            {
                // ... set it to the appropriate tank transform.
                targets[i] = m_Tanks[i].m_Instance.transform;
            }

            // These are the targets the camera should follow.
            m_CameraControl.m_Targets = targets;
        }


        // This is called from start and will run each phase of the game one after another.
        private IEnumerator GameLoop()
        {
            while (m_Tanks.Count < 2) yield return null;
            // Start off by running the 'RoundStarting' coroutine but don't return until it's finished.
            yield return StartCoroutine(RoundStarting());

            // Once the 'RoundStarting' coroutine is finished, run the 'RoundPlaying' coroutine but don't return until it's finished.
            yield return StartCoroutine(RoundPlaying());

            // Once execution has returned here, run the 'RoundEnding' coroutine, again don't return until it's finished.
            yield return StartCoroutine(RoundEnding());

            // This code is not run until 'RoundEnding' has finished.  At which point, check if a game winner has been found.
            if (m_GameWinner != null)
            {
                // If there is a game winner, restart the level.
                SceneManager.LoadScene(0);
            }
            else
            {
                // If there isn't a winner yet, restart this coroutine so the loop continues.
                // Note that this coroutine doesn't yield.  This means that the current version of the GameLoop will end.
                StartCoroutine(GameLoop());
            }
        }


        private IEnumerator RoundStarting()
        {
            // As soon as the round starts reset the tanks and make sure they can't move.
            ResetAllTanks();
            DisableTankControl();

            // Snap the camera's zoom and position to something appropriate for the reset tanks.
            //m_CameraControl.SetStartPositionAndSize();

            // Increment the round number and display text showing the players what round it is.
            m_RoundNumber++;
            m_MessageText.text = "ROUND " + m_RoundNumber;

            // Wait for the specified length of time until yielding control back to the game loop.
            yield return m_StartWait;
        }


        private IEnumerator RoundPlaying()
        {
            // As soon as the round begins playing let the players control the tanks.
            EnableTankControl();

            // Clear the text from the screen.
            m_MessageText.text = string.Empty;

            // While there is not one tank left...
            while (!OneTankLeft())
            {
                // ... return on the next frame.
                yield return null;
            }
        }


        private IEnumerator RoundEnding()
        {
            // Stop tanks from moving.
            DisableTankControl();

            // Clear the winner from the previous round.
            m_RoundWinner = null;

            // See if there is a winner now the round is over.
            m_RoundWinner = GetRoundWinner();

            // If there is a winner, increment their score.
            if (m_RoundWinner != null)
                m_RoundWinner.m_Wins++;

            // Now the winner's score has been incremented, see if someone has one the game.
            m_GameWinner = GetGameWinner();

            // Get a message based on the scores and whether or not there is a game winner and display it.
            string message = EndMessage();
            m_MessageText.text = message;

            // Wait for the specified length of time until yielding control back to the game loop.
            yield return m_EndWait;
        }


        // This is used to check if there is one or fewer tanks remaining and thus the round should end.
        private bool OneTankLeft()
        {
            // Start the count of tanks left at zero.
            int numTanksLeft = 0;

            // Go through all the tanks...
            for (int i = 0; i < m_Tanks.Count; i++)
            {
                // ... and if they are active, increment the counter.
                if (m_Tanks[i].m_Instance.activeSelf)
                    numTanksLeft++;
            }

            // If there are one or fewer tanks remaining return true, otherwise return false.
            return m_Tanks.Count != 1 && numTanksLeft <= 1;
        }


        // This function is to find out if there is a winner of the round.
        // This function is called with the assumption that 1 or fewer tanks are currently active.
        private TankManager GetRoundWinner()
        {
            // Go through all the tanks...
            for (int i = 0; i < m_Tanks.Count; i++)
            {
                // ... and if one of them is active, it is the winner so return it.
                if (m_Tanks[i].m_Instance.activeSelf)
                    return m_Tanks[i];
            }

            // If none of the tanks are active it is a draw so return null.
            return null;
        }


        // This function is to find out if there is a winner of the game.
        private TankManager GetGameWinner()
        {
            // Go through all the tanks...
            for (int i = 0; i < m_Tanks.Count; i++)
            {
                // ... and if one of them has enough rounds to win the game, return it.
                if (m_Tanks[i].m_Wins == m_NumRoundsToWin)
                    return m_Tanks[i];
            }

            // If no tanks have enough rounds to win, return null.
            return null;
        }


        // Returns a string message to display at the end of each round.
        private string EndMessage()
        {
            // By default when a round ends there are no winners so the default end message is a draw.
            string message = "DRAW!";

            // If there is a winner then change the message to reflect that.
            if (m_RoundWinner != null)
                message = m_RoundWinner.m_ColoredPlayerText + " WINS THE ROUND!";

            // Add some line breaks after the initial message.
            message += "\n\n\n\n";

            // Go through all the tanks and add each of their scores to the message.
            for (int i = 0; i < m_Tanks.Count; i++)
            {
                message += m_Tanks[i].m_ColoredPlayerText + ": " + m_Tanks[i].m_Wins + " WINS\n";
            }

            // If there is a game winner, change the entire message to reflect that.
            if (m_GameWinner != null)
                message = m_GameWinner.m_ColoredPlayerText + " WINS THE GAME!";

            return message;
        }


        // This function is used to turn all the tanks back on and reset their positions and properties.
        private void ResetAllTanks()
        {
            for (int i = 0; i < m_Tanks.Count; i++)
            {
                m_Tanks[i].Reset();
            }
        }


        private void EnableTankControl()
        {
            for (int i = 0; i < m_Tanks.Count; i++)
            {
                m_Tanks[i].EnableControl();
            }
        }


        private void DisableTankControl()
        {
            for (int i = 0; i < m_Tanks.Count; i++)
            {
                m_Tanks[i].DisableControl();
            }
        }

        private void OnApplicationQuit()
        {
            this.ManagerList.ForEach(p => p.OnDestroy());
        }
    }
}