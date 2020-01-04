using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ObjectHotspot : MonoBehaviour
{
	public enum ObjectType {
		[Description("light")]
		Light ,
		[Description("speaker")]
		Speaker ,
		[Description("ledgrid")]
		LEDGrid ,
		[Description("unknown")]
		Unknown
	}

	public string TypeName {
		get {
			return Type.GetDescription();
		}
	}

	public float minScale = .001f;
	public float maxScale = 2f;

	[SerializeField]
	private GameObject _visual;
	[SerializeField]
	private TextMeshPro _label;

	private float _labelMarginY;

	private bool _updateLabelFlag = true;
	private ObjectType _type = ObjectType.Unknown;
	public ObjectType Type {
		get {
			return _type;
		}
		set {
			_type = value;
			_updateLabelFlag = true;
		}
	}

	void Start()
	{
		_label = FindObjectOfType<TextMeshPro>();
		_labelMarginY = Mathf.Abs( _label.transform.localPosition.y ) - ( GameObjectUtils.GetBounds( _visual ).size.y / 2 );
		Debug.Log( "MARGIN: " + _labelMarginY );
	}

	void Update()
	{
		if ( _updateLabelFlag && _label )
		{
			_label.text = TypeName;
			_updateLabelFlag = false;
		}
	}

	public void SetVisualScale(float scale)
	{
		scale = Mathf.Clamp(scale, minScale, maxScale);
		
		_visual.transform.localScale = new Vector3( scale , scale , scale );
		float newHeight = GameObjectUtils.GetBounds(_visual , false ).size.y;
		float labelPosition = -((newHeight / 2) + _labelMarginY);
		Debug.Log( "newHeight: " + newHeight + " / labelPosition: " + labelPosition );
		_label.transform.localPosition = new Vector3( 0 , labelPosition , 0 );
	}

	public void SetVisualSize(float size)
	{
		size = Mathf.Clamp(size, minScale, maxScale);
		GameObjectUtils.SetSize( _visual , size );
		_label.transform.localPosition = new Vector3( 0 , -((size/2) + _labelMarginY) , 0 );
	}

}