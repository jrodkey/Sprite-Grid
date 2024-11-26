using Assets.Scripts.Grid.Layers.Common;
using UnityEngine;

namespace Assets.Scripts.Grid.Layers
{
    /// <summary>
    /// Consists of the base class for all cell layers.
    /// </summary>
    public class SpriteGridCellLayer : MonoBehaviour, ICellLayer, IScaleModification, IPostDrawModify
    {
        public LayerProperties Properties { get; set; }

        public SpriteRenderer SpriteRenderer { get; set; }

        public Sprite Sprite { get; set; }

        /// <summary>
        /// Creates the layer and sets the properties.
        /// </summary>
        /// <param name="properties"></param>
        public virtual void Create(LayerProperties properties)
        {
            Properties = properties;
            transform.SetParent(Properties.Parent);
            transform.position = Properties.Parent.position;
            SpriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            SpriteRenderer.sortingOrder = Properties.SortingOrder;
        }

        /// <summary>
        /// Executes the sprite load operation for the layer.
        /// </summary>
        public virtual void Load() 
        {
            Sprite = Resources.Load<Sprite>(Properties.SpriteName);
            if (Sprite != null)
            {
                SpriteRenderer.sprite = Sprite;
                SpriteRenderer.drawMode = SpriteDrawMode.Sliced;
            }
            else
            {
                throw new System.Exception("Failed to load sprite for: " + name);
            }
        }

        /// <summary>
        /// Updates the color of the layer.
        /// </summary>
        public virtual void UpdateColor(float r, float g, float b, float a = 1.0f)
        {
            Properties.Color = new Color(r, g, b, a);
            SpriteRenderer.color = Properties.Color;
        }

        /// <summary>
        /// Applies modifications to the layer.
        /// </summary>
        public virtual void ApplyModifications() {}

        /// <summary>
        /// Adjusts the scale of the layer to fit the cell size.
        /// </summary>
        public virtual void AdjustScale() 
        {
            Vector3 parentScale = transform.localScale;
            float xScaleFactor = Properties.CellSize / SpriteRenderer.sprite.bounds.size.x;
            float yScaleFactor = Properties.CellSize / SpriteRenderer.sprite.bounds.size.y;
            transform.localScale = new Vector3(xScaleFactor / parentScale.x, yScaleFactor / parentScale.y, 1);
        }
    }

    /// <summary>
    /// Property set for a layer.
    /// </summary>
    public class LayerProperties
    {
        public Color Color { get; set; }
        public string SpriteName { get; set; }
        public float CellSize { get; set; }
        public float CellSpacing { get; set; }
        public Transform Parent { get; set; }
        public int SortingOrder { get; set; }
    }
}
