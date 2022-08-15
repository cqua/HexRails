using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridRender : MonoBehaviour
{
    float CELL_SIZE = 16;
    float GridHeight = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int x = -10; x < 10; x++) {
            for (int z = -10; z < 10; z++) {
                Vector3 cellOrigin = new Vector3(x * CELL_SIZE * 3 + (z % 2 == 0 ? 0 : CELL_SIZE * 1.5f), GridHeight, z * CELL_SIZE);

                Vector3[] hexagonPoint = {
                    cellOrigin + new Vector3(CELL_SIZE, 0, 0),
                    cellOrigin + new Vector3(CELL_SIZE / 2, 0, CELL_SIZE),
                    cellOrigin + new Vector3(-CELL_SIZE / 2, 0, CELL_SIZE),
                    cellOrigin + new Vector3(-CELL_SIZE, 0, 0)
                    //cellOrigin + new Vector3(-CELL_SIZE / 2, 0, -CELL_SIZE),
                    //cellOrigin + new Vector3(CELL_SIZE / 2, 0, -CELL_SIZE)
                    };

                Debug.DrawLine(hexagonPoint[0], hexagonPoint[1]);
                Debug.DrawLine(hexagonPoint[1], hexagonPoint[2]);
                Debug.DrawLine(hexagonPoint[2], hexagonPoint[3]);
                //Debug.DrawLine(hexagonPoint[3], hexagonPoint[4]);
                //Debug.DrawLine(hexagonPoint[4], hexagonPoint[5]);
                //Debug.DrawLine(hexagonPoint[5], hexagonPoint[0]);
            }
        }
	}
}
