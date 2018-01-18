using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleCrtl : MonoBehaviour {
	/// <summary>
	/// 选择中心点
	/// </summary>
	public Vector3 basePoint = new Vector3(0f,0f,0f);
	/// <summary>
	/// 半径
	/// </summary>
	public float radius = 1f;
	/// <summary>
	/// 角度
	/// </summary>
	public float angle = -90f;
	/// <summary>
	/// 目标角度
	/// </summary>
	public float targetAngle = -90f;
	/// <summary>
	/// 每秒旋转度数
	/// </summary>
	public float angleSpeed = 1f;
	/// <summary>
	/// 角度步长
	/// </summary>
	public float angleStep = 1f;

	public float angleMax = 0f;

	public float angleMin = -180f;

	public Vector3 eulerAngles = new Vector3(0f,0f,0f);
	// Use this for initialization
	void Start () {
		
		Set ();

	}
	// Update is called once per frame
	virtual protected void Update () {

		if (targetAngle != angle) {
			
			if (Mathf.Abs (targetAngle - angle) > angleSpeed) {
				angle += (targetAngle - angle) * Time.deltaTime * angleSpeed;
			} else {
				angle = targetAngle;
			}
			Set ();
		}

	}

	virtual public void RunStep(){
		targetAngle = angle + angleStep;
	}

	virtual public void Set(){
		transform.position = GetPosition_R_Z(basePoint,angle,radius);
		eulerAngles.z = angle + 90f;
		transform.eulerAngles = eulerAngles;
	}

	public static Vector3 GetPosition_R_Z(Vector3 basePoint,float angle,float radius) {

		Vector3 position;

		if(radius > 0)
		{
			position.x = basePoint.x - radius * Mathf.Cos((angle) * Mathf.Deg2Rad);
			position.y = basePoint.y - radius * Mathf.Sin((angle) * Mathf.Deg2Rad);
		}
		else
		{
			position.x = basePoint.x + radius * Mathf.Cos((angle) * Mathf.Deg2Rad);
			position.y = basePoint.y + radius * Mathf.Sin((angle) * Mathf.Deg2Rad);
		}

		position.z = basePoint.z;

		return position;
	}

	public static Vector3 GetPosition_R_Y(Vector3 basePoint,float angle,float radius) {

		Vector3 position;

		if(radius > 0)
		{
			position.x = basePoint.x - radius * Mathf.Cos((angle) * Mathf.Deg2Rad);
			position.z = basePoint.z - radius * Mathf.Sin((angle) * Mathf.Deg2Rad);
		}
		else
		{
			position.x = basePoint.x + radius * Mathf.Cos((angle) * Mathf.Deg2Rad);
			position.z = basePoint.z + radius * Mathf.Sin((angle) * Mathf.Deg2Rad);
		}

		position.y = basePoint.y;

		return position;
	}

	public static Vector3 GetPosition_R_X(Vector3 basePoint,float angle,float radius) {

		Vector3 position;

		if(radius > 0)
		{
			position.y = basePoint.y - radius * Mathf.Cos((angle) * Mathf.Deg2Rad);
			position.z = basePoint.z - radius * Mathf.Sin((angle) * Mathf.Deg2Rad);
		}
		else
		{
			position.y = basePoint.y + radius * Mathf.Cos((angle) * Mathf.Deg2Rad);
			position.z = basePoint.z + radius * Mathf.Sin((angle) * Mathf.Deg2Rad);
		}

		position.x = basePoint.x;

		return position;
	}

}
