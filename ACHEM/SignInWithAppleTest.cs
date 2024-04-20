using UnityEngine;
using UnityEngine.SignInWithApple;

public class SignInWithAppleTest : MonoBehaviour
{
	public void Start()
	{
		base.gameObject.AddComponent<SignInWithApple>();
		Open();
	}

	public void Open()
	{
		base.gameObject.SetActive(value: true);
	}

	public void Close()
	{
		base.gameObject.SetActive(value: false);
	}

	public void ButtonPress()
	{
		base.gameObject.GetComponent<SignInWithApple>().Login(OnLogin);
	}

	public void CredentialButton()
	{
		base.gameObject.GetComponent<SignInWithApple>().GetCredentialState("<userid>", OnCredentialState);
	}

	private void OnCredentialState(SignInWithApple.CallbackArgs args)
	{
		Debug.Log("User credential state is: " + args.credentialState);
	}

	private void OnLogin(SignInWithApple.CallbackArgs args)
	{
		Debug.Log("Sign in with Apple login has completed.");
	}
}
