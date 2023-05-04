using UnityEngine;


namespace Utility
{
    internal class LifeManager : MonoBehaviour
    {
        #region Fields and Properties

        [SerializeField] private GameObject _lifeOne;
        [SerializeField] private GameObject _lifeTwo;
        [SerializeField] private GameObject _lifeThree;

        internal int Lives { get; private set; } = 3;

        #endregion

        #region Functions

        internal void LoseLife()
        {
            if (Lives > 0)
            {
                Lives--;
            }

            switch (Lives) 
            {
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
