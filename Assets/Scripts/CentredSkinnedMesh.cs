﻿using System;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class CentredSkinnedMesh : MonoBehaviour
{
    [Serializable]
    public struct BoneMass
    {
        public Transform bone;
        [Range(minRelativeMass, maxRelativeMass)]
        public float relativeDensity;
        public float mass;

        public const float minRelativeMass = 0.2f;
        public const float maxRelativeMass = 2f;

        public BoneMass (Transform bone, float mass)
        {
            this.bone = bone;
            relativeDensity = 1f;
            this.mass = mass;
        }

        public Vector3 GetBoneCentreOfMass ()
        {
            return bone.position * relativeDensity * mass;
        }
    }

    [SerializeField]
    Vector3 m_CentreOfMass; 
    [SerializeField]
    BoneMass[] m_BoneMasses;
    [SerializeField]
    CenterOfMass com = new CenterOfMass();
    public CenterOfMass COM => com;

    [SerializeField]
    GameObject m_SkeletonRoot;

    public void SetBoneMasses (Transform[] bones, float[] weightedMasses)
    {
        if(bones.Length != weightedMasses.Length)
            throw new UnityException("Bones and weighted masses are not the same length.  Make sure there is one weighted mass per bone.");
        
        m_BoneMasses = new BoneMass[bones.Length];

        for (int i = 0; i < m_BoneMasses.Length; i++)
        {
            m_BoneMasses[i] = new BoneMass (bones[i], weightedMasses[i]);
        }
    }

    public Vector3 CalculateCentreOfMass ()
    {
        float summedWeight = 0f;
        com.pos = Vector3.zero;
        for (int i = 0; i < m_BoneMasses.Length; i++)
        {
            com.pos += m_BoneMasses[i].GetBoneCentreOfMass ();
            summedWeight += m_BoneMasses[i].relativeDensity * m_BoneMasses[i].mass;
        }

        com.pos /= summedWeight;
       
        return com.pos;
    }

    void Update()
    {
        m_CentreOfMass = CalculateCentreOfMass();
    }
}
