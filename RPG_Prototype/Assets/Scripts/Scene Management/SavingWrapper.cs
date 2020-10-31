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

        IEnumerator Start()
        {
            SceneTransitionFader sceneTransitionFader = FindObjectOfType<SceneTransitionFader>();
            sceneTransitionFader.InstantFadeOut();
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
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
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }
    }
}
