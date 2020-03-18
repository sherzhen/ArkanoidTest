using UnityEngine;

namespace MiniIT.ArkanoidTest
{
    public class GameObserver : MonoBehaviour
    {
        #region Variables init

        [SerializeField]
        private CanvasController canvasController = null;

        [SerializeField]
        private LevelController levelGenerator = null;

        [SerializeField]
        private CameraController cameraController = null;

        [SerializeField]
        private PlatformController platformController = null;

        [SerializeField]
        private BallController ballController = null;

        private enum GameState
        {
            WaitForStart = 0,
            LevelGeneration = 1,
            Gameplay = 2,
            GameEnd = 3,
        }

        private GameState currentGameState = GameState.WaitForStart;

        private Color currentBGColor;
        private int currentLevel = 0;
        private int currentScore = 0;
        private int bestScore = 0;

        private void Start()
        {
            // Set starting values ​​if we enter the game for the first time
            if (PlayerPrefs.GetInt("IsFirstTime") == 0)
            {
                PlayerPrefs.SetInt("Current Level", 1);

                // Starting values of camera bg color
                PlayerPrefs.SetFloat("Color R", 1);
                PlayerPrefs.SetFloat("Color G", 1);
                PlayerPrefs.SetFloat("Color B", 1);

                PlayerPrefs.SetInt("IsFirstTime", 1);
            }

            currentBGColor = new Color(PlayerPrefs.GetFloat("Color R"), PlayerPrefs.GetFloat("Color G"), PlayerPrefs.GetFloat("Color B"));
            cameraController.SetBackgroundColor(currentBGColor);

            // Add point to the total score if block was destroyed
            levelGenerator.OnBlockDestroyedAction += AddPoint;

            currentScore = PlayerPrefs.GetInt("Current Score");
            bestScore = PlayerPrefs.GetInt("Best Score");
            canvasController.SetScoreText(currentScore, bestScore);

            currentLevel = PlayerPrefs.GetInt("Current Level");
            canvasController.SetLevelText(currentLevel);
        }
        #endregion

        #region Game observe

        void Update()
        {
            switch (currentGameState)
            {
                case GameState.WaitForStart:

                    if (canvasController.IsStartButtonPressed)
                    {
                        levelGenerator.GenerateLevel(currentLevel);

                        currentGameState = GameState.LevelGeneration;
                    }

                    break;

                case GameState.LevelGeneration:

                    if (levelGenerator.IsLevelGenerated)
                    {
                        ballController.gameObject.SetActive(true);
                        platformController.StartMoving();

                        currentGameState = GameState.Gameplay;
                    }

                    break;

                case GameState.Gameplay:

                    // If we win or fail...
                    if (levelGenerator.IsAllBlocksDestroyed || ballController.IsBallDestroyed)
                    {
                        // We checking if we win...
                        if (levelGenerator.IsAllBlocksDestroyed)
                        {
                            // Increase and save current level...
                            currentLevel++;
                            PlayerPrefs.SetInt("Current Level", currentLevel);

                            // Create and save new random color...
                            currentBGColor = Random.ColorHSV(0, 1, 0.2f, 0.4f, 1, 1);
                            PlayerPrefs.SetFloat("Color R", currentBGColor.r);
                            PlayerPrefs.SetFloat("Color G", currentBGColor.g);
                            PlayerPrefs.SetFloat("Color B", currentBGColor.b);

                            // Set this random color to camera bg...
                            cameraController.SetBackgroundColor(currentBGColor);
                        }
                        // If we fail...
                        else
                        {
                            // Zero and save current score...
                            currentScore = 0;
                            PlayerPrefs.SetInt("Current Score", currentScore);
                        }

                        levelGenerator.RemoveLevel();

                        currentGameState = GameState.GameEnd;
                    }

                    break;

                case GameState.GameEnd:

                    // When camera changed color...
                    if (cameraController.IsCameraColorChanged && levelGenerator.IsLevelRemoved)
                    {
                        // We reset player objects...
                        ballController.ResetBall();
                        platformController.ResetPlatform();

                        // And display the current results
                        canvasController.SetLevelText(currentLevel);
                        canvasController.SetScoreText(currentScore, bestScore);
                        canvasController.IsStartButtonPressed = false;

                        currentGameState = GameState.WaitForStart;
                    }

                    break;
            }
        }
        #endregion

        private void AddPoint()
        {
            currentScore++;
            PlayerPrefs.SetInt("Current Score", currentScore);

            if (currentScore > bestScore)
            {
                bestScore = currentScore;
                PlayerPrefs.SetInt("Best Score", bestScore);
            }

            canvasController.SetScoreText(currentScore, bestScore);
        }
    }
}