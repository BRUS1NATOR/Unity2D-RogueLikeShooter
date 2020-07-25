using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SimpleFunctions 
{
    private struct PositionAndDistance
    {
        public float distance;
        public Vector2 position;
        public Vector2 closeToThisPosition;

        public PositionAndDistance(float Distance, Vector2 Position, Vector2 CloseToThisPosition)
        {
            distance = Distance;
            position = Position;
            closeToThisPosition = CloseToThisPosition;
        }
    }

    public static float CountPercent(float chance, float overallChance)
    {
        float mult = 100 / overallChance;
        chance *= mult;
        return chance;
    }

    public static List<Vector3> RandomCircle(Vector3 center, float radius, int amount, float offset)
    {
        List<Vector3> coords = new List<Vector3>();
        for (int i = 0; i < amount; i++)
        {
            float ang = i * 360f / (float)amount + offset;
            Vector3 pos;
            pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
            pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
            pos.z = center.z;
            coords.Add(pos);
        }
        return coords;
    }

    public static void DoDamage(Collider2D collider, int damage, int power, Transform curPos)
    {
        IHealth objectWithHealth = collider.gameObject.GetComponent<IHealth>();
        if (objectWithHealth != null)
        {
            if (power > 0)
            {
                Vector2 normalized = -(curPos.position - collider.gameObject.transform.position).normalized;
                if (objectWithHealth.ignoreBulletImpulse == false)
                {
                    objectWithHealth.PushBack(normalized, (float)power);
                }
                objectWithHealth.TakeDamage(damage);
            }
            else
            {
                objectWithHealth.TakeDamage(damage);
            }

        }
    }

    public static void Flip(GameObject obj)
    {
        // Multiply the parent's x local scale by -1.
        Vector3 theScale = obj.transform.localScale;
        theScale.x *= -1;
        obj.transform.localScale = theScale;
    }

    public static bool Check_Superimpose(Vector2 left, Vector2 right, Vector2 left2, Vector2 right2, int distance)  //distance 0 в прямоугольнике 1 касается 2 рядом                                                                                                                    //true => пересекаются
    {                                                                                                               //true => пересекаются
        float maxX, maxY, minX, minY;

        maxX = Max(left2.x, left.x);
        maxY = Max(left2.y, left.y);
        minX = Min(right2.x, right.x);
        minY = Min(right2.y, right.y);

        if (((minX - maxX) > -distance) && ((minY - maxY) > -distance))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool Check_Superimpose(Vector2 pos, Vector2 leftCorner, Vector2 rightCorner)
    {
        if (pos.x > leftCorner.x && pos.x < rightCorner.x)
        {
            if (pos.y > leftCorner.y && pos.y < rightCorner.y)
            {
                return true;
            }
        }
        return false;
    }

    public static DistanceToTheRoom Check_SuperimposeDistanceVector(Room room1, Room room2)  //distance 0 в прямоугольнике 1 касается 2 рядом                                                                                                                    //true => пересекаются
    {
        Vector2 pos_now = room1.start;

        List<PositionAndDistance> distances = new List<PositionAndDistance>();
        PositionAndDistance positionAndDistance = new PositionAndDistance();
        positionAndDistance.distance = 999;
        positionAndDistance.position = new Vector2(0, 0);

        for (int i = 0; i < 4; i++)
        {
            switch (i)
            {
                case 0:
                    pos_now = room1.start;
                    break;
                case 1:
                    pos_now = room1.end;
                    break;
                case 2:
                    pos_now = new Vector2(room1.start.x, room1.end.y);
                    break;
                case 3:
                    pos_now = new Vector2(room1.end.x, room1.start.y);
                    break;
            }


            distances.Add(new PositionAndDistance(Distance_BetweenVectors(pos_now, room2.start), pos_now, room2.start));

            distances.Add(new PositionAndDistance(Distance_BetweenVectors(pos_now, room2.end), pos_now, room2.end));

            distances.Add(new PositionAndDistance(Distance_BetweenVectors(pos_now, new Vector2(room2.start.x, room2.end.y)), pos_now, new Vector2(room2.start.x, room2.end.y)));

            distances.Add(new PositionAndDistance(Distance_BetweenVectors(pos_now, new Vector2(room2.end.x, room2.start.y)), pos_now, new Vector2(room2.end.x, room2.start.y)));
        }

        distances.Sort((s1, s2) => s1.distance.CompareTo(s2.distance));

        DistanceToTheRoom dist = new DistanceToTheRoom();

        dist.room = room2;
        dist.closestDistance = distances[0].distance;

        dist.closestCorner = distances[0].position;
        dist.toOtherRoomClosestCorner = distances[0].closeToThisPosition;

        return dist;
    }

    private static float Distance_BetweenVectors(Vector2 vector1, Vector2 vector2)
    {
        return Mathf.Sqrt(Mathf.Pow(vector1.x - vector2.x,2) + Mathf.Pow(vector1.y - vector2.y, 2));
    }

    public static IEnumerator WaitForEndOfAnimation(Animator anim)
    {
        yield return new WaitForEndOfFrame();
        float time = anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(time);
    }

    public static float Min(float a, float b)
    {
        if (a < b)
        {
            return a;
        }
        return b;
    }
    public static float Max(float a, float b)
    {
        if (a > b)
        {
            return a;
        }
        return b;
    }
    public static int Min(int a, int b)
    {
        if (a < b)
        {
            return a;
        }
        return b;
    }
    public static int Max(int a, int b)
    {
        if (a > b)
        {
            return a;
        }
        return b;
    }

    public static Vector2 LowestVector(Vector2 a, Vector2 b)
    {
        Vector2 result;
        if (a.x < b.x)
        {
            result.x = a.x;
        }
        else
        {
            result.x = b.x;
        }
        if (a.y < b.y)
        {
            result.y = a.y;
        }
        else
        {
            result.y = b.y;
        }
        return result;
    }

    public static Vector2 HighestVector(Vector2 a, Vector2 b)
    {
        Vector2 result;
        if (a.x > b.x)
        {
            result.x = a.x;
        }
        else
        {
            result.x = b.x;
        }
        if (a.y > b.y)
        {
            result.y = a.y;
        }
        else
        {
            result.y = b.y;
        }
        return result;
    }

    public static List<Vector2> GetRectangle(Vector2 lowest, Vector2 highest)
    {
        List<Vector2> rect = new List<Vector2>();

        Vector2 start = LowestVector(lowest, highest);
        Vector2 end = HighestVector(lowest, highest);

        for(int i = (int)start.x; i <= end.x; i++)
        {
            for (int j = (int)start.y; j <= end.y; j++)
            {
                rect.Add(new Vector2(i, j));
            }
        }
        return rect;
    }
}
