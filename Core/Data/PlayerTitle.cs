using System.Collections.Generic;

namespace YongAnFrame.Core.Data
{
    public class PlayerTitle
    {
        public uint Id { get; private set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public bool Pro { get; set; }
        public List<string[]> DynamicCommand { get; set; }

        public PlayerTitle(uint id, string name, string color, bool pro, string dynamicCommandString)
        {
            Id = id;
            Name = name;
            Color = color;
            Pro = pro;
            SetDynamicCommand(dynamicCommandString);
        }

        public void SetDynamicCommand(string dynamicCommandString)
        {
            List<string[]> dynamicCommands = null;
            if (!string.IsNullOrEmpty(dynamicCommandString))
            {
                dynamicCommands = [];
                foreach (string dCommand in dynamicCommandString.Split(';'))
                {
                    dynamicCommands.Add(dCommand.Split(','));
                }
            }
            DynamicCommand = dynamicCommands;
        }
    }
}
