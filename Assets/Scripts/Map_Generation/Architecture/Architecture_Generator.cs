using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public static class Architecture_Generator {

    enum ArchitectureType
    {
        CutCorners,
        Corners,
        Columns,
        SmallRooms,
        Cuts,
        Pools
    }

    class ArchitectureFactory
    {
        public Architecture CreateArchitecture(ArchitectureType type)
        {
            switch (type)
            {
                case ArchitectureType.CutCorners:
                    return new CutCorners(); 
                case ArchitectureType.Corners:
                    return new Corners();
                case ArchitectureType.Columns:
                    return new Columns();
                case ArchitectureType.SmallRooms:
                    return new SmallRooms();
                default:
                    return new SmallRooms();
            }
        }
    }

    // Use this for initialization
    public static IEnumerator Generate(System.Random rand)
    { 
        ArchitectureFactory architectureFactory = new ArchitectureFactory();

        Architecture.direction dir;

        foreach (Room room in Map.roomObjs)
        {
            if (room.roomType == roomType.common)
            {
                yield return new WaitForSecondsRealtime(0.25f);
                for (int i = 0; i < 3; i++)
                {
                    ArchitectureType type = (ArchitectureType)rand.Next(0, 4);

                    Architecture arch = architectureFactory.CreateArchitecture(type);
                    arch.randomGen = rand;

                    dir = arch.GetRandom();

                    arch.Create(room, dir);
                }

                //int createPool = Random.Range(0, 6);//20%

                //if (createPool == 0)
                //{
                //    Pools pool = ScriptableObject.CreateInstance("Pools") as Pools;

                //    dir = pool.GetRandom();

                //    pool.Create(room, dir);
                //}

                int createCuts = rand.Next(0, 2);//50%
                if (createCuts == 0)
                {
                    Cuts cuts = new Cuts();
                    cuts.randomGen = rand;

                    dir = cuts.GetRandom();

                    cuts.Create(room, dir);
                }

            }
        }
        yield return 0;

    }

}
