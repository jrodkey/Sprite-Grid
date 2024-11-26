
using System.Collections.Generic;

namespace Assets.Scripts.Controllers
{
    /// <summary>
    /// Controls the flow of the game by managing the undo/redo actions. The actions are stored 
    /// in a stack, that can be executed or undone, and the capacity of the stack can be 
    /// adjusted as needed.
    /// </summary>
    public class GameFlowControl
    {
        // Represents the capacity of the game flow. This can be adjusted as needed.
        private static int Capacity = 100;

        // Holds the cache of all of the current undo/redo actions.
        private Stack<GameFlowAction> m_undoStack = new Stack<GameFlowAction>(Capacity);
        private Stack<GameFlowAction> m_redoStack = new Stack<GameFlowAction>(Capacity);

        /// <summary>
        /// Runs the specified action and adds it to the undo stack and clears the redo stack.
        /// </summary>
        /// <param name="action">Specified action.</param>
        public void ExecuteAction(GameFlowAction action)
        {
            action.Execute();
            m_undoStack.Push(action);
            m_redoStack.Clear();

            if (m_undoStack.Count > Capacity)
            {
                m_undoStack.TrimExcess();
            }
        }

        /// <summary>
        /// Undo the last action and add it to the redo stack.
        /// </summary>
        public void Undo()
        {
            if (m_undoStack.Count == 0)
            {
                return;
            }
            
            GameFlowAction action = m_undoStack.Pop();
            action.Undo();
            m_redoStack.Push(action);
        }

        /// <summary>
        /// Redo the last action and add it to the undo stack.
        /// </summary>
        public void Redo()
        {
            if (m_redoStack.Count == 0)
            {
                return;
            }

            GameFlowAction action = m_redoStack.Pop();
            action.Execute();
            m_undoStack.Push(action);
        }
    }

    /// <summary>
    /// Defines the basic structure of a game flow action.
    /// </summary>
    public abstract class GameFlowAction
    {
        public string Name { get; set; }
        public abstract void Execute();
        public abstract void Undo();
    }
}
