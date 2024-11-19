using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Runtime.VisualStyle.UI.Scripts
{
  [ExecuteAlways]
  [RequireComponent(typeof(CanvasRenderer))]
  public class CustomImage : MaskableGraphic
  {
    [SerializeField] private int cornerSpaces;

    [SerializeField] private bool showOnlyFrame;

    [SerializeField] private int frameThickness = 1;

    [SerializeField] private bool upperLeftCorner;
    [SerializeField] private bool upperRightCorner;
    [SerializeField] private bool lowerLeftCorner;
    [SerializeField] private bool lowerRightCorner;
    
    public bool ShowOnlyFrame
    {
      get => showOnlyFrame;
      set => showOnlyFrame = value;
    }

    private List<Vector2> _innerCorners;

    private int _numberOfVertices = 4;
    private List<Vector2> _outerCorners;

    private UIVertex _vertex;

    protected override void OnPopulateMesh(VertexHelper vertexHelper)
    {
      _outerCorners = new List<Vector2>();
      _innerCorners = new List<Vector2>();

      Vector2 pivot = rectTransform.pivot;
      Rect rect = rectTransform.rect;
      float width = rect.width;
      float height = rect.height;

      vertexHelper.Clear();

      // Upper Left Corner
      _outerCorners.Add(new Vector2(-pivot.x * width, pivot.y * height));
      // Upper Right Corner
      _outerCorners.Add(new Vector2(pivot.x * width, pivot.y * height));
      // Lower Right Corner
      _outerCorners.Add(new Vector2(pivot.x * width, -pivot.y * height));
      // Lower Left Corner
      _outerCorners.Add(new Vector2(-pivot.x * width, -pivot.y * height));


      _vertex = UIVertex.simpleVert;

      // If the color will change for each vertex, this is done before addVertex.
      _vertex.color = color;

      bool hasCorner = cornerSpaces == 0;

      if (showOnlyFrame)
      {
        DrawFrame(vertexHelper);
      }
      else
      {
        if (hasCorner)
          DrawRectangle(vertexHelper);
        else
          DrawRectangleWithoutCorners(vertexHelper);
      }
    }

    private void DrawFrame(VertexHelper vertexHelper)
    {
      _numberOfVertices = 8;

      foreach (Vector2 outerCorner in _outerCorners)
      {
        float outerCornerX = outerCorner.x > 0
          ? outerCorner.x - cornerSpaces
          : outerCorner.x + cornerSpaces;
        float outerCornerY = outerCorner.y > 0
          ? outerCorner.y - cornerSpaces
          : outerCorner.y + cornerSpaces;

        float margin = frameThickness / Mathf.Sqrt(2f) + cornerSpaces;


        if (outerCorner.x > 0)
        {
          if (outerCorner.y > 0)
          {
            if (upperRightCorner)
            {
              FillCorner(vertexHelper, outerCorner);
            }
            else
            {
              _vertex.position = new Vector2(outerCornerX, outerCorner.y);
              vertexHelper.AddVert(_vertex);

              _vertex.position = new Vector2(outerCorner.x, outerCornerY);
              vertexHelper.AddVert(_vertex);

              _innerCorners.Add(new Vector2(outerCorner.x - margin, outerCorner.y - frameThickness));
              _innerCorners.Add(new Vector2(outerCorner.x - frameThickness, outerCorner.y - margin));
            }
          }
          else
          {
            if (lowerRightCorner)
            {
              FillCorner(vertexHelper, outerCorner);
            }
            else
            {
              _vertex.position = new Vector2(outerCorner.x, outerCornerY);
              vertexHelper.AddVert(_vertex);

              _vertex.position = new Vector2(outerCornerX, outerCorner.y);
              vertexHelper.AddVert(_vertex);

              _innerCorners.Add(new Vector2(outerCorner.x - frameThickness, outerCorner.y + margin));
              _innerCorners.Add(new Vector2(outerCorner.x - margin, outerCorner.y + frameThickness));
            }
          }
        }
        else
        {
          if (outerCorner.y > 0)
          {
            if (upperLeftCorner)
            {
              FillCorner(vertexHelper, outerCorner);
            }
            else
            {
              _vertex.position = new Vector2(outerCorner.x, outerCornerY);
              vertexHelper.AddVert(_vertex);

              _vertex.position = new Vector2(outerCornerX, outerCorner.y);
              vertexHelper.AddVert(_vertex);

              _innerCorners.Add(new Vector2(outerCorner.x + frameThickness, outerCorner.y - margin));
              _innerCorners.Add(new Vector2(outerCorner.x + margin, outerCorner.y - frameThickness));
            }
          }
          else
          {
            if (lowerLeftCorner)
            {
              FillCorner(vertexHelper, outerCorner);
            }
            else
            {
              _vertex.position = new Vector2(outerCornerX, outerCorner.y);
              vertexHelper.AddVert(_vertex);

              _vertex.position = new Vector2(outerCorner.x, outerCornerY);
              vertexHelper.AddVert(_vertex);

              _innerCorners.Add(new Vector2(outerCorner.x + margin, outerCorner.y + frameThickness));
              _innerCorners.Add(new Vector2(outerCorner.x + frameThickness, outerCorner.y + margin));
            }
          }
        }
      }

      foreach (Vector2 innerCorner in _innerCorners)
      {
        _vertex.position = innerCorner;
        vertexHelper.AddVert(_vertex);
      }

      for (int i = 0; i < _numberOfVertices; i++)
      {
        vertexHelper.AddTriangle(i, (i + 1) % _numberOfVertices,
          (i + 1) % _numberOfVertices + _numberOfVertices);
        vertexHelper.AddTriangle(i, i + _numberOfVertices, (i + 1) % _numberOfVertices + _numberOfVertices);
      }
    }

    private void FillCorner(VertexHelper vertexHelper, Vector2 outerCorner)
    {
      float innerCornerX = outerCorner.x > 0
        ? outerCorner.x - frameThickness
        : outerCorner.x + frameThickness;
      float innerCornerY = outerCorner.y > 0
        ? outerCorner.y - frameThickness
        : outerCorner.y + frameThickness;

      _vertex.position = new Vector2(outerCorner.x, outerCorner.y);
      vertexHelper.AddVert(_vertex);

      _vertex.position = new Vector2(outerCorner.x, outerCorner.y);
      vertexHelper.AddVert(_vertex);

      _innerCorners.Add(new Vector2(innerCornerX, innerCornerY));
      _innerCorners.Add(new Vector2(innerCornerX, innerCornerY));
    }

    private void DrawRectangleWithoutCorners(VertexHelper vertexHelper)
    {
      _numberOfVertices = 8;

      foreach (Vector2 outerCorner in _outerCorners)
      {
        float outerCornerX = outerCorner.x > 0
          ? outerCorner.x - cornerSpaces
          : outerCorner.x + cornerSpaces;
        float outerCornerY = outerCorner.y > 0
          ? outerCorner.y - cornerSpaces
          : outerCorner.y + cornerSpaces;


        if (outerCorner.x > 0)
        {
          if (outerCorner.y > 0)
          {
            if (upperRightCorner)
            {
              FillCorner(vertexHelper, outerCorner);
            }
            else
            {
              _vertex.position = new Vector2(outerCornerX, outerCorner.y);
              vertexHelper.AddVert(_vertex);

              _vertex.position = new Vector2(outerCorner.x, outerCornerY);
              vertexHelper.AddVert(_vertex);
            }
          }
          else
          {
            if (lowerRightCorner)
            {
              FillCorner(vertexHelper, outerCorner);
            }
            else
            {
              _vertex.position = new Vector2(outerCorner.x, outerCornerY);
              vertexHelper.AddVert(_vertex);

              _vertex.position = new Vector2(outerCornerX, outerCorner.y);
              vertexHelper.AddVert(_vertex);
            }
          }
        }
        else
        {
          if (outerCorner.y > 0)
          {
            if (upperLeftCorner)
            {
              FillCorner(vertexHelper, outerCorner);
            }
            else
            {
              _vertex.position = new Vector2(outerCorner.x, outerCornerY);
              vertexHelper.AddVert(_vertex);

              _vertex.position = new Vector2(outerCornerX, outerCorner.y);
              vertexHelper.AddVert(_vertex);
            }
          }
          else
          {
            if (lowerLeftCorner)
            {
              FillCorner(vertexHelper, outerCorner);
            }
            else
            {
              _vertex.position = new Vector2(outerCornerX, outerCorner.y);
              vertexHelper.AddVert(_vertex);

              _vertex.position = new Vector2(outerCorner.x, outerCornerY);
              vertexHelper.AddVert(_vertex);
            }
          }
        }
      }

      for (int i = 1; i < _numberOfVertices - 1; i++) vertexHelper.AddTriangle(0, i, i + 1);
    }

    private void DrawRectangle(VertexHelper vertexHelper)
    {
      _numberOfVertices = 4;

      foreach (Vector2 outerCorner in _outerCorners)
      {
        _vertex.position = outerCorner;
        vertexHelper.AddVert(_vertex);
      }

      for (int i = 1; i < _numberOfVertices - 1; i++) vertexHelper.AddTriangle(0, i, i + 1);
    }
  }
}