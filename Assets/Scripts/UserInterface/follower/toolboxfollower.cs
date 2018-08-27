/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace IMRE.HandWaver
{
    public class toolboxfollower : MonoBehaviour
    {
        public Transform player;


        [Range(0f,2f)]
        public float minDist;
        [Range(0f, 5f)]
        public float maxDist;
        [Range(0f, 2f)]
        public float followerheight;
        public GameObject bone;

        [Range(0f, 10f)]
        public float speed;
        private float initSpeed;

        public float increaseSpeed;
        public int speedup_distance;

        private bool nearPlayer = false;

        public bool isParked = true;
        public Vector3 parkSpot;            //-1.85, 1.1, -1.2


        float Dist;

        void Start()
        {
			//player = GameObject.Find("CenterEyeAnchor").transform;  //need to expand upon
			player = Camera.main.transform;
			initSpeed = speed;
        }

        void Update()
        {
            if (!isParked || bone != null)
            {
                FindDist();
            }
        }

        void FindDist()
        {

			nearPlayer = true;
            if (bone != null)
            {
                nearPlayer = false;
                Dist = Vector3.Magnitude(Vector3.ProjectOnPlane((bone.transform.position - this.transform.position), Vector3.up));

                if (Dist > maxDist)
                {

                    if (Dist > speedup_distance) { speed += increaseSpeed; }
                    Summon(bone.transform, false);
                    if (Vector3.Magnitude(Vector3.ProjectOnPlane((player.transform.position - this.transform.position), Vector3.up))>3.0f)
                    {
                        speed = initSpeed;
                        //increaseSpeed = 1.2f;
                        bone.GetComponent<boneBehave>().despawn();
                    }
                }
                else
                {
                    speed = initSpeed;
                    //increaseSpeed = 1.2f;
                    bone.GetComponent<boneBehave>().despawn();
                }                
            }
            else
            {
                Dist = Vector3.Magnitude((player.transform.position - this.transform.position));
                if (Dist > maxDist)
                {
                    nearPlayer = false;
                    Summon(player, true);
                }
                else if(Dist < minDist)
                {
					isParked = true;
                    //BackUp(player);
                }
            }

        }
 
        void Summon(Transform player, bool useMinDist)
        {
            Vector3 Target = player.transform.position.x*Vector3.right + player.transform.position.z*Vector3.forward + followerheight*Vector3.up;

			if (useMinDist)
            {

				//this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.FromToRotation(this.transform.forward, player.transform.position - this.transform.position), 5f);
				//transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position);
				transform.rotation = Quaternion.LookRotation(transform.position - player.transform.position);


				transform.position = Vector3.MoveTowards(transform.position, Target, speed * Mathf.Abs(Dist - minDist) * Time.deltaTime);
            }
            else
            {
				transform.rotation = Quaternion.LookRotation(bone.transform.position - transform.position);

				//this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.FromToRotation(this.transform.forward, player.transform.position - this.transform.position), 5f);
				transform.position = Vector3.Lerp(transform.position, bone.transform.position, Mathf.Max(2f,10f*Dist) * Time.deltaTime);
            }
        }

        void BackUp(Transform player)
        {
            Vector3 Target = (player.transform.position - new Vector3(0, player.transform.position.y, 0) + new Vector3((UnityEngine.Random.Range(-maxDist, maxDist)), followerheight, (UnityEngine.Random.Range(-maxDist, maxDist))));
            transform.position = Vector3.MoveTowards(transform.position, Target, -2f * Time.deltaTime);
            //Debug.Log("BackUp");
        }
        /// <summary>
        /// if tools are in a spawnable state
        /// </summary>
        /// <returns>boolean of if follower is near player or it is parked</returns>
        public bool spawnable()
        {
            return (nearPlayer || isParked);
        }

        /// <summary>
        /// Sends the follower to stay in one spot. If already parked, follower is sent back to player.
        /// </summary>
        public void parkFollower()
        {			
			/*  REMOVE WHEN FEATURE IS FULLY FUNCTIONAL */
			/**/if(playMode.demo)					/*  */
			/**/	return ;						/*  */
			/*  REMOVE WHEN FEATURE IS FULLY FUNCTIONAL */

            if (isParked) {
                unParkFollower();
            }
        }

        private void unParkFollower()
        {
            isParked = false;
        }
    }
}
