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
using FronkonGames.GameWork.Foundation;
using FronkonGames.GameWork.Core;

namespace FronkonGames.GameWork.Modules.AIModule
{
  /// <summary>
  /// Finite State Machine.
  /// </summary>
  public class StateMachine<TState, TStateID, TTransition> : IInitializable,
                                                             IUpdatable
                                                             where TState : class
                                                             where TStateID : struct
                                                             where TTransition : struct
  {
    /// <summary>
    /// Is it initialized?
    /// </summary>
    /// <value>Value</value>
    public bool Initialized { get; set; }

    /// <summary>
    /// Should be updated?
    /// </summary>
    /// <value>True/false.</value>
    public bool ShouldUpdate { get; set; } = true;

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    public State<TState, TStateID, TTransition> CurrentState { get; protected set; }

    private Dictionary<TStateID, State<TState, TStateID, TTransition>> states = new Dictionary<TStateID, State<TState, TStateID, TTransition>>();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="stateID"></param>
    /// <returns></returns>
    public bool ExistsState(TStateID stateID) => states.ContainsKey(stateID);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="state"></param>
    public void AddState(State<TState, TStateID, TTransition> state)
    {
      if (states.ContainsKey(state.StateID) == false)
        states.Add(state.StateID, state);
      else
        Log.Error($"The state '{state.StateID.GetType().Name}' already exists");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="state"></param>
    public void AddStates(State<TState, TStateID, TTransition>[] states)
    {
      for (int i = 0; i < states.Length; ++i)
        AddState(states[i]);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="stateID"></param>
    public void RemoveState(TStateID stateID)
    {
      if (states.ContainsKey(stateID) == true)
      {
        if (CurrentState == states[stateID])
          CurrentState = null;

        states.Remove(stateID);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="transition"></param>
    public void DoTransition(TTransition transition)
    {
      if (CurrentState != null)
      {
        TStateID newStateID = CurrentState.GetTransitionStateID(transition);
        if (CurrentState.StateID.Equals(newStateID) == false)
          ChangeState(newStateID);
        else
          Log.Error($"Invalid transition '{transition.GetType().Name}' or returns the original state");
      }
      else
        Log.Error("No active state");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="stateID"></param>
    public void ChangeState(TStateID stateID)
    {
      if (states.ContainsKey(stateID) == true)
      {
        State<TState, TStateID, TTransition> nextState = states[stateID];
        
        CurrentState?.OnExit();
        CurrentState = nextState;
        CurrentState.OnEnter();
      }
      else
        Log.Error($"Unknown state '{stateID.GetType().Name}'");
    }

    /// <summary>
    /// When initialize.
    /// </summary>
    public void OnInitialize()
    {
    }

    /// <summary>
    /// At the end of initialization.
    /// Called in the first Update frame.
    /// </summary>
    public void OnInitialized()
    {
    }

    /// <summary>
    /// When deinitialize.
    /// </summary>
    public void OnDeinitialize()
    {
      CurrentState?.OnExit();
      CurrentState = null;

      states.Clear();
    }

    /// <summary>
    /// Update event.
    /// </summary>
    public void OnUpdate() => CurrentState?.Update();

    /// <summary>
    /// FixedUpdate event.
    /// </summary>
    public void OnFixedUpdate()
    {
    }

    /// <summary>
    /// LateUpdate event.
    /// </summary>
    public void OnLateUpdate()
    {
    }
  }
}
