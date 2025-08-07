using Sandbox.Game.Localization;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces.Terminal;
using SpaceEngineers.Game.ModAPI;
using VRage.Game.ModAPI;
using VRage.Utils;

namespace Catopia.ProtectionBlock
{
    public static class TerminalControls
    {
        const string IdPrefix = "Catopia_Prot_";
        static bool Done = false;

        public static void DoOnce(IMyModContext context) // called by GyroLogic.cs
        {
            if (Done)
                return;
            Done = true;

            CreateControls();
        }

        static bool CustomVisibleCondition(IMyTerminalBlock b)
        {
            return b?.GameLogic?.GetAs<ProtectionBlock>() != null;
        }

        static void CreateControls()
        {
            {
                var c = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlSeparator, IMySafeZoneBlock>(""); // separators don't store the id
                c.SupportsMultipleBlocks = true;
                c.Visible = CustomVisibleCondition;

                MyAPIGateway.TerminalControls.AddControl<IMySafeZoneBlock>(c);
            }
            {
                var c = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlOnOffSwitch, IMySafeZoneBlock>(IdPrefix + "ProtectionOnOff");
                c.Title = MyStringId.GetOrCompute("Grid Protection");
                c.Tooltip = MyStringId.GetOrCompute("Enable protection for grid");
                c.SupportsMultipleBlocks = true;
                c.Visible = CustomVisibleCondition;

                c.OnText = MySpaceTexts.SwitchText_On;
                c.OffText = MySpaceTexts.SwitchText_Off; ;

                c.Getter = (b) => b?.GameLogic?.GetAs<ProtectionBlock>()?.Terminal_ExampleToggle ?? false;
                c.Setter = (b, v) =>
                {
                    var logic = b?.GameLogic?.GetAs<ProtectionBlock>();
                    if (logic != null)
                        logic.Terminal_ExampleToggle = v;
                };

                MyAPIGateway.TerminalControls.AddControl<IMySafeZoneBlock>(c);
            }

        }
    }
}
