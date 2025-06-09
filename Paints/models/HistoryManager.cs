// tombol undo/redo

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace PaintCeunah.models
{
    public class HistoryManager
    {
        private Stack<List<Shape>> undoStack;
        private Stack<List<Shape>> redoStack;
        private int maxHistorySize = 50;

        public HistoryManager()
        {
            undoStack = new Stack<List<Shape>>();
            redoStack = new Stack<List<Shape>>();
        }

        public void SaveState(List<Shape> shapes)
        {
            // Deep copy shapes list
            var shapesCopy = shapes.Select(s => CloneShape(s)).ToList();
            undoStack.Push(shapesCopy);
            
            // Clear redo stack when new action is performed
            redoStack.Clear();
            
            // Limit history size
            if (undoStack.Count > maxHistorySize)
            {
                var tempStack = new Stack<List<Shape>>();
                for (int i = 0; i < maxHistorySize - 1; i++)
                {
                    tempStack.Push(undoStack.Pop());
                }
                undoStack.Clear();
                while (tempStack.Count > 0)
                {
                    undoStack.Push(tempStack.Pop());
                }
            }
        }

        public List<Shape> Undo()
        {
            if (undoStack.Count > 1)
            {
                redoStack.Push(undoStack.Pop());
                return undoStack.Peek().Select(s => CloneShape(s)).ToList();
            }
            return null;
        }

        public List<Shape> Redo()
        {
            if (redoStack.Count > 0)
            {
                var state = redoStack.Pop();
                undoStack.Push(state);
                return state.Select(s => CloneShape(s)).ToList();
            }
            return null;
        }

        public bool CanUndo => undoStack.Count > 1;
        public bool CanRedo => redoStack.Count > 0;

        private Shape CloneShape(Shape original)
        {
            // Simple cloning - you might want to implement ICloneable
            switch (original.ShapeType)
            {
                case EnumShape.CIRCLE:
                    return new Circle(original.ShapeType, original.StartPoint, original.EndPoint,
                        original.FillColor, original.BorderColor, new Pen(original.BorderColor, original.BorderWidth.Width),
                        original.RotationAngle);
                case EnumShape.SQUARE:
                    return new Square(original.ShapeType, original.StartPoint, original.EndPoint,
                        original.FillColor, original.BorderColor, new Pen(original.BorderColor, original.BorderWidth.Width),
                        original.RotationAngle);
                case EnumShape.RECTANGLE:
                    return new RectangleDrawer(original.ShapeType, original.StartPoint, original.EndPoint,
                        original.FillColor, original.BorderColor, new Pen(original.BorderColor, original.BorderWidth.Width),
                        original.RotationAngle);
                case EnumShape.STAR:
                    return new Star(original.ShapeType, original.StartPoint, original.EndPoint,
                        original.FillColor, original.BorderColor, new Pen(original.BorderColor, original.BorderWidth.Width),
                        original.RotationAngle);
                // Add other shapes...
                default:
                    return original; // Fallback
            }
        }
    }
}