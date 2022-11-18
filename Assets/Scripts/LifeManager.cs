using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameName;
using EnumCollection;


namespace GameName.Lives
{

    //this class handles the usage of player lives and their
    //simple visual representation as buttons or sprites
    //meant to be a component of the GameManager
    public class LifeManager : MonoBehaviour
    {
        #region Fields

        //GameObjects (buttons or sprites) that represent the
        //player lives need to be dragged into the script
        [SerializeField] private GameObject _lifeOne;
        [SerializeField] private GameObject _lifeTwo;
        [SerializeField] private GameObject _lifeThree;

        #endregion

        #region Properties

        //lives property allows to get the remaining player
        //lives from other scripts
        private int _lives;

        public int Lives
        {
            get { return _lives; }
            private set { _lives = value; }
        }


        #endregion

        #region Public Functions

        //public function to be called by other scripts
        //for example on collision with enemies or similar
        public void LoseLife()
        {
            if (_lives > 0)
            {
                _lives--;
            }

            //switches visual representation of lives on and off
            //depending on leftover lives
            //has to be extended for further lives than 3
            switch (_lives) 
            {
                //case with full lives is necessary for new game
                //will reset the life counter on call
                //call this in GameState "NewGame"
                case 3:
                    _lifeOne.SetActive(true); 
                    _lifeTwo.SetActive(true);
                    _lifeThree.SetActive(true);                 
                    break;

                case 2:
                    _lifeThree.SetActive(false);
                    break;

                case 1:
                    _lifeTwo.SetActive(false);
                    break;

                case 0:
                    _lifeOne.SetActive(false);
                    GameManager.Instance.SwitchState(GameState.GameOver);
                    break;

            }

        }

        #endregion
    }
}
