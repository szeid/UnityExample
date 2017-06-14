using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Character : MonoBehaviour {
	// These sprite references will be used to store the different characters default, win and lose sprites.
	public Sprite myStandSprite;
	public Sprite myLoseSprite;
	public Sprite myWinSprite;

	// A reference to the Sprite Renderer of this game object will be stored in this variable.
	// A sprite renderer is used to display a sprite.  We will replace the sprite according to whether we won or lost.
	private SpriteRenderer mySpriteRenderer;

	// Use this for initialization
	void Start () {
		// We get the Sprite Renderer component that is assigned to this Character game object.
		mySpriteRenderer = GetComponent<SpriteRenderer> ();
	}

	// If we win we should assign the win sprite to this character.
	public void Win(){
		mySpriteRenderer.sprite = myWinSprite;
	}

	// If we lose we should assign the lose sprite to this character.
	public void Lose(){
		mySpriteRenderer.sprite = myLoseSprite;
	}

	// At the end of a round we should reset the characters sprite.
	public void Reset(){
		mySpriteRenderer.sprite = myStandSprite;
	}
}
