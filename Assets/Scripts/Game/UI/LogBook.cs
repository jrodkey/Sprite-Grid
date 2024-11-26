
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Assets.Scripts.Game.UI
{
    /// <summary>
    /// Represents a log book with a text area to display log entries.
    /// </summary>
    public class LogBook : MonoBehaviour
    {
        private const int MAX_LOG_LENGTH = 1000;

        private GameObject m_logBookContainer;
        private TextMeshProUGUI m_logText;
        private Button m_logButton;

        public void Awake()
        {
            m_logBookContainer = GameObject.Find("LogBookContainer");
            Assert.IsNotNull(m_logBookContainer, "LogBookContainer is null");

            m_logText = m_logBookContainer.GetComponentInChildren<TextMeshProUGUI>();
            Assert.IsNotNull(m_logText, "LogText is null");

            GameObject logsButtonGameObject = GameObject.Find("LogsButton");
            m_logButton = logsButtonGameObject.GetComponent<Button>();
            Assert.IsNotNull(m_logButton, "LogsButton is null");

            m_logText.overflowMode = TextOverflowModes.Truncate;
            m_logButton.onClick.AddListener(Button_OnClick);

            m_logBookContainer.SetActive(false);
        }

        public void OnDestroy()
        {
            m_logText = null;
            m_logButton = null;
            m_logBookContainer = null;
        }

        /// <summary>
        /// Logs a message to the log book.
        /// </summary>
        /// <param name="message">Log message.</param>
        public void Log(string message)
        {
            m_logText.text = message + "\n" + m_logText.text;
            
            Canvas.ForceUpdateCanvases();

            if (m_logText.text.Length > MAX_LOG_LENGTH)
            {
                m_logText.text = m_logText.text.Substring(0, MAX_LOG_LENGTH);
            }
        }

        /// <summary>
        /// Clears the log book.
        /// </summary>
        public void Clear()
        {
            m_logText.text = string.Empty;
        }

        /// <summary>
        /// Triggers when a log entry is received.
        /// </summary>
        /// <param name="message">Log message.</param>
        public void Button_OnClick()
        {
            m_logBookContainer.SetActive(!m_logBookContainer.activeSelf);
        }
    }
}
