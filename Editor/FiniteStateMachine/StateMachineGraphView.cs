////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Martin Bustos @FronkonGames <fronkongames@gmail.com>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of
// the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using FronkonGames.GameWork.Foundation;
using UnityEngine.UIElements;

namespace FronkonGames.GameWork.Modules.AI
{
  /// <summary>
  /// .
  /// </summary>
  public class StateMachineGraphView : GraphView
  {
    private readonly Vector2 DefaultNodeSize = new Vector2(150, 200);

    public StateMachineGraphView()
    {
      styleSheets.Add(Resources.Load<StyleSheet>("StateMachineGraph"));
      SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

      this.AddManipulator(new ContentDragger());
      this.AddManipulator(new SelectionDragger());
      this.AddManipulator(new RectangleSelector());

      GridBackground grid = new GridBackground();
      Insert(0, grid);
      grid.StretchToParentSize();

      AddElement(CreateEntryPointNode());
    }
    
    public override List<Port> GetCompatiblePorts(Port start, NodeAdapter adapter)
    {
      List<Port> compatible = new List<Port>();

      ports.ForEach(port =>
      {
        if (start != port && start.node != port.node)
          compatible.Add(port);
      });

      return compatible;
    }
    
    public void CreateNode(string name)
    {
      AddElement(CreateStateMachineNode(name));
    }

    public StateMachineNode CreateStateMachineNode(string name)
    {
      StateMachineNode node = new StateMachineNode()
      {
        title = name,
        GUID = Guid.NewGuid().ToString()
      };

      Port input = CreatePort(node, Direction.Input, Port.Capacity.Multi);
      input.name = "INPUT";
      node.inputContainer.Add(input);

      Button button = new Button(() => { AddChoicePort(node); });
      button.text = "New Choice";
      node.titleContainer.Add(button);
      
      node.RefreshExpandedState();
      node.RefreshPorts();
      node.SetPosition(new Rect(Vector2.zero, DefaultNodeSize));

      return node;
    }

    private StateMachineNode CreateEntryPointNode()
    {
      StateMachineNode node = new StateMachineNode()
      {
        title = "START",
        GUID = Guid.NewGuid().ToString(),
        EntryPoint = true
      };

      Port port = CreatePort(node, Direction.Output);
      port.portName = "NEXT";
      
      node.outputContainer.Add(port);
      
      node.RefreshExpandedState();
      node.RefreshPorts();
      
      node.SetPosition(new Rect(100, 200, 100, 150));

      return node;
    }

    private Port CreatePort(StateMachineNode node, Direction direction, Port.Capacity capacity = Port.Capacity.Single)
      => node.InstantiatePort(Orientation.Horizontal, direction, capacity, typeof(float));

    private void AddChoicePort(StateMachineNode node)
    {
      Port port = CreatePort(node, Direction.Output);
      
      int portCount = node.outputContainer.Query("connector").ToList().Count;
      port.portName = $"Port {portCount.ToString()}";
      
      node.outputContainer.Add(port);
      node.RefreshPorts();
      node.RefreshExpandedState();
    }
  }
}
