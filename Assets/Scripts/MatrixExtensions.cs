using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public static class MatrixExtensions
    {
        public static bool IsContains(this List<Matrix4x4> mList, Matrix4x4 m2, float tolerance = 1e-4f)
        {
            foreach (var x4 in mList)
            {
                if (IsEqual(x4, m2, tolerance))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsEqual(this Matrix4x4 m1, Matrix4x4 m2, float tolerance = 1e-4f)
        {
            for (int i = 0; i < 16; i++)
            {
                if (Mathf.Abs(m1[i] - m2[i]) > tolerance) return false;
            }

            return true;
        }
    }
}