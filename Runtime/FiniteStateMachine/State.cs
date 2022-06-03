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

namespace FronkonGames.GameWork.Modules.AIModule
{
  /// <summary>
  /// State.
  /// </summary>
  public abstract class State<TState, TStateID, TTransition> where TState : class
                                                             where TStateID : struct
                                                             where TTransition : struct
  {
    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    public TStateID StateID { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public abstract void OnEnter();

    /// <summary>
    /// 
    /// </summary>
    public abstract void Update();

    /// <summary>
    /// 
    /// </summary>
    public abstract void OnExit();

    protected Dictionary<TTransition, TStateID> transitions = new Dictionary<TTransition, TStateID>();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="stateID"></param>
    public State(TStateID stateID)
    {
      StateID = stateID;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="transition"></param>
    /// <returns></returns>
    public TStateID GetStateID(TTransition transition) => transitions.ContainsKey(transition) == true ? transitions[transition] : StateID;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="transition"></param>
    /// <returns></returns>
    public TStateID GetTransitionStateID(TTransition transition) => transitions.ContainsKey(transition) == true ? transitions[transition] : StateID;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="transition"></param>
    /// <returns></returns>
    public bool ExistsTransition(TTransition transition) => transitions.ContainsKey(transition);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="transition"></param>
    /// <param name="stateID"></param>
    public void AddTransition(TTransition transition, TStateID stateID)
    {
      if (transitions.ContainsKey(transition) == false)
        transitions.Add(transition, stateID);
      else
        Log.Error($"The transition '{transition}' already exists");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="transition"></param>
    public void RemoveTransition(TTransition transition)
    {
      if (transitions.ContainsKey(transition) == true)
        transitions.Remove(transition);
      else
        Log.Error($"The transition '{transition}' does not exist");
    }
  }
}
