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
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using FronkonGames.GameWork.Foundation;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace FronkonGames.GameWork.Modules.AI
{
  /// <summary>
  /// .
  /// </summary>
  public class StateMachineGraph : EditorWindow
  {
    private StateMachineGraphView graphView;

    [MenuItem("Game:Work/Modules/AI/State Machine/StateMachine Graph")]
    public static void OpenStateMachineGraphWindow()
    {
      StateMachineGraph window = GetWindow<StateMachineGraph>();
      window.titleContent = new GUIContent("StateMachine Graph");
    }

    private void OnEnable()
    {
      CreateGraphView();
      
      CreateToolbar();
    }

    private void OnDisable()
    {
      rootVisualElement.Remove(graphView);
    }

    private void CreateToolbar()
    {
      Toolbar toolbar = new Toolbar();

      Button nodeCreateButton = new Button(() => { graphView.CreateNode("StateMachine Node"); });
      nodeCreateButton.text = "Create Node";
      
      toolbar.Add(nodeCreateButton);
      
      rootVisualElement.Add(toolbar);
    }

    private void CreateGraphView()
    {
      graphView = new StateMachineGraphView
      {
        name = "StateMachine Graph"
      };
      graphView.StretchToParentSize();
      rootVisualElement.Add(graphView);
    }
  }
}
