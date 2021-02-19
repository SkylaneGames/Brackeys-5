using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPuzzleElement
{
    bool IsComplete();
    void SetColor(Vector4 color, float intensity);
}
