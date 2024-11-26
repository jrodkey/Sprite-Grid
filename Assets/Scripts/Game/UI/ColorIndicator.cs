
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Game.UI
{
    /// <summary>
    /// Root component that handles the generatation of the color variants and manages 
    /// all of the associated GameObject.
    /// </summary>
    public class ColorIndicator : MonoBehaviour
    {
        public delegate void OnColorIndicatorChanged(ColorIndicator colorIndicator);
        public event OnColorIndicatorChanged ColorIndicatorChanged;

        public ColorVariant ColorVariantPrefab;

        private readonly List<Color> Colors = new List<Color>
        {
            Color.black,
            Color.red,
            Color.green,
            Color.blue,
            Color.yellow,
            Color.cyan,
            Color.magenta,
            Color.white,
            Color.gray,
        };

        private List<ColorVariant> m_colorVariants = new List<ColorVariant>();
        private RectTransform m_rectTransform;
        private Image m_image;
        private float m_offsetSpacing = 15.0f;
        private bool m_childrenVisible = false;

        public RectTransform RectTransform
        {
            get { return m_rectTransform; }
        }

        public Color Color
        {
            get { return m_image.color; }
            set
            {
                m_image.color = value;
                ColorIndicatorChanged?.Invoke(this);
            }
        }
        void Awake()
        {
            m_rectTransform = GetComponent<RectTransform>();
            m_image = GetComponent<Image>();

            BuildVariants();
        }

        void OnDestroy()
        {
            RemoveAll();
        }

        /// <summary>
        /// Builds the color variants based on the list of colors.
        /// </summary>
        private void BuildVariants()
        {
            RemoveAll();

            foreach (var variant in Colors)
            {
                AddColorVariant(variant);
            }

            // Ensure the color indicator is on top of the hierarchy.
            transform.SetAsLastSibling();
        }

        /// <summary>
        /// Sets the visibility of the children color variants and moves them to the correct position.
        /// </summary>
        /// <param name="isVisible">If true, sets the specified visibilty.</param>
        public void SetChildrenVisible(bool isVisible)
        {
            if (isVisible)
            {
                for (int i = 0; i < m_colorVariants.Count; ++i)
                {
                    float height = m_colorVariants[i].RectTransform.rect.height;
                    m_colorVariants[i].GoToPosition(
                        new Vector3(m_colorVariants[i].transform.position.x,
                                    m_colorVariants[i].transform.position.y + (height * (i + 1) + m_offsetSpacing),
                                    m_colorVariants[i].transform.position.z));
                }
            }
            else
            {
                for (int i = 0; i < m_colorVariants.Count; ++i)
                {
                    m_colorVariants[i].GoToPosition(transform.position);
                }
            }

            m_childrenVisible = isVisible;
        }

        /// <summary>
        /// Adds a color variant to the list with the specified color and initializes it.
        /// </summary>
        /// <param name="color">Assigned color to variant.</param>
        public void AddColorVariant(Color color)
        {
            ColorVariant colorVariant = Instantiate(ColorVariantPrefab, transform.parent);
            colorVariant.SetColor(color);
            colorVariant.gameObject.transform.position = transform.position;
            colorVariant.ColorVariantClicked += ColorVariant_OnClicked;

            m_colorVariants.Add(colorVariant);
        }

        /// <summary>
        /// Destroys all color variants and clears the list.
        /// </summary>
        public void RemoveAll()
        {
            foreach (ColorVariant colorVariant in m_colorVariants)
            {
                Destroy(colorVariant.gameObject);
            }

            m_colorVariants.Clear();
        }

        /// <summary>
        /// Triggers when a color variant is clicked.
        /// </summary>
        /// <param name="colorVariant"></param>
        public void ColorVariant_OnClicked(ColorVariant colorVariant)
        {
            Color = colorVariant.Color.color;
            ColorIndicatorChanged?.Invoke(this);
        }

        /// <summary>
        /// Triggers when the color indicator is clicked.
        /// </summary>
        public void ColorIndicator_OnClick()
        {
            SetChildrenVisible(!m_childrenVisible);
        }
    }
}
