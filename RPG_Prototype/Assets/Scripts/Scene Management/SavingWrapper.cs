﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using Cinemachine;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";

        private void Awake()
        {
            StartCoroutine(LoadLastScene());
        }

        private IEnumerator LoadLastScene()
        {
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            SceneTransitionFader sceneTransitionFader = FindObjectOfType<SceneTransitionFader>();
            sceneTransitionFader.InstantFadeOut();
            yield return sceneTransitionFader.FadeIn(2);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                DeleteSaveFile();
            }
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public void DeleteSaveFile()
        {
            GetComponent<SavingSystem>().Delete(defaultSaveFile);
        }
    }
}
