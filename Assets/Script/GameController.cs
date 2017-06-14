using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GameController : MonoBehaviour {
	// A reference to the player character.
	// We can use this to tell the player whether they won or lost by calling their public functions.
	public Character myPlayer;

	// A reference to the enemy character.
	// We can use this to tell the player whether they won or lost by calling their public functions.
	public Character myEnemy;

	// This is the game object we will use to show whether it is time to strike or not.
	public GameObject myStrike;

	// We will use this boolean (true/false) to keep track of whether the player is allowed to attack or not.
	private bool myPlayerCanStrike = false;

	// We will keep track of whether or not the round is over.  If it is then player input shouldn't be allowed.
	private bool myRoundComplete = false;

	// This will be the minimum time in seconds before we enter the strike state.
	public float myMinToStrikeTime = 3f;
	// This will be the maximum time in seconds before we enter the strike state.
	public float myMaxToStrikeTime = 4f;

	// This will be the minimum time in seconds before the enemy will perform a strike after we enter the strike state.
	public float myMinEnemyStrikeTime = 1f;
	// This will be the maximum time in seconds before the enemy will perform a strike after we enter the strike state.
	public float myMaxEnemyStrikeTime = 2f;

	// This is the wait time where we display to the player whether they won or lost.
	public float myWaitBetweenRoundsTime = 1.5f;

	private AudioSource myStrikeSound;

	// Use this for initialization
	void Start () {
		// We use this function call to start a new round!  This is the first round of the game.
		NewRound ();

		// We get the Audio Source component from the game object and keep a reference of it so we can play it when we want.
		myStrikeSound = GetComponent<AudioSource> ();
	}

	// Update is called once per frame
	void Update() {
		// If the round isn't over yet, then...
		if (myRoundComplete == false) {
			// If we press down the spacebar, then...
			if (Input.GetKeyDown (KeyCode.Space)) {
				// We check if the player is allowed to strike.
				if (myPlayerCanStrike == true) {
					// If the player was allowed to strike they win.
					PlayerWin();
				} else {
					// If the player was not allowed to strike they lose :(
					// No itchy trigger fingers.  You pressed space too early.
					// We also want to cancel the currently running invokes this is so that the StrikeState function will not fire.
					CancelInvoke();
					PlayerLose();
				}
			}
		}
	}

	void NewRound(){
		// This will disable the "Strike" game object.
		// While a game object is disabled it will not render or run any of its functionality.
		myStrike.SetActive (false);

		// The round is no longer complete as we are starting a new one.
		myRoundComplete = false;

		// We make sure that the player isn't allowed to attack just yet.
		myPlayerCanStrike = false;

		// We reset both the player sprites.
		myPlayer.Reset ();
		myEnemy.Reset ();

		// Here we get a random range between our minimum strike time and maximum.
		// We assign this to a variable that will be used to determine when it is time for the player to be able to attack.
		float timeToStrike = Random.Range (myMinToStrikeTime, myMaxToStrikeTime);

		// Invoke is a special function.  It will run the function we give it a string name of in a number of seconds.
		// Paramater one is a string with the name of the function.
		// Paramater two is the time we wait until we run the function.
		Invoke ("StrikeState", timeToStrike); 
	}

	// Whenever the game enters the state where a player is allowed to strike we should run this function.
	void StrikeState() {
		// We set the "strike" game object to active so that we can see it in the scene.
		myStrike.SetActive (true);

		// We set it so that the player can press space and not lose.
		myPlayerCanStrike = true;

		// We get a random time for when the enemy will attack.
		float enemyTimeToStrike = Random.Range (myMinEnemyStrikeTime, myMaxEnemyStrikeTime);

		// We will make the enemy perform their attack by calling a function using invoke.
		Invoke("EnemyStrike", enemyTimeToStrike);
	}

	// Whenever the enemy would have attempted to strike this function is executed.
	void EnemyStrike(){
		// If the round is already over we best not strike.  We've already lost.
		if (myRoundComplete == true) {
			// Return will exit the function even though we return nothing as the return type is void.
			return;
		}

		// If the round wasn't complete it is now!  And the enemy won.
		PlayerLose();
	}

	// Execute this function when the player wins.
	void PlayerWin(){
		// Set the player and enemy sprites accordingly.
		myPlayer.Win ();
		myEnemy.Lose ();

		// We end the current round.
		EndRound ();
	}
		
	// Execute this function when the player loses.
	void PlayerLose(){
		// Set the player and enemy sprites accordingly.
		myPlayer.Lose ();
		myEnemy.Win ();

		// We end the current round.
		EndRound ();
	}

	// When a round ends we play the strike sound effect and prepare to start the next round.
	void EndRound(){
		// Play the strike sound effect.
		myStrikeSound.Play ();

		// We set the round to complete and prepare to show a new round in a moment.
		myRoundComplete = true;
		Invoke ("NewRound", myWaitBetweenRoundsTime);
	}
}
