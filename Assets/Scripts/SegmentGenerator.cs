using System.Collections;
using UnityEngine;

public class SegmentGenerator : MonoBehaviour {


    public GameObject[] segment;

    [SerializeField] int xPos = -50;
    [SerializeField] bool creatingSegment = false;
    [SerializeField] int segmentNum;
    void Update() {

        if(creatingSegment == false) {
            creatingSegment = true;
            StartCoroutine(SegmentGen());
        } 
    }
    
    IEnumerator SegmentGen() {
        segmentNum = Random.Range(0, 1); // In case we want to add more segments later
        GameObject clone = Instantiate(segment[segmentNum], new Vector3( xPos, 0, 0), Quaternion.identity);
        Destroy(clone, 30f);
        xPos -= 50;
        yield return new WaitForSeconds(10);
        creatingSegment = false;
    }
    
}
