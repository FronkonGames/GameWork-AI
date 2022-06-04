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
using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using FronkonGames.GameWork.Modules.AI;

/// <summary>
/// AI module test.
/// </summary>
public partial class AIModuleTests
{
  public enum EnemyState
  {
    Patrol,
    Aim,
    Shoot,
  }

  public enum EnemyTransition
  {
    NoPlayerNearby,
    PlayerNear,
  }

  public class Enemy {}

  public class StatePatrol : State<Enemy, EnemyState, EnemyTransition>
  {
    public StatePatrol() : base(EnemyState.Patrol) {}

    public override void OnEnter() { }

    public override void Update() { }

    public override void OnExit() { }
  }

  public class StateAim : State<Enemy, EnemyState, EnemyTransition>
  {
    public StateAim() : base(EnemyState.Aim) { }

    public override void OnEnter() {}

    public override void Update() {}

    public override void OnExit() {}
  }

  public class StateShoot : State<Enemy, EnemyState, EnemyTransition>
  {
    public StateShoot() : base(EnemyState.Shoot) { }

    public override void OnEnter() { }

    public override void Update() { }

    public override void OnExit() { }
  }

  /// <summary>
  /// State Machine test.
  /// </summary>
  [UnityTest]
  public IEnumerator StateMachine()
  {
    StateMachine<Enemy, EnemyState, EnemyTransition> enemyStateMachine = new StateMachine<Enemy, EnemyState, EnemyTransition>();

    // @HACK: Simulating the functioning of the modules without registering them.
    enemyStateMachine.OnInitialize();
    enemyStateMachine.OnInitialized();

    Assert.IsNull(enemyStateMachine.CurrentState);

    StatePatrol statePatrol = new StatePatrol();
    statePatrol.AddTransition(EnemyTransition.PlayerNear, EnemyState.Aim);

    StateAim stateAim = new StateAim();
    stateAim.AddTransition(EnemyTransition.PlayerNear, EnemyState.Shoot);
    stateAim.AddTransition(EnemyTransition.NoPlayerNearby, EnemyState.Patrol);

    StateShoot stateShoot = new StateShoot();
    stateShoot.AddTransition(EnemyTransition.NoPlayerNearby, EnemyState.Patrol);

    enemyStateMachine.AddState(statePatrol);
    enemyStateMachine.AddState(stateAim);
    enemyStateMachine.AddState(stateShoot);

    // The enemy begins patrol.
    enemyStateMachine.ChangeState(EnemyState.Patrol);

    Assert.NotNull(enemyStateMachine.CurrentState);
    Assert.AreEqual(enemyStateMachine.CurrentState.StateID, EnemyState.Patrol);

    // There are no players near, keep patrol.
    enemyStateMachine.DoTransition(EnemyTransition.NoPlayerNearby);
    Assert.AreEqual(enemyStateMachine.CurrentState.StateID, EnemyState.Patrol);

    // Find a player, he points to him.
    enemyStateMachine.DoTransition(EnemyTransition.PlayerNear);
    Assert.AreEqual(enemyStateMachine.CurrentState.StateID, EnemyState.Aim);

    // Keep close, shoots him.
    enemyStateMachine.DoTransition(EnemyTransition.PlayerNear);
    Assert.AreEqual(enemyStateMachine.CurrentState.StateID, EnemyState.Shoot);

    // He keeps shooting.
    enemyStateMachine.DoTransition(EnemyTransition.PlayerNear);
    Assert.AreEqual(enemyStateMachine.CurrentState.StateID, EnemyState.Shoot);

    // He loses it, patrol again.
    enemyStateMachine.DoTransition(EnemyTransition.NoPlayerNearby);
    Assert.AreEqual(enemyStateMachine.CurrentState.StateID, EnemyState.Patrol);

    enemyStateMachine.OnDeinitialize();

    yield return null;
  }
}