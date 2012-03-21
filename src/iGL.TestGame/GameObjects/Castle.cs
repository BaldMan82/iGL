using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine;
using iGL.Engine.Math;

namespace iGL.TestGame.GameObjects
{
    public class Castle : GameObject
    {
        public const int LandSizeX = 17;
        public const int LandSizeY = 5;
        public const int LandSizeZ = 17;

        public const float StoneMass = 10.0f;

        private Cube _floor;
        private Sphere _energySphere;
        private Vector3 _offset;
        private int _bodyCount;

        public struct StonePosition
        {
            public int X;
            public int Y;
            public int Z;

            public Stone Stone;

            public StonePosition(int x, int y, int z, Stone stone)
            {
                X = x;
                Y = y;
                Z = z;
                Stone = stone;
            }

            public override int GetHashCode()
            {
                return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
            }


            public override bool Equals(Object obj)
            {
                return obj is StonePosition && this == (StonePosition)obj;
            }


            public static bool operator ==(StonePosition left, StonePosition right)
            {
                return left.X == right.X && left.Y == right.Y && left.Z == right.Z;
            }
            public static bool operator !=(StonePosition left, StonePosition right)
            {
                return !(left == right);
            }
        }

        public class Stone
        {
            public Cube Cube { get; internal set; }
            public int? BodyId { get; internal set; }
            public StonePosition Position { get; internal set; }

            public enum Type
            {
                Normal
            }
        }

        public Stone[, ,] Stones
        {
            get;
            private set;
        }


        public Castle()
        {
            Stones = new Stone[LandSizeX, LandSizeY, LandSizeZ];
        }

        public void SetDefaultStones()
        {
            for (int j = 0; j < Castle.LandSizeY; j++)
            {
                foreach (int k in new List<int>() { 0, Castle.LandSizeZ - 1 })
                {
                    for (int i = 0; i < Castle.LandSizeX; i++)
                    {
                        if (i == 3 || i == 13) continue;

                        if (j < Castle.LandSizeY - 1 || i % 2 == 0)
                        {
                            Stones[i, j, k] = new Castle.Stone();
                        }
                    }
                }

                foreach (int i in new List<int>() { 0, Castle.LandSizeX - 1 })
                {
                    for (int k = 1; k < Castle.LandSizeZ - 1; k++)
                    {
                        if (k %2 == 0 ) continue;

                        if (j < Castle.LandSizeY - 1 || k % 2 == 0)
                        {
                            Stones[i, j, k] = new Castle.Stone();
                        }
                    }
                }
            }
        }

        public void Build(Vector3 offset)
        {
            //_offset = offset;
            _floor = new Cube() { Scale = new Vector3(LandSizeZ + 10.0f, 1.0f, LandSizeX + 10.0f) };
            _floor.Position = new Vector3(0, -5.2f, 0) + _offset;
            _floor.Rotation = new Vector3(0.5f, 0, 0);

            _floor.Material.Ambient = new Vector4(0.1f, 0.1f, 0.1f, 0.0f);
            _floor.Material.Diffuse = new Vector4(0.5f, 0.5f, 0.5f, 1.0f);

            _floor.AddComponent(new BoxColliderComponent());

            var floorBody = new RigidBodyComponent(isStatic: true);          
            
            _floor.AddComponent(floorBody);

            Scene.AddGameObject(_floor);

            //_energySphere = new Sphere(1.0f);
            //_energySphere.Position = new Vector3(0, 1.0f, 0);
            //_energySphere.Material.Ambient = new Vector4(0.5f, 0.5f, 0.0f, 1.0f);
            //_energySphere.Material.Diffuse = new Vector4(1.0f, 1.0f, 0.0f, 1.0f);

            //AddChild(_energySphere);

            int count = 0;
            for (int i = 0; i < LandSizeX; i++)
            {
                for (int j = 0; j < LandSizeY; j++)
                {
                    for (int k = 0; k < LandSizeZ; k++)
                    {
                        var stone = Stones[i, j, k];
                        if (stone != null)
                        {
                            count++;
                            stone.Cube = new Cube() { Scale = new Vector3(1.0f, 1.0f, 1.0f) };
                            stone.Cube.Material.Ambient = new Vector4(0.1f, 0.1f, 0.1f, 0.0f);
                            stone.Cube.Material.Diffuse = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);
                            stone.Cube.AddComponent(new BoxColliderComponent());
                            stone.Cube.AddComponent(new RigidBodyComponent());

                            var pos = new Vector3(-(LandSizeX / 2.0f) + i + 0.5f, j, -(LandSizeZ / 2.0f) + k + 0.5f);
                            stone.Cube.Position = pos;
                            stone.Position = new StonePosition(i, j, k, stone);

                            //Scene.AddGameObject(stone.Cube);
                        }
                    }
                }
            }
            
