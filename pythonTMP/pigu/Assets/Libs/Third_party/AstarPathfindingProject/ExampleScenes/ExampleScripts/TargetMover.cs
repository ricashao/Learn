using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Pathfinding {
    using Pathfinding.RVO;
    using Pathfinding.Util;
    /** Moves the target in example scenes.
	 * This is a simple script which has the sole purpose
	 * of moving the target point of agents in the example
	 * scenes for the A* Pathfinding Project.
	 *
	 * It is not meant to be pretty, but it does the job.
	 */
    [HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_target_mover.php")]
	public class TargetMover : MonoBehaviour {
		/** Mask for the raycast placement */
		public LayerMask mask;

		public Transform target;
        public Transform player;
		AIPath[] ais2;
		AILerp[] ais3;

		/** Determines if the target position should be updated every frame or only on double-click */
		public bool onlyOnDoubleClick;
		public bool use2D;

        int m_stepIndex = 0;
        List<Vector3> m_stepVec;
        bool isMove = false;

		Camera cam;

		public void Start () {
			//Cache the Main Camera
			cam = Camera.main;
			ais2 = FindObjectsOfType<AIPath>();
			ais3 = FindObjectsOfType<AILerp>();

			useGUILayout = false;
            /*
            AstarPath.OnPathPreSearch = OnPathPreSearch;
            AstarPath.OnPathPostSearch = OnPathPostSearch;
            */
        }

        public void OnPathPreSearch(Path p){
            Debug.Log(p);
        }
        /*
        public void OnPathPostSearch(Path p)
        {
            string pathStr = "";

            foreach (Vector3 p3 in p.vectorPath) {
                pathStr += "," + p3.ToString();
            }

            m_stepIndex = 1;
            m_stepVec = p.vectorPath;
            isMove = true;
            lastDir = player.transform.rotation;

            Debug.Log("path => " + pathStr);
        }
        */
        public void OnGUI () {
			if (onlyOnDoubleClick && cam != null && Event.current.type == EventType.MouseDown && Event.current.clickCount == 2) {
				UpdateTargetPosition();
			}
		}
        /*
        Quaternion lastDir;
        public bool RotationToTarget(Vector3 dir)
        {
            player.transform.LookAt(new Vector3(dir.x, player.transform.position.y, dir.z));

            if (lastDir != player.transform.rotation)
            {
                userMoveCmd cmd = new userMoveCmd();
                cmd.sceneObjId = DataManager.getInstance().m_userData.sceneObjectId;
                cmd.type = 1;
                cmd.pos.dir_x = (int)(dir.x * 1000);
                cmd.pos.dir_z = (int)(dir.z * 1000);
                cmd.serialize();
                cmd.SendCmd();

                lastDir = player.transform.rotation;
            }
            return true;
        }


        bool isSend = false;
        public bool RunToTarget(Vector3 _target)
        {
            CharacterController cc = player.gameObject.GetComponent<CharacterController>();
            cc.SimpleMove(cc.transform.forward * 4.0f);

            if (!isSend)
            {
                userMoveCmd cmd = new userMoveCmd();
                cmd.sceneObjId = DataManager.getInstance().m_userData.sceneObjectId;
                cmd.type = 2;
                cmd.pos.ix = (int)(cc.transform.position.x * 1000);
                cmd.pos.iz = (int)(cc.transform.position.z * 1000);
                cmd.speed = 4 * 1000;
                cmd.serialize();
                cmd.SendCmd();
                isSend = true;
            }
            

            if (Mathf.Abs(cc.transform.position.x - _target.x) <= 0.1f && Mathf.Abs(cc.transform.position.z - _target.z) <= 0.1f)
            {
                isSend = false;    
                return true;
            }

            return false;
        }

        // Update is called once per frame
        void Update () {
			if (!onlyOnDoubleClick && cam != null) {
				UpdateTargetPosition();
			}

            if (isMove)
            {
                if(m_stepIndex >= m_stepVec.Count)
                {
                    userMoveCmd cmd = new userMoveCmd();
                    cmd.sceneObjId = DataManager.getInstance().m_userData.sceneObjectId;
                    cmd.type = 3;
                    cmd.pos.ix = (int)(player.transform.position.x * 1000);
                    cmd.pos.iz = (int)(player.transform.position.z * 1000);
                    cmd.serialize();
                    cmd.SendCmd();

                    isMove = false;
                    return;
                }
                RotationToTarget(m_stepVec[m_stepIndex]);
                if (RunToTarget(m_stepVec[m_stepIndex]))
                {
                    m_stepIndex++;
                }
            }
		}
        */
		public void UpdateTargetPosition () {
			Vector3 newPosition = Vector3.zero;
			bool positionFound = false;

			if (use2D) {
				newPosition = cam.ScreenToWorldPoint(Input.mousePosition);
				newPosition.z = 0;
				positionFound = true;
			} else {
				//Fire a ray through the scene at the mouse position and place the target where it hits
				RaycastHit hit;
				if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, mask)) {
					newPosition = hit.point;
					positionFound = true;
				}
			}

			if (positionFound && newPosition != target.position) {
				target.position = newPosition;

				if (onlyOnDoubleClick) {
					if (ais2 != null) {
						for (int i = 0; i < ais2.Length; i++) {
							if (ais2[i] != null) ais2[i].SearchPath();
						}
					}

					if (ais3 != null) {
						for (int i = 0; i < ais3.Length; i++) {
							if (ais3[i] != null) ais3[i].SearchPath();
						}
					}
				}
			}
		}
	}
}
