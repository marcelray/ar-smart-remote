using System;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class SpatialTarget : MonoBehaviour
{
	private static int currentIndex = 0;
	
	public string id;
	public int index;

	private string _label;
	public string label
	{
		get { return _label; }
		set
		{
			_label = value;
			if ( _labelText )
				_labelText.text = _label;
		}
	}

	private bool _selected;
	public bool selected
	{
		get { return _selected; }
		set
		{
			_selected = value;
			_visual.GetComponent<Renderer>().material.color = selected ? selectedColor : defaultColor;
		}
	}

	public float minScale = .001f;
	public float maxScale = 2f;

	[SerializeField]
	private GameObject _visual;
	[SerializeField]
	private TextMeshPro _labelText;

	public Color defaultColor = Color.white;
	public Color selectedColor = Color.magenta;

	private float _labelMarginY;

	private void Awake()
	{
		label = "Object";
		this.index = SpatialTarget.currentIndex;
		SpatialTarget.currentIndex++;
	}

	void Start()
	{
		_labelText = FindObjectOfType<TextMeshPro>();
		_labelMarginY = Mathf.Abs( _labelText.transform.localPosition.y ) - ( GameObjectUtils.GetBounds( _visual ).size.y / 2 );
	}

	void Update()
	{
		_labelText.text = label + "-" + index.ToString();
	}

	public void SetRelativeVisualScale(float delta)
	{
		SetVisualScale( _visual.transform.localScale.x + delta );
	}
	
	public void SetVisualScale(float scale)
	{
		scale = Mathf.Clamp(scale, minScale, maxScale);
		
		_visual.transform.localScale = new Vector3( scale , scale , scale );
		float newHeight = GameObjectUtils.GetBounds(_visual , false ).size.y;
		float labelPosition = -((newHeight / 2) + _labelMarginY);
		_labelText.transform.localPosition = new Vector3( 0 , labelPosition , 0 );
	}
}