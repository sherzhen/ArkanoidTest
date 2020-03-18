using UnityEngine;
using UnityEngine.UI;

namespace MiniIT.ArkanoidTest
{
    public class CanvasController : MonoBehaviour
    {
        [SerializeField]
        private GameObject startGameMenuGO = null;
        [SerializeField]
        private Text levelText = null;
        [SerializeField]
        private Text currentScoreText = null;
        [SerializeField]
        private Text bestScoreText = null;

        private bool isStartButtonPressed = false;
        public bool IsStartButtonPressed
        {
            get
            {
                return isStartButtonPressed;
            }

            set
            {
                isStartButtonPressed = value;

                startGameMenuGO.SetActive(!isStartButtonPressed);
            }
        }

        public void SetLevelText(int level)
        {
            levelText.text = "Level " + level.ToString();
        }

        public void SetScoreText(int currentScore, int bestScore)
        {
            currentScoreText.text = currentScore.ToString();
            bestScoreText.text = "Best: " + bestScore.ToString();
        }
    }
}