            MapBlocks();
        }

        private int GetNewBodyId()
        {
            return _bodyCount++;
        }

        private void MapBlocks()
        {
            List<List<Stone>> bodies = new List<List<Stone>>();

            for (int i = 0; i < LandSizeX; i++)
            {
                for (int j = 0; j < LandSizeY; j++)
                {
                    for (int k = 0; k < LandSizeZ; k++)
                    {
                        var stone = Stones[i, j, k];
                        if (stone != null && !stone.BodyId.HasValue)
                        {
                            var group = AttachToNeightbours(stone);
                            bodies.Add(group);
                        }
                    }
                }
            }

            foreach (var body in bodies)
            {
                var children = new List<GameObject>();

                foreach (var stone in body)
                {
                    children.Add(stone.Cube);
                }

                var compoundObject = new CompoundObject(children, StoneMass * children.Count);
                compoundObject.Position = compoundObject.Position + _offset;
                Scene.AddGameObject(compoundObject);
            }
        }

        private IEnumerable<Stone> GetNeighbours(Stone stone)
        {
            int x = stone.Position.X, y = stone.Position.Y, z = stone.Position.Z;
            /* a stone can have a maximum of 6 neighbours */

            List<Stone> neighBours = new List<Stone>();

            if (x + 1 < LandSizeX) neighBours.Add(Stones[x + 1, y, z]);
            if (x - 1 >= 0) neighBours.Add(Stones[x - 1, y, z]);
            if (y + 1 < LandSizeY) neighBours.Add(Stones[x, y + 1, z]);
            if (y - 1 >= 0) neighBours.Add(Stones[x, y - 1, z]);
            if (z + 1 < LandSizeZ) neighBours.Add(Stones[x, y, z + 1]);
            if (z - 1 >= 0) neighBours.Add(Stones[x, y, z - 1]);

            return neighBours.Where(s => s != null);
        }

        private List<Stone> AttachToNeightbours(Stone stone)
        {
            List<StonePosition> stoneGroup = new List<StonePosition>();
            var bodyId = FindBodyId(stone, ref stoneGroup);

            if (bodyId == null)
            {
                /* no body has been defined, create one */
                int id = GetNewBodyId();

                /* set all neighbours to this id */
                stoneGroup.ForEach(p => p.Stone.BodyId = id);
            }

            return stoneGroup.Select(s => s.Stone).ToList();
        }

        private int? FindBodyId(Stone stone, ref List<StonePosition> stoneGroup)
        {
            if (stoneGroup.Contains(stone.Position)) return null;

            stoneGroup.Add(stone.Position);

            if (stone.BodyId.HasValue) return stone.BodyId;
            
            var checkList = stoneGroup.ToList();

            var neigbours = GetNeighbours(stone);
            var stoneWithBodyId = neigbours.FirstOrDefault(s => s.BodyId.HasValue);

            if (stoneWithBodyId != null) return stoneWithBodyId.BodyId;

            var uncheckedNeighbours = neigbours.Where(s => !checkList.Any(p => p == s.Position));

            foreach (var neighbour in uncheckedNeighbours)
            {
                var bodyId = FindBodyId(neighbour, ref stoneGroup);
                if (bodyId.HasValue) return bodyId;
            }

            return null;
        }
    }
}
