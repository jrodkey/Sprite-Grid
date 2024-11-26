
using Assets.Scripts.Grid;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class CameraManager
    {
        /// <summary>
        /// Readjusts the camera to fit the grid.
        /// </summary>
        /// <param name="gridInfo">Grid info.</param>
        private void AdjustCamera(SpriteGridInfo gridInfo)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("Main camera not found");
                return;
            }

            if (!gridInfo.Validate())
            {
                Debug.LogError("Invalid grid dimensions");
                return;
            }

            // Calculate the size of the grid
            float gridWidth = gridInfo.Width * (gridInfo.CellSize + gridInfo.CellSpacing) - gridInfo.CellSpacing;
            float gridHeight = gridInfo.Height * (gridInfo.CellSize + gridInfo.CellSpacing) - gridInfo.CellSpacing;

            // Apply padding factor
            gridWidth *= 1.2f;
            gridHeight *= 1.35f;

            // Calculate the orthographic size based on the grid dimensions and
            // center the camera on the grid.
            float aspectRatio = mainCamera.aspect;
            float cameraSize = Mathf.Max(gridHeight / 2, gridWidth / (2 * aspectRatio));
            mainCamera.orthographicSize = cameraSize;
            mainCamera.transform.position = new Vector3(0, 0, -10);
        }

        /// <summary>
        /// Triggers when the grid is created.
        /// </summary>
        /// <param name="gridInfo">Grid info.</param>
        public void SpriteGrid_OnGridCreated(SpriteGridInfo gridInfo)
        {
            AdjustCamera(gridInfo);
        }
    }
}
