using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour {

	Transform tp{
		get{
			return transform.Find ("textparent");
		}
	}

	TextMesh btextmesh {
		get {
			return tp.GetComponentInChildren<TextMesh> ();
		}
	}

	public bool active{
		get{
			return gameObject.activeSelf;
		}
		set{
			gameObject.SetActive (value);
			GM.canvas.SetActive (!value);
		}
	}

	public void Initialize(){
		active = true;
		btextmesh.GetComponent<Renderer> ().sortingLayerName = "Cinema";
		btextmesh.GetComponent<Renderer> ().sortingOrder = 550;
		StartCoroutine (MoveTitleUp ());
		GM.music.running = 1;
	}

	Vector3 target{
		get{
			return new Vector3 (-3.65f, -0.8f, 0f);
		}
	}

	IEnumerator MoveTitleUp(){
		while (Vector3.Distance (target, tp.localPosition) > 1f) {
			tp.localPosition = Vector3.MoveTowards (tp.localPosition, target, Time.deltaTime * 5f);
			yield return null;
		}
		yield return new WaitForSeconds (.5f);
		btextmesh.text = "The passion of \na peasant\n<i><size=150>Press any control key to start</size></i>";
	}

}
