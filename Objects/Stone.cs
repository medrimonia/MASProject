using System;
using Mogre;


namespace MASProject.Objects
{
    class Stone : GraphicalObject
    {
        public Stone(SceneManager sm, int stoneId, Vector3 initialLocation) : base()
        {
            string entityName = "Stone" + stoneId;
            string nodeName = "StoneNode" + stoneId;
            ent = sm.CreateEntity(entityName, "BeerBarrel.mesh");
            node = sm.RootSceneNode.CreateChildSceneNode(nodeName, initialLocation);
            node.AttachObject(ent);
            float wishedSize = 50f;
            float ratio = wishedSize / BoundingBox.Size.Length;
            node.SetScale(ratio, ratio, ratio);
        }
    }
}
