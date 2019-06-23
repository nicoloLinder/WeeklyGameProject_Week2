public class HalfEdge
{
    //The next edge
    public HalfEdge nextEdge;

    //The edge going in the opposite direction
    public HalfEdge oppositeEdge;

    //The previous
    public HalfEdge prevEdge;

    //The face this edge is a part of
    public Triangle t;

    //The vertex the edge points to
    public Vertex v;

    //This structure assumes we have a vertex class with a reference to a half edge going from that vertex
    //and a face (triangle) class with a reference to a half edge which is a part of this face 
    public HalfEdge(Vertex v)
    {
        this.v = v;
    }
}