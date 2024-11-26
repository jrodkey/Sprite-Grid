
using Assets.Scripts.Game.Grid.Cell;
using UnityEngine;

namespace Assets.Scripts.Controllers.Actions
{
    /// <summary>
    /// Action that changes the color of the cell.
    /// </summary>
    public class ChangeCellColorAction : GameFlowAction
    {
        private SpriteGridCell cell;
        private Color previousColor;
        private Color newColor;

        public ChangeCellColorAction(SpriteGridCell cell, Color newColor)
        {
            this.cell = cell;
            this.previousColor = cell.CellInfo.Color;
            this.newColor = newColor;
        }

        public override void Execute()
        {
            cell.SetColor(newColor.r, newColor.g, newColor.b, newColor.a);
        }

        public override void Undo()
        {
            cell.SetColor(previousColor.r, previousColor.g, previousColor.b, previousColor.a);
        }
    }
}
