using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tree : MonoBehaviour
{
    [SerializeField] private GameObject branchPrefab;
    [SerializeField] private float rootLength, reductionLengthPerLevel;
    [SerializeField] private int recursionDepth;
    private float _currentLength;
    private int _currentDepth = 0;
    
    private readonly Queue<GameObject> _frontier = new Queue<GameObject>();   

    private void Start()
    {
        _currentLength = rootLength;

        var root = Instantiate(branchPrefab,transform);
        SetBranchLenght(root, rootLength);

        _frontier.Enqueue(root);
        GenerateTree();
    }
    private void GenerateTree()
    {
        if (_currentDepth >= recursionDepth) return;
        
        ++_currentDepth;
        
        rootLength = rootLength - rootLength * reductionLengthPerLevel;

        var levelNodes = new List<GameObject>();

        while (_frontier.Count > 0)
        {
            var branch = _frontier.Dequeue();

            var leftBranch = CreateBranch(branch, Random.Range(10, 30));
            var rightBranch = CreateBranch(branch, -Random.Range(10, 30));

            levelNodes.Add(leftBranch);
            levelNodes.Add(rightBranch);
        }
        foreach (var node in levelNodes)
        {
            _frontier.Enqueue(node);
        }
        
        GenerateTree();
    }
    private GameObject CreateBranch(GameObject previousBranch, float angle)
    {
        var branch = Instantiate(branchPrefab, transform);
        
        branch.transform.position = previousBranch.transform.position + previousBranch.transform.up * _currentLength;
        var previousRotation = previousBranch.transform.rotation;

        SetBranchLenght(branch, _currentLength);
        previousRotation *= Quaternion.Euler(0, 0, angle);  
        branch.transform.rotation = previousRotation;
       
        return branch;
    }
    private void SetBranchLenght(GameObject branch, float length)
    {
        var line = branch.transform.GetChild(0);
        var node = branch.transform.GetChild(1);
        
        line.localScale = new Vector3(line.localScale.x, length, line.localScale.z);
        line.localPosition = new Vector3(0f, length * 0.5f, 0f);
        node.localPosition = new Vector3(0f, length, 0f);
    }
}
