using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public GameObject[] clouds;
    public GameObject[] trees;
    public int cloudSpacing = 5;
    public int cloudSpacingVariance = 3;
    public int cloudHeight = 5;
    public int cloudHeightVariance = 1;
    public int treeSpacing = 6;
    public int treeSpacingVariance = 2;

    void Start()
    {
        for (var x = -20; x < 20; x++)
        {
            createCloud(x);
            createTree(x);
        }

        for (var x = -20; x < 20; x++)
        {
            createCloud(x);
        }
    }

    void createCloud(int x)
    {
        int randomCloudHeight = Random.Range(cloudHeight - cloudHeightVariance, cloudHeight + cloudHeightVariance);
        int randomCloudSpacing = Random.Range(x * cloudSpacing - cloudSpacingVariance, x * cloudSpacing + cloudSpacingVariance);
        int cloudIndex = Random.Range(0, clouds.Length);
        Instantiate(clouds[cloudIndex], new Vector3(randomCloudSpacing, randomCloudHeight, 11f), Quaternion.identity);
    }

    void createTree(int x)
    {
        int randomTreeSpacing = Random.Range(x * treeSpacing - treeSpacingVariance, x * treeSpacing + treeSpacingVariance);
        int treeIndex = Random.Range(0, trees.Length);
        GameObject tree = trees[treeIndex];
        Vector3 position = new Vector3(randomTreeSpacing, (tree.transform.GetComponent<SpriteRenderer>().bounds.size.y / 2) + 0.15f, 10f);
        Instantiate(tree, position, Quaternion.identity);
    }
}
