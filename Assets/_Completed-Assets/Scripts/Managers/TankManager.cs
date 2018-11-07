using System;
using UnityEngine;

namespace Complete
{
    [Serializable]
    public class TankManager
    {
        // This class is to manage various settings on a tank.
        // It works with the GameManager class to control how the tanks behave
        // and whether or not players have control of their tank in the 
        // different phases of the game.

        public Color m_PlayerColor;                             // This is the color this tank will be tinted.
        public Transform m_SpawnPoint;                          // The position and direction the tank will have when it spawns.
        [HideInInspector] public int m_PlayerNumber;            // This specifies which player this the manager for.
        [HideInInspector] public string m_ColoredPlayerText;    // A string that represents the player with their number colored to match their tank.
        [HideInInspector] public GameObject m_Instance;         // A reference to the instance of the tank when it is created.
        [HideInInspector] public int m_Wins;                    // The number of wins this player has so far.
        [HideInInspector] public string m_LocalPlayerId;
        [HideInInspector] public string m_playerName;
        [HideInInspector] public bool isLocalPlayer;


        private TankMovement m_Movement;                        // Reference to tank's movement script, used to disable and enable control.
        private TankShooting m_Shooting;                        // Reference to tank's shooting script, used to disable and enable control.
        private GameObject m_CanvasGameObject;                  // Used to disable the world space UI during the Starting and Ending phases of each round.
        private TankName m_TankName;
        private TankHealth m_Health;

        public void Setup()
        {
            // Get references to the components.
            m_Movement = m_Instance.GetComponent<TankMovement>();
            m_Shooting = m_Instance.GetComponent<TankShooting>();
            m_Health = m_Instance.GetComponent<TankHealth>();
            m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;
            m_TankName = m_Instance.AddComponent<TankName>();

            // Set the player numbers to be consistent across the scripts.
            m_Movement.m_PlayerNumber = m_PlayerNumber;
            m_Movement.m_LoaclId = m_LocalPlayerId;
            m_Movement.isLocalPlayer = isLocalPlayer;

            m_Shooting.m_PlayerNumber = m_PlayerNumber;
            m_Shooting.m_LoaclId = m_LocalPlayerId;
            m_Shooting.isLocalPlayer = isLocalPlayer;

            m_Health.isLocalPlayer = isLocalPlayer;

            m_Instance.name = "Player-" + m_LocalPlayerId;
            m_TankName.playerName = m_Instance.name;

            if (isLocalPlayer)
            {
                //Camera.main.GetComponent<CameraFollow>().player = m_Instance.transform;
                m_Shooting.ShootEvent += M_Shooting_ShootEvent;
                m_Health.HealthEvent += M_Health_HealthEvent;
            }

            // Create a string using the correct color that says 'PLAYER 1' etc based on the tank's color and the player's number.
            m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + "> " + m_Instance.name + "</color>";

            // Get all of the renderers of the tank.
            MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer>();

            //Go through all the renderers...
            for (int i = 0; i < renderers.Length; i++)
            {
                // ... set their material color to the color specific to this tank.
                renderers[i].material.color = m_PlayerColor;
            }
        }

        private void M_Health_HealthEvent(object sender, float health)
        {
            PlayerManager.Instance.TakeDamage(health);
        }

        private void M_Shooting_ShootEvent(object sender, float e)
        {
            PlayerManager.Instance.Shoot(e);
        }


        // Used during the phases of the game where the player shouldn't be able to control their tank.
        public void DisableControl()
        {
            m_Movement.enabled = false;
            m_Shooting.enabled = false;

            m_CanvasGameObject.SetActive(false);
        }


        // Used during the phases of the game where the player should be able to control their tank.
        public void EnableControl()
        {
            m_Movement.enabled = true;
            m_Shooting.enabled = true;

            m_CanvasGameObject.SetActive(true);
        }


        // Used at the start of each round to put the tank into it's default state.
        public void Reset()
        {
            m_Instance.transform.position = m_SpawnPoint.position;
            m_Instance.transform.rotation = m_SpawnPoint.rotation;

            m_Instance.SetActive(false);
            m_Instance.SetActive(true);
        }
    }
}