using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class SolutionsVisualizer : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button findSolutionButton;
        [SerializeField] private Button visualizeSolutionButton;
        [SerializeField] private Button clearSpaceButton;

        [SerializeField] private GameObject spacePointPrefab;
        [SerializeField] private GameObject modelPointPrefab;

        [SerializeField] private Transform spaceParent;

        [SerializeField] private SolutionFinder solutionFinder;
        
        private void Awake()
        {
            findSolutionButton.onClick.AddListener(OnFindSolutionClicked);
            visualizeSolutionButton.onClick.AddListener(OnVisualizeSolutionClicked);
            clearSpaceButton.onClick.AddListener(ClearSpace);

            visualizeSolutionButton.interactable = false;
            clearSpaceButton.interactable = false;
        }

        private void OnDestroy()
        {
            findSolutionButton.onClick.RemoveAllListeners();
            visualizeSolutionButton.onClick.RemoveAllListeners();
            clearSpaceButton.onClick.RemoveAllListeners();
        }

        private void OnFindSolutionClicked()
        {
            findSolutionButton.interactable = false;
            solutionFinder.FindSolutions(() =>
            {
                visualizeSolutionButton.interactable = true;
                clearSpaceButton.interactable = true;
            });
        }
        
        private void OnVisualizeSolutionClicked()
        {
            if (Int32.TryParse(inputField.text, out var solutionIdx))
            {
                if (solutionIdx >= solutionFinder.Solutions.Count)
                {
                    Debug.LogWarning("Incorrect solution idx.");
                    return;
                }

                VisualizeSolution(solutionIdx);
            }
            else
            {
                Debug.LogWarning("Incorrect solution idx string.");
            }
        }

        private void VisualizeSolution(int solutionIdx)
        {
            ClearSpace();
            FillSpace(solutionIdx);
        }

        private void FillSpace(int solutionIdx)
        {
            if (solutionIdx < 0 || solutionIdx >= solutionFinder.Solutions.Count)
            {
                Debug.LogWarning("Try fill space with incorrect solution idx");
                return;
            }

            var solution = solutionFinder.Solutions[solutionIdx];
            var transformedModels = new List<Matrix4x4>();
            foreach (var modelMatrix in solutionFinder.Model)
            {
                transformedModels.Add(solution * modelMatrix);
            }

            foreach (var spacePointMatrix in solutionFinder.Space)
            {
                var pointGameObject = (GameObject)null;
                if (transformedModels.IsContains(spacePointMatrix))
                {
                    pointGameObject = Pool.Instance.Spawn(modelPointPrefab);
                }
                else
                {
                    pointGameObject = Pool.Instance.Spawn(spacePointPrefab);
                }

                pointGameObject.transform.SetParent(spaceParent);
                pointGameObject.transform.position = spacePointMatrix.GetPosition();
                pointGameObject.transform.rotation = spacePointMatrix.rotation;
                pointGameObject.transform.localScale = new Vector3(spacePointMatrix.GetColumn(0).magnitude, spacePointMatrix.GetColumn(1).magnitude,
                    spacePointMatrix.GetColumn(2).magnitude);
            }
        }

        private void ClearSpace()
        {
            while (spaceParent.childCount > 0)
            {
                Pool.Instance.Despawn(spaceParent.GetChild(0).gameObject);
            }
        }
    }
}