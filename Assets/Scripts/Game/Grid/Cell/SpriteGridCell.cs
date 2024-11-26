
using Assets.Scripts.Grid;
using Assets.Scripts.Grid.Layers;
using UnityEngine;

namespace Assets.Scripts.Game.Grid.Cell
{
    /// <summary>
    /// SpriteGridCell is a cell that consists of three layers: background, sprite, and border.
    /// </summary>
    public class SpriteGridCell : MonoBehaviour
    {
        // Consists of the following layers
        private SpriteGridCellLayer m_bgCellLayer;
        public SpriteGridCellLayer BackgroundLayer { get { return m_bgCellLayer; } }
        private SpriteGridCellLayer m_spriteCellLayer;
        public SpriteGridCellLayer SpriteLayer { get { return m_spriteCellLayer; } }
        private SpriteGridCellLayer m_borderCellLayer;
        public SpriteGridCellLayer BorderLayer { get { return m_borderCellLayer; } }

        // GameObjects
        public SpriteGrid m_parentGrid;

        // Components
        private BoxCollider2D m_boxCollider;

        private SpriteGridCellInfo m_cellInfo;
        public SpriteGridCellInfo CellInfo { get { return m_cellInfo; } }

        /// <summary>
        /// Initializes the cell with the specified properties.
        /// </summary>
        /// <param name="parent">Parent GameObject.</param>
        /// <param name="cellId">ID that represents the cell.</param>
        /// <param name="x">X position of cell.</param>
        /// <param name="y">Y position of cell.</param>
        /// <param name="cellSize">Size of the cell.</param>
        /// <param name="cellSpacing">Spacing of the cell.</param>
        /// <param name="color">Color of cell.</param>
        public void Init(Transform parent, int cellId, int x, int y, float cellSize, float cellSpacing, Color color)
        {
            m_cellInfo = new SpriteGridCellInfo
            {
                CellID = cellId,
                X = x,
                Y = y,
                CellSize = cellSize,
                CellSpacing = cellSpacing,
                Color = color
            };

            name = string.Format("Cell_{0}_{1}", x, y);
            transform.SetParent(parent);
            transform.localScale = parent.localScale;
            transform.position = new Vector3(x * (cellSize + cellSpacing), y * (cellSize + cellSpacing), 0);

            m_parentGrid = parent.GetComponent<SpriteGrid>();

            m_boxCollider = gameObject.AddComponent<BoxCollider2D>();
            m_boxCollider.size = new Vector2(cellSize, cellSize);
            m_boxCollider.isTrigger = true;

            // Create a background layer.
            GameObject bgGameObject = new GameObject("BackgroundLayer");
            m_bgCellLayer = bgGameObject.AddComponent<SpriteGridCellLayer>();
            m_bgCellLayer.Create(new LayerProperties
            {
                SpriteName = "bg",
                CellSize = cellSize,
                CellSpacing = cellSpacing,
                SortingOrder = 0,
                Parent = transform
            });
            m_bgCellLayer.Load();

            // Create a sprite layer.
            GameObject spriteGameObject = new GameObject("SpriteLayer");
            m_spriteCellLayer = spriteGameObject.AddComponent<SpriteGridCellLayer>();
            m_spriteCellLayer.Create(new LayerProperties
            {
                SpriteName = "number_" + m_cellInfo.CellID,
                CellSize = cellSize,
                CellSpacing = cellSpacing,
                SortingOrder = 1,
                Parent = transform
            });
            m_spriteCellLayer.Load();

            // Create a border layer.
            GameObject borderGameObject = new GameObject("BorderLayer");
            m_borderCellLayer = borderGameObject.AddComponent<SpriteGridCellLayer>();
            m_borderCellLayer.Create(new LayerProperties
            {
                SpriteName = "bg_border",
                CellSize = cellSize,
                CellSpacing = cellSpacing,
                SortingOrder = 2,
                Parent = transform,
            });
            m_borderCellLayer.Load();

            //Finally, set a reference in the info block.
            m_cellInfo.Cell = this;

            // Set the color of the cell.
            SetColor(color.r, color.g, color.b);

            // Perform any minor scaling to all cell layers.
            AdjustLayerScale();
        }

        /// <summary>
        /// Sets the color of the background layer.
        /// </summary>
        /// <param name="r">red value.</param>
        /// <param name="g">green value.</param>
        /// <param name="b">blue value.</param>
        /// <param name="a">alpha value</param>
        public void SetColor(float r, float g, float b, float a = 1.0F)
        {
            m_bgCellLayer.UpdateColor(r, g, b, a);
        }

        /// <summary>
        /// Runs a pass on all the cell layers to adjust the scale,
        /// so that they will line up in unison
        /// </summary>
        private void AdjustLayerScale()
        {
            m_bgCellLayer.AdjustScale();
            m_spriteCellLayer.AdjustScale();
            m_borderCellLayer.AdjustScale();
        }
    }

    /// <summary>
    /// Represents the information of a SpriteGridCell.
    /// </summary>
    public struct SpriteGridCellInfo
    {
        public int CellID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public float CellSize { get; set; }
        public float CellSpacing { get; set; }
        public Color Color { get; set; }
        public SpriteGridCell Cell { get; set; }
    }
}
