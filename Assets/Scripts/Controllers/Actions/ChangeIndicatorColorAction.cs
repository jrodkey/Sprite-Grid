
using Assets.Scripts.Game.UI;
using UnityEngine;

namespace Assets.Scripts.Controllers.Actions
{
    /// <summary>
    /// Action that changes the color of the UI indicator.
    /// </summary>
    public class ChangeIndicatorColorAction : GameFlowAction
    {
        private ColorIndicator m_colorIndicator;
        private Color m_previousColor;
        private Color m_newColor;

        public ChangeIndicatorColorAction(ColorIndicator colorIndicator, Color newColor)
        {
            m_colorIndicator = colorIndicator;
            m_previousColor = colorIndicator.Color;
            m_newColor = newColor;
        }

        public override void Execute()
        {
            m_colorIndicator.Color = m_newColor;
        }

        public override void Undo()
        {
            m_colorIndicator.Color = m_previousColor;
        }
    }
}
