
using Assets.Scripts.Game.Grid.Cell;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Assertions;

namespace Assets.Scripts.Grid
{
    /// <summary>
    /// Represents a grid of cells.
    /// </summary>
    public class SpriteGrid : MonoBehaviour
    {
        // Events
        public delegate void OnGridCellMouseDown(SpriteGridCellInfo cellInfo);
        public event OnGridCellMouseDown GridCellMouseDown;
        public delegate void OnGridCreated(SpriteGridInfo gridInfo);
        public event OnGridCreated GridCreated;

        [SerializeField]
        private int m_width;
        [SerializeField]
        private int m_height;
        [SerializeField]
        private float m_cellSize;
        [SerializeField]
        private float m_cellSpacing;
        [SerializeField]
        private Color m_cellColor;

        // Fields
        private Vector2 m_gridArray;
        private Vector2 m_previousMousePosition;
        private PlayerInputActions m_inputActions;
        private bool m_isDragging;

        public void Awake()
        {
            Assert.IsTrue(m_width > 0, "Width must be greater than 0");
            Assert.IsTrue(m_height > 0, "Height must be greater than 0");
            Assert.IsTrue(m_cellSize > 0, "Cell size must be greater than 0");

            m_gridArray = new Vector2(m_width, m_height);
            m_inputActions = new PlayerInputActions();
            m_previousMousePosition = Vector2.zero;

            m_isDragging = false;
        }

        public void Start()
        {
            Create();
        }

        public void OnEnable()
        {
            m_inputActions.Enable();
            m_inputActions.Player.DragAndMove.started += OnDragStarted;
            m_inputActions.Player.DragAndMove.canceled += OnDragCanceled;
        }

        public void OnDisable()
        {
            m_inputActions.Player.DragAndMove.started -= OnDragStarted;
            m_inputActions.Player.DragAndMove.canceled -= OnDragCanceled;
            m_inputActions.Disable();
        }

        public void Update()
        {
            if (m_isDragging)
            {
                HandleMouseDrag(Mouse.current.position.value);
            }
        }

        private void OnDragStarted(InputAction.CallbackContext context)
        {
            m_isDragging = true;
            m_previousMousePosition = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());
            Debug.Log("Drag Started");
        }

        private void OnDragCanceled(InputAction.CallbackContext context)
        {
            m_isDragging = false;
            m_previousMousePosition = Vector2.zero;
            Debug.Log("Drag Canceled");
        }

        private void OnDrag(InputAction.CallbackContext context)
        {
            if (m_isDragging)
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());
                Vector2 delta = mousePosition - m_previousMousePosition;
                m_previousMousePosition = mousePosition;

                Debug.Log("Cur pos: " + mousePosition);
                Debug.Log("Delta: " + delta);

                HandleMouseDrag(delta);
            }
        }

        /// <summary>
        /// Triggered when a cell is clicked and dragged.
        /// </summary>
        private void HandleMouseDrag(Vector2 mousePos)
        {
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction);

            Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
            foreach (RaycastHit2D hit in hits)
            {
                SpriteGridCell cell = hit.collider.GetComponent<SpriteGridCell>();
                if (cell != null)
                {
                    GridCellMouseDown?.Invoke(cell.CellInfo);
                }

                Debug.Log("Hit: " + cell.name + ", pos: " + cell.transform.position);
            }
        }

        /// <summary>
        /// Generates the grid of cells that make up the SpriteGrid and broadcasts the grid info.
        /// </summary>
        private void Create()
        {
            // Ensure the alpha value is set to 1.0F.
            m_cellColor.a = 1.0F;

            int cnt = 1;
            for (int y = 0; y < m_gridArray.y; ++y)
            {
                for (int x = 0; x < m_gridArray.x; ++x)
                {
                    if (cnt > 9)
                    {
                        cnt = 1;
                    }

                    SpriteGridCell cell = new GameObject().AddComponent<SpriteGridCell>();
                    cell.Init(transform, cnt, x, y, m_cellSize, m_cellSpacing, m_cellColor);
                    ++cnt;
                }
            }

            // Calculate the center of the grid based on the width and height.
            float tempx = -m_width * m_cellSize / 2 + m_cellSize / 2;
            float tempy = -m_height * m_cellSize / 2 + m_cellSize / 2;
            transform.position = new Vector3(-m_width * (m_cellSize + m_cellSpacing) / 2 + (m_cellSize + m_cellSpacing) / 2,
                                                -m_height * (m_cellSize + m_cellSpacing) / 2 + (m_cellSize + m_cellSpacing) / 2, 0);

            GridCreated?.Invoke(new SpriteGridInfo()
            {
                Width = m_width,
                Height = m_height,
                CellSize = m_cellSize,
                CellSpacing = m_cellSpacing,
                CurrentColor = m_cellColor
            });
        }
    }

    /// <summary>
    /// Represents the information of a SpriteGrid.
    /// </summary>
    public class SpriteGridInfo
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public float CellSize { get; set; }
        public float CellSpacing { get; set; }
        public Color CurrentColor { get; set; }

        /// <summary>
        /// Validates the dimensions of the grid.
        /// </summary>
        /// <returns>Passes, if true.</returns>
        public bool Validate()
        {
            return Width > 0 && Height > 0 && CellSize > 0;
        }
    }
}
