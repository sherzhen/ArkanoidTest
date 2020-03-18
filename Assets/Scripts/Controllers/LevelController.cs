using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace MiniIT.ArkanoidTest
{
    public class LevelController : MonoBehaviour
    {
        #region Variables init

        [Header("Block Settings")]
        [SerializeField]
        private GameObject[] blockPrefabs = null;
        [Tooltip("It is necessary to determine the distance between the blocks by X when generating a new level")]
        [SerializeField]
        private float blockSizeX = 0.64f;
        [Tooltip("It is necessary to determine the distance between the blocks by Y when generating a new level")]
        [SerializeField]
        private float blockSizeY = 0.32f;

        [Header("Audio Settings")]
        [SerializeField]
        private AudioSource audioSource = null;
        [SerializeField]
        private AudioClip[] clips = null;
        private enum ClipsName
        {
            BrickImpact = 0,
            BrickExplosion = 1,
            WinSound = 2,
        }
        #endregion

        #region Level generation

        /// <summary>Returns true when level has been created</summary>
        public bool IsLevelGenerated { get; private set; } = false;

        private List<GameObject> createdBlocks = null;

        /// <summary>Level bounds by X</summary>
        private const float boundX = 2.24f;
        /// <summary>Level bounds by Y</summary>
        private const float boundY = 0.32f;

        private int blocksCount = 0;

        /// <summary>Generates a level from blocks based on the current level</summary>
        /// <param name="levelNum">Current level</param>
        public void GenerateLevel(int levelNum)
        {
            IsLevelGenerated = false;

            createdBlocks = new List<GameObject>();

            for (float x = -boundX; x <= boundX; x += blockSizeX)
            {
                for (float y = -boundY * levelNum; y <= boundY * levelNum; y += blockSizeY)
                {
                    GameObject newBlock = Instantiate(blockPrefabs[Random.Range(0, blockPrefabs.Length)], transform);
                    newBlock.transform.position = new Vector2(x, y);
                    newBlock.GetComponent<BlockController>().OnExplodeAction += OnBlockDestroyed;
                    newBlock.SetActive(false);

                    createdBlocks.Add(newBlock);
                }
            }

            StartCoroutine(LevelGenerationEffect());
        }

        private IEnumerator LevelGenerationEffect()
        {
            // Randomly activates all created blocks
            for (int i = 0; i < createdBlocks.Count; i++)
            {
                int rand = 0;
                do
                {
                    rand = Random.Range(0, createdBlocks.Count);
                }
                while (createdBlocks[rand].activeInHierarchy);

                createdBlocks[rand].SetActive(true);

                audioSource.pitch = Random.Range(0.8f, 1.2f);
                audioSource.PlayOneShot(clips[(int)ClipsName.BrickImpact]);

                yield return new WaitForSeconds(0.1f);
            }

            blocksCount = createdBlocks.Count;
            IsLevelGenerated = true;
        }
        #endregion

        #region Level removing

        /// <summary>Returns true when blocks have been destroyed</summary>
        public bool IsAllBlocksDestroyed { get; set; } = false;

        /// <summary>Returns true when level has been removed</summary>
        public bool IsLevelRemoved { get; private set; } = false;

        /// <summary>Remove all created blocks</summary>
        public void RemoveLevel()
        {
            IsLevelRemoved = false;

            StartCoroutine(LevelRemovalEffect());
        }

        private IEnumerator LevelRemovalEffect()
        {
            // If all blocks haven't been destroyed...
            if (!IsAllBlocksDestroyed)
            {
                // We play explosion sound of the remaining blocks...
                audioSource.pitch = 1;
                audioSource.PlayOneShot(clips[(int)ClipsName.BrickExplosion]);

                // Hide active blocks and play their particles
                for (int i = 0; i < createdBlocks.Count; i++)
                {
                    if (createdBlocks[i].GetComponent<SpriteRenderer>().enabled)
                    {
                        createdBlocks[i].GetComponent<SpriteRenderer>().enabled = false;
                        createdBlocks[i].GetComponent<ParticleSystem>().Play();
                    }
                }

                yield return new WaitForSeconds(1);
            }
            else
            {
                IsAllBlocksDestroyed = false;
            }

            // Then after particles has passed, we destroy all the blocks
            for (int i = createdBlocks.Count - 1; i >= 0; i--)
            {
                Destroy(createdBlocks[i]);
            }

            IsLevelRemoved = true;
        }
        #endregion

        public UnityAction OnBlockDestroyedAction { get; set; } = null;

        private void OnBlockDestroyed()
        {
            OnBlockDestroyedAction.Invoke();

            blocksCount--;

            if (blocksCount <= 0)
            {
                audioSource.pitch = 1;
                audioSource.PlayOneShot(clips[(int)ClipsName.WinSound]);

                IsAllBlocksDestroyed = true;
            }
        }
    }
}