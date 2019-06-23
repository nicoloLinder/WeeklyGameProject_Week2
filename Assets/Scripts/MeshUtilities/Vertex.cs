using UnityEngine;

public class Vertex
{
    //The outgoing halfedge (a halfedge that starts at this vertex)
    //Doesnt matter which edge we connect to it
    public HalfEdge halfEdge;
    public bool isConvex;
    public bool isEar;

    //Properties this vertex may have
    //Reflex is concave
    public bool isReflex;
    public Vertex nextVertex;
    public Vector3 position;

    //The previous and next vertex this vertex is attached to
    public Vertex prevVertex;

    //Which triangle is this vertex a part of?
    public Triangle triangle;

    public Vertex(Vector3 position)
    {
        this.position = position;
    }

    //Get 2d pos of this vertex
    public Vector2 GetPos2D_XZ()
    {
        var pos_2d_xz = new Vector2(position.x, position.z);

        return pos_2d_xz;
    }
}