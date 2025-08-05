using ObjectBuilders.SafeZone;
using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI;
using SpaceEngineers.Game.ModAPI;
using System;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.ObjectBuilders;

namespace Catopia.ProtectionBlock
{
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_SafeZoneBlock), false, "AdminSafeZoneBlock")]
    public class ProtectionBlock : MyGameLogicComponent
    {
        IMySafeZoneBlock block;

        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            block = Entity as IMySafeZoneBlock;
            block.EnabledChanged += Block_EnabledChanged;
            
            NeedsUpdate = MyEntityUpdateEnum.BEFORE_NEXT_FRAME;

        }

        private void Block_EnabledChanged(IMyTerminalBlock obj)
        {           
            if (block.Enabled)
            {
                ProtectionSession.Instance?.ProtectionBlocks.Add(block);
            }
            else
            {
                ProtectionSession.Instance?.ProtectionBlocks.Remove(block);
            }
        }

        public override void UpdateOnceBeforeFrame()
        {
            if (block.CubeGrid?.Physics == null)
                return;
            NeedsUpdate = MyEntityUpdateEnum.EACH_100TH_FRAME;
        }

        public override void UpdateBeforeSimulation100()
        {
            base.UpdateBeforeSimulation100();
          
            if (block.IsSafeZoneEnabled())
            {
                ProtectionSession.Instance?.ProtectionBlocks.Add(block);
            } else
            {
                ProtectionSession.Instance?.ProtectionBlocks.Remove(block);
            }
        }

        public override void Close() 
        {
            ProtectionSession.Instance?.ProtectionBlocks.Remove((IMyFunctionalBlock)Entity);
        }
    }
}
