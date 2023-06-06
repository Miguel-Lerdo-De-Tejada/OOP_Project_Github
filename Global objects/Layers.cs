using System.Collections;
using System.Collections.Generic;

public class Layers
{
    // ENCAPSULATION
    public static int player { get { return 1 << 3; } private set { } }
    public static int water { get { return 1 << 4; } private set { } }
    public static int npc { get { return 1 << 6; } private set { } }
    public static int ground1 { get { return 1 << 7; } private set { } }
    public static int ground2 { get { return 1 << 8; } private set { } }
    public static int computer { get { return 1 << 9; } private set { } }
    public static int wall { get { return 1 << 10; } private set { } }
    public static int key { get { return 1 << 11; } private set { } }    
    public static int gameManager { get { return 1 << 12; }private set { } }
    public static int patrolPoint { get { return 1 << 13; }private set { } }
    public static int limitZone { get { return 1 << 14; }private set { } }
    public static int keyHoldingLock { get { return 1 << 15; }private set { } }

    public static int ReadLayer(int layer) { return 1 << layer; }
}
