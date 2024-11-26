
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Assets.Scripts.Game.UI
{
    /// <summary>
    /// Represents a color variant.
    /// </summary>
    public class ColorVariant : MonoBehaviour
    {
        public delegate void OnColorVariantClicked(ColorVariant colorVariant);
        public event OnColorVariantClicked ColorVariantClicked;

        public Image Color;
        public Image Border;

        private Color m_color = UnityEngine.Color.white;

        private RectTransform m_rectTransform;
        public RectTransform RectTransform
        {
            get { return m_rectTransform; }
        }

        void Awake()
        {
            Assert.IsNotNull(Color);
            Assert.IsNotNull(Border);

            Color.color = m_color;
            m_rectTransform = GetComponent<RectTransform>();
        }

        void OnDestroy()
        {
            Color = null;
            Border = null;
        }

        public void SetColor(Color color) 
        {
            m_color = color;
            if (Color == null)
            {
                return;
            }

            Color.color = m_color;
        }

        /// <summary>
        /// Assigns the new position to the color variant, then animates the 
        /// movement over a fixed duration.
        /// </summary>
        /// <param name="position">Position of destination.</param>
        public void GoToPosition(Vector3 position)
        {
            StartCoroutine(MoveToPosition(position, 0.25f));
        }

        /// <summary>
        /// Moves to the target position given a duration.
        /// </summary>
        /// <param name="targetPosition"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        private IEnumerator MoveToPosition(Vector3 targetPosition, float duration)
        {
            Debug.Log("MoveToPosition | Start: " + transform.position);
            Vector3 startPosition = transform.position;
            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPosition;
            Debug.Log("MoveToPosition | End: " + targetPosition);

            if (transform.position == targetPosition)
            {
                Border.color = UnityEngine.Color.white;
            }
        }

        /// <summary>
        /// Triggered when the color variant is clicked.
        /// </summary>
        public void ColorVariant_OnClick()
        {
            //Border.color = UnityEngine.Color.red;
            ColorVariantClicked?.Invoke(this);
        }
    }
}
