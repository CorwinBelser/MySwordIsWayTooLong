using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class SwordCollisions : MonoBehaviour
{
	public GameObject SwordTip;
	public GameObject mainCam;
	public LayerMask SwordBlockLayers;
	public AudioClip[] ClinkNoises;

	public float XSensitivity;
	public float YSensitivity;

	private bool looksleeping = false;
	private bool movesleeping = false;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (true) {
			float yRot = CrossPlatformInputManager.GetAxis ("Mouse X") * XSensitivity;
			float xRot = CrossPlatformInputManager.GetAxis ("Mouse Y") * YSensitivity;

			Quaternion m_CharacterTargetRot = Quaternion.Euler (0f, yRot, 0f);
			Quaternion m_CameraTargetRot = Quaternion.Euler (-xRot, 0f, 0f);

			transform.localRotation *= m_CharacterTargetRot;
			Vector3 nextposition = SwordTip.transform.position;

			transform.localRotation *= Quaternion.Inverse (m_CharacterTargetRot);
			Debug.DrawRay (SwordTip.transform.position, (nextposition - SwordTip.transform.position) * 1, Color.cyan, 2f);
			float dist = Vector3.Distance (nextposition, SwordTip.transform.position);
			RaycastHit swordHit;
			if (Physics.Raycast (SwordTip.transform.position, (nextposition - SwordTip.transform.position) * 1, out swordHit, dist, SwordBlockLayers)) {
				m_CameraTargetRot = Quaternion.Euler (0f, -yRot / 2, 0f);
				m_CharacterTargetRot = Quaternion.Euler (0f, 0f, 0f);
				AudioSource m_AudioSource = GetComponent<AudioSource> ();
				m_AudioSource.clip = ClinkNoises[Random.Range(0,ClinkNoises.Length)];
				m_AudioSource.PlayOneShot (m_AudioSource.clip);
				Debug.Log ("bingo");
				StartCoroutine (MouseLookSleep ());
			}
			transform.localRotation *= m_CharacterTargetRot;
			mainCam.transform.localRotation *= m_CameraTargetRot;
		}
	}

	private IEnumerator MouseLookSleep() {
		looksleeping = true;
		yield return new WaitForSeconds (1f);
		Vector3 currRot = mainCam.transform.localRotation.eulerAngles;
		currRot.z = 0f;
		mainCam.transform.localRotation = Quaternion.Euler (currRot);
		looksleeping = false;
	}

	private IEnumerator MoveSleep() {
		movesleeping = true;
		yield return new WaitForSeconds (.25f);
		movesleeping = false;
	}

	public float willCollideWithMovement(Vector3 Movement) {
		if (!SwordTip.activeInHierarchy) {
			return -2f;
		}
		if (movesleeping) {
			return -1f;
		}
		Movement.y = 0f;
		Vector3 nextposition = SwordTip.transform.position + Movement;
		Debug.DrawRay (SwordTip.transform.position, (nextposition - SwordTip.transform.position) * 1, Color.cyan, Vector3.Distance(nextposition, SwordTip.transform.position));
		float dist = Vector3.Distance (nextposition, SwordTip.transform.position);
		RaycastHit swordHit;
		if (Physics.Raycast (SwordTip.transform.position, (nextposition - SwordTip.transform.position) * 1, out swordHit, dist, SwordBlockLayers)) {
			AudioSource m_AudioSource = GetComponent<AudioSource> ();
			m_AudioSource.clip = ClinkNoises[Random.Range(0,ClinkNoises.Length)];
			m_AudioSource.PlayOneShot (m_AudioSource.clip);
			StartCoroutine (MoveSleep ());
			return swordHit.distance;
		}
		return -2f;
	}
}