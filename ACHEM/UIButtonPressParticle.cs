using System;
using UnityEngine;

public class UIButtonPressParticle : MonoBehaviour
{
	private float GlowScaleX;

	private float GlowScaleY;

	private ParticleSystem ParticleSystem;

	private UISprite sprite;

	private UIButton button;

	private void Start()
	{
		ParticleSystem = GetComponent<ParticleSystem>();
		sprite = base.transform.parent.GetComponent<UISprite>();
		button = base.transform.parent.GetComponent<UIButton>();
		GlowScaleX = (float)sprite.width / 322.58f;
		GlowScaleY = (float)sprite.height / 357.14f;
		ParticleSystem.ShapeModule shape = ParticleSystem.shape;
		Vector3 scale = new Vector3(GlowScaleX, GlowScaleY, 0f);
		ParticleSystemShapeType shapeType = ParticleSystemShapeType.Box;
		shape.shapeType = shapeType;
		shape.scale = scale;
		UIEventListener uIEventListener = UIEventListener.Get(button.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(ClickParticle));
	}

	private void ClickParticle(GameObject go)
	{
		ParticleSystem.Stop();
		ParticleSystem.Play();
	}

	private void Update()
	{
	}
}
