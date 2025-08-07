using ObjectBuilders.SafeZone;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI;
using SpaceEngineers.Game.ModAPI;
using System;
using VRage.Game.Components;
using VRage.ModAPI;
using VRage.ObjectBuilders;

namespace Catopia.ProtectionBlock
{
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_SafeZoneBlock), false, "AdminSafeZoneBlock")]
    public class ProtectionBlock : MyGameLogicComponent
    {
        IMySafeZoneBlock block;
        private bool protectionEnabled = true;

        public static Guid ZoneIdsKey = new Guid("4C97F675-CED0-4624-BD67-E2E0C686BDFE");

        public bool Terminal_ExampleToggle
        {
            get
            {
                return protectionEnabled;
            }
            set
            {
                protectionEnabled = value;
                updateProtection();
                block.Storage[ZoneIdsKey] = protectionEnabled.ToString();
            }
        }

        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            Log.Msg("Init...");
            block = Entity as IMySafeZoneBlock;
            block.EnabledChanged += Block_EnabledChanged;

            if (block.Storage == null)
                block.Storage = new MyModStorageComponent();

            NeedsUpdate = MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
        }

        private void Block_EnabledChanged(IMyTerminalBlock obj)
        {
            updateProtection();
        }

        internal void updateProtection()
        {
            if (block.Enabled && protectionEnabled)
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
            TerminalControls.DoOnce(ModContext);

            if (block.CubeGrid?.Physics == null)
                return;

            string storage;
            if (block.Storage.TryGetValue(ZoneIdsKey, out storage))
            {
                try
                {
                    protectionEnabled = Boolean.Parse(storage);
                }
                catch (Exception e)
                {
                    Log.Msg($"Storage e={e}");
                    protectionEnabled = false; ;
                }
            }
        }


        public override void Close()
        {
            block.EnabledChanged -= Block_EnabledChanged;

            ProtectionSession.Instance?.ProtectionBlocks.Remove((IMyFunctionalBlock)Entity);
        }

    }
}
