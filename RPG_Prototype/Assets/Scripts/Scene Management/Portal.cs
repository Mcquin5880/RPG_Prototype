using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] int sceneIndexToLoad = -1;
        [SerializeField] Transform playerSpawnPoint;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                StartCoroutine(SceneTransition());
            }
        }

        private IEnumerator SceneTransition()
        {
            DontDestroyOnLoad(this.gameObject);
            yield return SceneManager.LoadSceneAsync(sceneIndexToLoad);

            Portal exitPortal = GetExitPortal();
            SpawnPlayerAtExitPortal(exitPortal);



            Destroy(gameObject);
        }

        private Portal GetExitPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;

                return portal;
            }

            return null;
        }

        private void SpawnPlayerAtExitPortal(Portal exitPortal)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = exitPortal.playerSpawnPoint.position;
            player.transform.rotation = exitPortal.playerSpawnPoint.rotation;
        }
    }
}
