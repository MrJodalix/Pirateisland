/**
 * Script zum erstellen einer Splinefläche für den Sand.
 * Hier für wird das ursprungliche Mesh des Objektes zerstört und durch eine Splinefläche asu B-Splines in den
 * angegebenen Größen erstellt. Zudem werden Boxcollider zwischen den Kontrollpunkten erstellt, welche durch
 * Objekte mit den SplineSpade-Skript getroffen werden können und so die um leigenden Kontrollpunkte niedriger 
 * setzten kann. Dabei kann zusätzlich eine Truhe dem Skript hinzugefügt werden, welche beim treffen des 
 * angegebenen Colliders auftaucht.
 * @author Moritz Richter
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class GeneratePlaneMesh : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Größe in X Richtung (verändert nicht die größe der angezeigten Fläche, sondern nur der zur Laufzeit erstellten Kontrollpunkte)")]
    float sizeX = 1.0f;

    [SerializeField]
    [Tooltip("Größe in Z Richtung (verändert nicht die größe der angezeigten Fläche, sondern nur der zur Laufzeit erstellten Kontrollpunkte)")]
    float sizeZ = 1.0f;

    [SerializeField]
    [Tooltip("zeigt die Kontrollpunkte durch Spehren an")]
    bool debugSpheres = false;

    [SerializeField]
    [Tooltip("Anzahl der Kontrollpunkte")]
    int controllPoints = 4;

    [SerializeField]
    [Tooltip("Auflösung der Splinefläche (immer >= 1)")]
    int subDivs = 1;

    [SerializeField]
    [Tooltip("Höhen-Bereich der Kontrollpunkte (-heightDif bis heightDif)")]
    float heightDif = 0.2f;

    [SerializeField]
    [Tooltip("Wie weit die Kontrollpunkte runter gesetzt werden")]
    float lowering = 0.1f;

    [SerializeField]
    [Tooltip("Größe der Box-Collider in Y")]
    float colliderSizeY = 0.9f;

    [SerializeField]
    [Tooltip("Höhe der Box-Collider")]
    float colliderHeight = 1.0f;

    [SerializeField]
    [Tooltip("Position der Kiste")]
    int[] chestPos = new int[2]{ 1, 1 };

    [SerializeField]
    [Tooltip("Kiste, welche im Sand versteckt ist")]
    GameObject chest;

    /* Anzahl an Vertices in einer Richtung*/
    private int gridSize;

    /* Index der Kiste */
    private int chestIdx;

    /* Mesh des Objektes */
    private MeshFilter filter;

    /* start Position der Fläche */
    private Vector3 startPos;

    /* eine Normale */
    private Vector3 normal = new Vector3(0.0f, 0.0f, 0.0f);

    /* Array con Kontrollpunkten*/
    private List<Vector3> points = new List<Vector3>();

    /* Liste der Box-Collider*/
    private List<GameObject> colliders = new List<GameObject>();

    /* B-Spline Matrix */
    private int[][] g_bSpline = new int[4][]
        {
            new int[]{-1,  3, -3,  1},
            new int[]{ 3, -6,  3,  0},
            new int[]{-3,  0,  3,  0},
            new int[]{ 1,  4,  1,  0}
        };

    /* transponierte B-Spline Matrix */
    private int[][] g_bSplineT = new int[4][]
        {
            new int[]{-1,  3, -3,  1},
            new int[]{ 3, -6,  0,  4},
            new int[]{-3,  3,  3,  1},
            new int[]{ 1,  0,  0,  0}
        };

    
    

 /*--------------------------------------- FUNTKIONEN ------------------------------------------------------*/

    // Initalisiert die Größen der Splinefläche, ersetzt die Box-Collider und start das erstellen der Fläche
    void Start()
    {
        controllPoints = controllPoints < 4 ? 4 : controllPoints;
        subDivs = subDivs < 1 ? 1 : subDivs;
        gridSize = (controllPoints-3) * subDivs + 1;
        startPos = gameObject.transform.localPosition;
        setControllPoints();
        int idx = 0;

        for (int x = 1; x < (controllPoints - 2); x++){
            for (int y = 1; y < (controllPoints - 2); y++){
                colliders.Add(new GameObject("BoxCollider_" + x + "_" + y));
                colliders[idx].tag = "Sand";
                BoxCollider boxCollider = colliders[idx].AddComponent<BoxCollider>();
                colliders[idx].AddComponent<SplinePlaneCollider>();
                boxCollider.isTrigger = false;
                if (x == chestPos[0] && y == chestPos[1]){
                    chestIdx = idx;
                }
                idx++;
            }
        }
        
        initPlane(); 
    }

    // Ersetzt das Mesh durch eine neue Fläche
    void initPlane ()
    {
        filter = GetComponent<MeshFilter>();
        filter.mesh = GenerateMesh(); 
    }

    // Setzt die Kontrollpunkte
    void setControllPoints ()
    {
        for (int x = 0; x < controllPoints; x++){
            for (int z = 0; z < controllPoints; z++){
                float height = Random.Range(-heightDif, heightDif);

                points.Add(new Vector3(0.5f * sizeX - x * sizeX / ((float)controllPoints - 1), 
                                         height,
                                         0.5f * sizeZ - z * sizeZ / ((float)controllPoints - 1)));

                // Spheren für die Kontrollpunkte, fürs Debugen
                if (debugSpheres)
                {
                    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    sphere.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
                    sphere.transform.localPosition = new Vector3(0.5f * sizeX - x * sizeX / ((float)controllPoints - 1), 
                                                                height,
                                                                0.5f * sizeZ - z * sizeZ / ((float)controllPoints - 1)) 
                                                                + startPos;
                }
            }
        }
    }

    // Generiet die Splinefläche 
    Mesh GenerateMesh()
    {
        Mesh mesh = new Mesh();

        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();


        var vertCount = gridSize;
        // erstellen der Vertices und Normal Liste, und fügt die UVs hinzu
        for (int x = 0; x < vertCount; x++) {
            for (int y = 0; y < vertCount; y++)
            {
                // Vertices für Mesh
                vertices.Add(new Vector3());
                normals.Add(new Vector3());
                
                uvs.Add(new Vector2(x / (float)vertCount, y / (float)vertCount));
            }
        }

        calcAll(vertices);
        calcNomals(normals);
        CreateBoxColliders();   

        // Erstellen der Indices-Liste
        var triangles = new List<int>();
        int tris = (gridSize-1) * (gridSize-1);
        int row = 0;
        int vert = 0;
        for(int i = 0; i < tris; i++){
            if ((vert % (gridSize-1)) == 0 && vert != 0){
                row++;
            }
            int v0 = vert++ + row;
            int v1 = v0 + 1;
            int v2 = v0 + gridSize;
            int v3 = v2 + 1;

            triangles.Add(v0);
            triangles.Add(v1);
            triangles.Add(v3);
            
            triangles.Add(v0);
            triangles.Add(v3);
            triangles.Add(v2);
        }


        mesh.SetVertices(vertices);
        mesh.SetNormals(normals);
        mesh.SetUVs(0, uvs);
        mesh.SetTriangles(triangles, 0);

        return mesh;
    }

    // Setzt die Boxcollider Größen
    void CreateBoxColliders()
    {
        int idx = 0;
        for (int x = 1; x < (controllPoints - 2); x++){
            for (int y = 1; y < (controllPoints - 2); y++){
                int index = x * controllPoints + y;
                float midX = (points[index][0] + points[index + controllPoints][0]) / 2.0f;
                float midZ = (points[index][2] + points[index + 1][2]) / 2.0f;
                float maxY = points[index][1];
                float minY = points[index][1];
                int[] indices = new int[]{index + 1, index +controllPoints, index + 1 + controllPoints};
                
                for (int i = 0; i < 3; i++){
                    float currY = points[indices[i]][1];
                    if (currY > maxY){
                        maxY = currY;
                    } else if (currY < minY) {
                        minY = currY;
                    }
                }
                
                float midY = (maxY + minY) / 2.0f - colliderHeight;

                BoxCollider boxCollider = colliders[idx].GetComponent<BoxCollider>();
            
                colliders[idx].transform.position = new Vector3(midX, midY, midZ) + startPos;


                boxCollider.size = new Vector3(points[index][0] - points[index + controllPoints][0],
                                               (maxY - minY) * colliderSizeY + colliderHeight * 2.0f,
                                               points[index][2] - points[index + 1][2]);

                
                colliders[idx].GetComponent<SplinePlaneCollider>().setConnection(index, gameObject);

                idx++;
            }
        }
    }

    // setzt die Punkte runter, welche getroffen wurden
    public void lowerPoints (int index) {
        // Temporäre Variablen für die Punkte erstellen
        Vector3 point1 = points[index];
        Vector3 point2 = points[index + 1];
        Vector3 point3 = points[index + controllPoints];
        Vector3 point4 = points[index + 1 + controllPoints];
        float minHeight = startPos.y - heightDif - 2 * lowering;
        int ColliderIdx = (int)(index / controllPoints) * (controllPoints - 3) + (int)(index % controllPoints) - (controllPoints - 2);

        if (colliders[ColliderIdx].transform.position.y  >= (minHeight - colliderHeight))
        {
            // Die y-Komponente der Punkte verringern
            point1.y -= lowering;
            point2.y -= lowering;
            point3.y -= lowering;
            point4.y -= lowering;

            // Die modifizierten Punkte wieder in die Liste einfügen
            points[index] = point1;
            points[index + 1] = point2;
            points[index + controllPoints] = point3;
            points[index + 1 + controllPoints] = point4;

            initPlane();
            chest.transform.position = new Vector3(chest.transform.position.x, startPos.y + calcSplinePoint(1, 0.5f, 0.5f, chestIdx) - 0.05f, chest.transform.position.z);
        }

        if (ColliderIdx == chestIdx && chest != null)
        {
            chest.SetActive(true);
        }
    }


    /**
    * verechnet die 4x4 B-Spline-Matrix mit der 4x4 Geometriematrix: M * G
    * @param axis Axe für die Geometriedaten
    * @param i Index für die Geometriedaten
    * @param bg zubefüllende 4x4 Matrix
    */
    void bspline_X_geoMat(int axis, int i, float[][] bxg){
        int cp = controllPoints;
        float[][] geoMat = new float[4][] {
            new float[4] { points[i][axis], points[i + cp][axis], points[i + 2 * cp][axis], points[i + 3 * cp][axis] },
            new float[4] { points[i + 1][axis], points[i + cp + 1][axis], points[i + 2 * cp + 1][axis], points[i + 3 * cp + 1][axis] },
            new float[4] { points[i + 2][axis], points[i + cp + 2][axis], points[i + 2 * cp + 2][axis], points[i + 3 * cp + 2][axis] },
            new float[4] { points[i + 3][axis], points[i + cp + 3][axis], points[i + 2 * cp + 3][axis], points[i + 3 * cp + 3][axis] }
        };

        for (int l = 0; l < 4; l++) {
            bxg[l] = new float[4];
            for (int j = 0; j < 4; j++) {
                bxg[l][j] = 0;
                for (int k = 0; k < 4; k++) {
                    bxg[l][j] += g_bSpline[l][k] * geoMat[k][j];
                }
            }
        }
    }

    /**
    * Berechnet eine Koordinate für ein gegebenes s,t auf der gegebenen Axe
    * @param axis Axe die berechnet werden soll
    * @param s derzeitiger s wert
    * @param t derzeitiger t wert
    * @param i Index des ersten Kontrollpunktder, 4*4 benötigten Punkte
    * @return die Koordinate für die Axe und den s,t Wert
    */
    float calcSplinePoint (int axis, float s, float t, int i){
        // Monom-Arrays für s und t
        float[] sMonom = new float[] { Mathf.Pow(s, 3), Mathf.Pow(s, 2), s, 1.0f };
        float[] tMonom = new float[] { Mathf.Pow(t, 3), Mathf.Pow(t, 2), t, 1.0f };

        // Berechne bxt als Produkt von tMonom und der transponierten B-Spline-Matrix
        float[] bxt = new float[4];
        for (int m = 0; m < 4; m++) {
            bxt[m] = tMonom[0] * g_bSplineT[m][0] + tMonom[1] * g_bSplineT[m][1] + tMonom[2] * g_bSplineT[m][2] + tMonom[3] * g_bSplineT[m][3];
        }

        // Kontrollpunkte (Geometriematrix)
        float[][] bxg = new float[4][];
        bspline_X_geoMat(axis, i, bxg);

        // Berechne sxbg als Produkt von sMonom und der transponierten B-Spline-Matrix
        float[] sxbg = new float[4];
        for (int m = 0; m < 4; m++) {
            sxbg[m] = sMonom[0] * bxg[0][m] + sMonom[1] * bxg[1][m] + sMonom[2] * bxg[2][m] + sMonom[3] * bxg[3][m];
        }

        // Rückgabe des Ergebnisses
        return (1.0f / 36.0f) * (sxbg[0] * bxt[0] + sxbg[1] * bxt[1] + sxbg[2] * bxt[2] + sxbg[3] * bxt[3]);
    }

    /**
    * berchent mit Hilfe von calcSplinePoint alle X,Y,Z Koordinaten für die
    * Spline-Fläche
    * @param vertices Zeiger auf das zufüllende Vertex-Array
    * @param subDivs Aufteilung der Spline-Fläche
    * @param gridSize Anzahl der Punkte pro Zeile/Splate
    */
    void calcAll (List<Vector3> vertices){
        int cp = controllPoints;
        int index = 0;
        int currCP = 0;
        int numVert = subDivs + 1;

        // Durchgehen der 4x4 Kontropunkt-Paare
        for (int cpX = 0; cpX < (cp-3); cpX++){
            for (int cpZ = 0; cpZ < (cp-3); cpZ++){
                currCP = cpZ + cpX * cp;
                // Setzten aller Spline Punkte ohne welche doppelt zuberechnen
                for (int s = 0; s < numVert; s++){
                    for (int t = 0; t < numVert; t++){
                        if ((cpX == 0 || t != 0) && (cpZ == 0 || s != 0)) {
                            index = s + t * gridSize + cpZ * subDivs + cpX * gridSize * subDivs;
                            vertices[index] = new Vector3(calcSplinePoint(0, (float)s/subDivs, (float)t/subDivs, currCP),
                                                          calcSplinePoint(1, (float)s/subDivs, (float)t/subDivs, currCP),
                                                          calcSplinePoint(2, (float)s/subDivs, (float)t/subDivs, currCP));                             
                        }
                    }
                }
            }
        }  
    }

    /**
    * Berechnung der partielen Ableitung nach s
    * @param bg 4x4 matrix von B-Spline-Matrix * Geometriematrix
    * @param s derzeitiger s wert
    * @param t derzeitiger t wert
    * @return Wert der partiellen Ableitung für ein s,t
    */
    float calcDerivationS(float[][] bg, float s, float t) {
        float ds = 0.0f;
        for (int i = 0; i < 4; i++) {
            ds += 3.0f * Mathf.Pow(s, 2) * (bg[0][i] * g_bSplineT[i][0] * Mathf.Pow(t, 3) +
                                            bg[0][i] * g_bSplineT[i][1] * Mathf.Pow(t, 2) +
                                            bg[0][i] * g_bSplineT[i][2] * t +
                                            bg[0][i] * g_bSplineT[i][3]) +
                2.0f * s * (bg[1][i] * g_bSplineT[i][0] * Mathf.Pow(t, 3) +
                            bg[1][i] * g_bSplineT[i][1] * Mathf.Pow(t, 2) +
                            bg[1][i] * g_bSplineT[i][2] * t +
                            bg[1][i] * g_bSplineT[i][3]) +
                bg[2][i] * g_bSplineT[i][0] * Mathf.Pow(t, 3) +
                bg[2][i] * g_bSplineT[i][1] * Mathf.Pow(t, 2) +
                bg[2][i] * g_bSplineT[i][2] * t +
                bg[2][i] * g_bSplineT[i][3];
        }
        return ds / 36.0f;
    }
    /**
    * Berechnung der partielen Ableitung nach t
    * @param bg 4x4 matrix von B-Spline-Matrix * Geometriematrix
    * @param s derzeitiger s wert
    * @param t derzeitiger t wert
    * @return Wert der partiellen Ableitung für ein s,t
    */
    float calcDerivationT(float[][] bg, float s, float t) {
        float dt = 0.0f;
        for (int i = 0; i < 4; i++) {
            dt += 3.0f * Mathf.Pow(t, 2) * (bg[0][i] * g_bSplineT[i][0] * Mathf.Pow(s, 3) +
                                            bg[1][i] * g_bSplineT[i][0] * Mathf.Pow(s, 2) +
                                            bg[2][i] * g_bSplineT[i][0] * s +
                                            bg[3][i] * g_bSplineT[i][0]) +
                2.0f * t * (bg[0][i] * g_bSplineT[i][1] * Mathf.Pow(s, 3) +
                            bg[1][i] * g_bSplineT[i][1] * Mathf.Pow(s, 2) +
                            bg[2][i] * g_bSplineT[i][1] * s +
                            bg[3][i] * g_bSplineT[i][1]) +
                bg[0][i] * g_bSplineT[i][2] * Mathf.Pow(s, 3) +
                bg[1][i] * g_bSplineT[i][2] * Mathf.Pow(s, 2) +
                bg[2][i] * g_bSplineT[i][2] * s +
                bg[3][i] * g_bSplineT[i][2];
        }
        return dt / 36.0f;
    }


    /**
    * Berechnen aller Normalen der Splinefläche durch partielle Ableitung von q(s,t)
    * @param vertices Zeiger auf das zufüllende Vertex-Array
    * @param subDivs Aufteilung der Spline-Fläche
    * @param gridSize Anzahl der Punkte pro Zeile/Splate
    */
    void calcNomals(List<Vector3> normals){
        int cp = controllPoints;
        int index = 0;
        int currCP = 0;
        int numVert = subDivs + 1;
        Vector3 dt = new Vector3();
        Vector3 ds = new Vector3();
        Vector3 normal = new Vector3();

        // Durchgehen der 4x4 Kontrollpunkt-Paare
        for (int cpX = 0; cpX < (cp - 3); cpX++) {
            for (int cpZ = 0; cpZ < (cp - 3); cpZ++) {
                currCP = cpZ + cpX * cp;
                // Setzen aller Spline-Punkte ohne welche doppelt zu berechnen
                for (int s = 0; s < numVert; s++) {
                    for (int t = 0; t < numVert; t++) {
                        if ((cpX == 0 || t != 0) && (cpZ == 0 || s != 0)) {
                            index = s + t * gridSize + cpZ * subDivs + cpX * gridSize * subDivs;
                            // Berechnen der beiden partiellen Ableitungen für die X-Achse an der Stelle t/s
                            float[][] bxg = new float[4][];
                            bspline_X_geoMat(0, currCP, bxg);
                            dt[0] = calcDerivationT(bxg, (float)s / subDivs, (float)t / subDivs);
                            ds[0] = calcDerivationS(bxg, (float)s / subDivs, (float)t / subDivs);
                            // Berechnen der beiden partiellen Ableitungen für die Y-Achse an der Stelle t/s
                            bspline_X_geoMat(1, currCP, bxg);
                            dt[1] = calcDerivationT(bxg, (float)s / subDivs, (float)t / subDivs);
                            ds[1] = calcDerivationS(bxg, (float)s / subDivs, (float)t / subDivs);
                            // Berechnen der beiden partiellen Ableitungen für die Z-Achse an der Stelle t/s
                            bspline_X_geoMat(2, currCP, bxg);
                            dt[2] = calcDerivationT(bxg, (float)s / subDivs, (float)t / subDivs);
                            ds[2] = calcDerivationS(bxg, (float)s / subDivs, (float)t / subDivs);

                            // Kreuzprodukt der Vektoren
                            normal[0] = ds[1] * dt[2] - ds[2] * dt[1];
                            normal[1] = ds[2] * dt[0] - ds[0] * dt[2];
                            normal[2] = ds[0] * dt[1] - ds[1] * dt[0];

                            // Normalisieren
                            float length = Mathf.Sqrt(normal[0] * normal[0] + normal[1] * normal[1] + normal[2] * normal[2]);
                            if (length > 0) {
                                normal[0] /= length;
                                normal[1] /= length;
                                normal[2] /= length;
                            }

                            // Optional: Invertieren der Normalen, falls sie nach innen zeigen
                            normals[index] = new Vector3(normal[0], normal[1], normal[2]);
                        }
                    }
                }
            }
        } 
    }
}
