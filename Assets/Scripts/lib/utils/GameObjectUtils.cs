using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class GameObjectUtils
{
	public static void SetSize(GameObject gameObject, Vector3 newSize)
	{
		Vector3 currentSize = gameObject.GetComponent<Renderer>().bounds.size;

		Vector3 currentScale = gameObject.transform.localScale;
		Vector3 newScale = new Vector3();
		newScale.x = newSize.x * currentScale.x / currentSize.x;
		newScale.y = newSize.y * currentScale.y / currentSize.y;
		newScale.z = newSize.z * currentScale.z / currentSize.z;

		if (newScale.x > 0)
			gameObject.transform.localScale = newScale;
	}
	
	public static void SetSize(GameObject gameObject, float newSize)
	{
		GameObjectUtils.SetSize( gameObject , new Vector3( newSize , newSize , newSize ));
	}

	// Source: https://stackoverflow.com/questions/11949463/how-to-get-size-of-parent-game-object
	public static Bounds GetBounds(GameObject gameObject , bool includeChildren = true )
	{
		Bounds bounds = new Bounds();
		
		if (includeChildren)
		{
			// Define center point of bounds
			Vector3 center = Vector3.zero;
			foreach (Transform child in gameObject.transform)
			{
				center += child.gameObject.GetComponent<Renderer>().bounds.center;
			}

			center /= gameObject.transform.childCount;

			// Calculate bounds of children
			bounds = new Bounds(center, Vector3.zero);
			foreach (Transform child in gameObject.transform)
			{
				bounds.Encapsulate(child.gameObject.GetComponent<Renderer>().bounds);
			}
		}
		else
		{
			return gameObject.GetComponent<Renderer>().bounds;
		}

		return bounds;
	}

	public static List<T> FindObjectsOfTypeAndName<T>(string name) where T : MonoBehaviour
	{
		MonoBehaviour[] firstList = GameObject.FindObjectsOfType<T>();
		List<T> finalList = new List<T>();

		for (var i = 0; i < firstList.Length; i++)
		{
			if (firstList[i].name == name)
			{
				finalList.Add(firstList[i] as T);
			}
		}

		return finalList;
	}

	public static void SetVisibility(GameObject gameObject, bool visibilty = true)
	{
		MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
		renderer.enabled = visibilty;
	}


	public static void SetAllComponentsEnabled<T>(GameObject gameObject, bool value)
	{
		T[] myComponents = gameObject.GetComponents<T>();
		T[] childComponents = gameObject.GetComponentsInChildren<T>();
		T[] allComponents = myComponents.Concat(childComponents).ToArray();
		foreach (T component in allComponents)
		{
			try
			{

				PropertyInfo propertyInfo = component.GetType().GetProperty("enabled");
				propertyInfo.SetValue(component, Convert.ChangeType(value, propertyInfo.PropertyType));
			}
			catch (Exception ex)
			{
				Debug.Log(component + " >> FAILED: " + ex.Message);
			}

		}
	}


	public static float GetHitPointTransparency(RaycastHit hit)
	{
		Renderer hitRenderer = hit.collider.GetComponent<Renderer>();

		try
		{
			if (hitRenderer == null || hitRenderer.sharedMaterial == null || hitRenderer.sharedMaterial.mainTexture == null)
				return -1;

			Texture2D hitTexture = (Texture2D)hitRenderer.material.mainTexture;
			Vector2 pixelUV = hit.textureCoord;
			pixelUV = new Vector2(pixelUV.x * hitTexture.width, pixelUV.y * hitTexture.height);
			Color pixelValue = hitTexture.GetPixel((int)pixelUV.x, (int)pixelUV.y);

			return pixelValue.grayscale;
		}
		catch (Exception)
		{

		}

		return -1;
	}
}
