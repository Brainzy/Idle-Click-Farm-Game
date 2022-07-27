using ScriptableObjectMakingScripts;

namespace BuildingScripts
{
    public partial class BuildingManager
    {
        public interface ITakeActionFromTool
        {
            void RunActionFromTool(NewTool toolData);
        }

        public interface IFinishedToolActionAnimal
        {
            void FinishedAction();
        }

        public interface IClickOnBuilding
        {
            void ClickedOnBuilding();
        }

        public enum StorageLocation
        {
            Barn,
            Silo,
            Compost
        }

        public enum ToolName
        {
            Machete,
            Bucket,
            Basket,
            ChickenFoodBag,
            Paint
        }

        public enum BuildingTag
        {
            Eucalyptus,
            Soy,
            Sugarcane,
            Banana,
            Cocoa,
            MombacaGrass,
            Palmito,
            Barn,
            Silo,
            CompostShed,
            Chicken,
            Cow
        }
        
        public int FindPriceBasedOnName(string itemName)
        {
            for (int i = 0; i < allBuildings.Length; i++)
            {
                if (allBuildings[i].displayName.Equals(itemName))
                {
                    return allBuildings[i].interactionResultSellPrice;
                }
            }
            return 0;
        }
    }
}