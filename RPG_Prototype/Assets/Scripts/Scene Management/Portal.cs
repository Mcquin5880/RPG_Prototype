using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationID
        {
            A, B, C, D, E
        }

        [SerializeField] int sceneIndexToLoad = -1;
        [SerializeField] Transform playerSpawnPoint;
        [SerializeField] DestinationID destination;

        [SerializeField] float fadeOutTime = 1f;
        [SerializeField] float fadeInTime = 2f;
        [SerializeField] float fadePauseTime = 0.5f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                StartCoroutine(SceneTransition());
            }
        }

        private IEnumerator SceneTransition()
        {
            if (sceneIndexToLoad < 0)
            {
                Debug.LogError("Scene to load not set");
                yield break;
            }

            SceneTransitionFader fader = FindObjectOfType<SceneTransitionFader>();

            DontDestroyOnLoad(this.gameObject);

            yield return fader.FadeOut(fadeOutTime);

            // save current level here
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            savingWrapper.Save();

            yield return SceneManager.LoadSceneAsync(sceneIndexToLoad);

            // load current level state here
            savingWrapper.Load();


            Portal exitPortal = GetExitPortal();
            SpawnPlayerAtExitPortal(exitPortal);

            savingWrapper.Save();

            yield return new WaitForSeconds(fadePauseTime);
            yield return fader.FadeIn(fadeInTime);

            Destroy(gameObject);
        }

        private Portal GetExitPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this || portal.destination != destination) continue;

                return portal;
            }

            return null;
        }

        private void SpawnPlayerAtExitPortal(Portal exitPortal)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.transform.position = exitPortal.playerSpawnPoint.position;
            player.transform.rotation = exitPortal.playerSpawnPoint.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}
