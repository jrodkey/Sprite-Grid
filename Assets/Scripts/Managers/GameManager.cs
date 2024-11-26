
using Assets.Scripts.Controllers;
using Assets.Scripts.Controllers.Actions;
using Assets.Scripts.Game.Grid.Cell;
using Assets.Scripts.Game.UI;
using Assets.Scripts.Grid;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.Managers
{
    /// <summary>
    /// Manages the game flow and board.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public SpriteGrid SpriteGrid;
        public ColorIndicator ColorIndicator;
        public LogBook LogBook;

        private CameraManager m_cameraManager;
        private GameFlowControl m_gameFlowControl;

        public Color Color
        {
            get { return ColorIndicator.Color; }
        }

        public void Awake()
        {
            Assert.IsNotNull(SpriteGrid, "SpriteGrid is null");
            Assert.IsNotNull(ColorIndicator, "ColorIndicator is null");
            Assert.IsNotNull(LogBook, "LogBook is null");

            DontDestroyOnLoad(this);

            m_cameraManager = new CameraManager();
            m_gameFlowControl = new GameFlowControl();
            SpriteGrid.GridCreated += SpriteGrid_OnGridCreated;
            SpriteGrid.GridCellMouseDown += SpriteGrid_OnGridCellMouseDown;
        }

        /// <summary>
        /// Adds a cell color change action to the game flow control stack
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="newColor"></param>
        private void AddCellColorChangeAction(SpriteGridCell cell, Color newColor)
        {
            m_gameFlowControl.ExecuteAction(new ChangeCellColorAction(cell, Color));
        }

        /// <summary>
        /// Sends a message to the log book.
        /// </summary>
        /// <param name="message"></param>
        private void SendToLogBook(string message)
        {
            LogBook.Log(message);
        }

        /// <summary>
        /// Triggers when the grid is created.
        /// </summary>
        /// <param name="gridInfo"></param>
        private void SpriteGrid_OnGridCreated(SpriteGridInfo gridInfo)
        {
            m_cameraManager.SpriteGrid_OnGridCreated(gridInfo);
            m_gameFlowControl.ExecuteAction(new ChangeIndicatorColorAction(ColorIndicator, gridInfo.CurrentColor));

            SendToLogBook("Grid created. Current color is: " + gridInfo.CurrentColor.ToString());
        }

        /// <summary>
        /// Triggers when a cell is clicked.
        /// </summary>
        /// <param name="cellInfo"></param>
        private void SpriteGrid_OnGridCellMouseDown(SpriteGridCellInfo cellInfo)
        {
            if (cellInfo.Cell.BackgroundLayer.Properties.Color != Color)
            {
                m_gameFlowControl.ExecuteAction(new ChangeCellColorAction(cellInfo.Cell, Color));

                SendToLogBook("Cell color changed. Current color is: " + Color);
            }
        }

        /// <summary>
        /// Triggers when the undo button is clicked.
        /// </summary>
        public void Undo_OnClick()
        {
            m_gameFlowControl.Undo();

            SendToLogBook("Undo");
        }

        /// <summary>
        /// Triggers when the redo button is clicked.
        /// </summary>
        public void Redo_OnClick()
        {
            m_gameFlowControl.Redo();

            SendToLogBook("Redo");
        }
    }
}
