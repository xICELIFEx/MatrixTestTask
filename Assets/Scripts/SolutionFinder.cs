using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class SolutionFinder : MonoBehaviour
    {
        private List<Matrix4x4> model = new();
        private List<Matrix4x4> space = new();

        private List<Matrix4x4> solutions = new();
        
        public IReadOnlyList<Matrix4x4> Model => model;
        public IReadOnlyList<Matrix4x4> Space => space;

        public IReadOnlyList<Matrix4x4> Solutions => solutions;
        
        public void FindSolutions(Action onComplete)
        {
            model = MatrixSource.GenerateMatrixFormText("model");
            space = MatrixSource.GenerateMatrixFormText("space");

            StartCoroutine(Check(onComplete));
        }
        
        private IEnumerator Check(Action onComplete)
        {
            solutions.Clear();

            var transformations = new List<Matrix4x4>();

            var modelMatrix = model[0];
            for (int j = 0; j < space.Count; j++)
            {
                var spaceMatrix = space[j];
                var transformationMatrix = spaceMatrix * modelMatrix.inverse;
                if (transformations.IsContains(transformationMatrix))
                {
                    continue;
                }

                transformations.Add(transformationMatrix);
                yield return null;
                if (CheckSolution(model, space, transformationMatrix))
                {
                    solutions.Add(transformationMatrix);
                }
            }
            
            Debug.Log($"Found {solutions.Count} solutions.");
            onComplete?.Invoke();
        }

        public bool CheckSolution(List<Matrix4x4> model, List<Matrix4x4> space, Matrix4x4 solutionMatrix)
        {
            for (int i = 0; i < model.Count; i++)
            {
                var modelMatrix = model[i];
                var transformedMatrix = solutionMatrix * modelMatrix;
                var found = false;
                for (int j = 0; j < space.Count; j++)
                {
                    var spaceMatrix = space[j];
                    if (spaceMatrix.IsEqual(transformedMatrix))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    return false;
                }
            }

            return true;
        }
    }
}