using CHAL.Core;
using CHAL.Data;
using CHAL.Systems.Inventory;
using CHAL.Systems.Items;

namespace CHAL.Systems.Hero
{

    public static class HeroLoadoutService
    {
        private static string BuildHeroGearInstanceId(string heroId)
            => $"hero:{heroId}:gear";

        private static string BuildHeroSocketsInstanceId(string heroId)
            => $"hero:{heroId}:sockets";

        // --------------------------------------------------------------------
        // GEAR: EQUIP
        // --------------------------------------------------------------------


        public static bool TryEquipGear(
            InventoryDomain inventory,
            string heroId,
            string fromInstanceId,
            int fromSlotIndex,
            int heroGearSlotIndex,
            out string failReason)
        {
            failReason = null;

            if (inventory == null)
            {
                failReason = "NoDomain";
                return false;
            }

            if (string.IsNullOrWhiteSpace(heroId))
            {
                failReason = "HeroIdEmpty";
                return false;
            }

            if (string.IsNullOrWhiteSpace(fromInstanceId))
            {
                failReason = "SourceInstanceEmpty";
                return false;
            }

            var sourceStackOpt = inventory.Peek(fromInstanceId, fromSlotIndex);
            if (!sourceStackOpt.HasValue || sourceStackOpt.Value.count <= 0 || sourceStackOpt.Value.IsEmpty)
            {
                failReason = "SourceSlotEmpty";
                return false;
            }

            var sourceStack = sourceStackOpt.Value;
            var sourceType = ItemTypeUtils.FromId(sourceStack.itemID);
            if (sourceType != ItemType.Gear)
            {
                failReason = "SourceNotGear";
                return false;
            }

            var heroGearInstanceId = BuildHeroGearInstanceId(heroId);

            var gm = GameManager.Instance;

            // Sicherstellen, dass das Hero-Gear-Inventar existiert
            if (!inventory.HasInstance(heroGearInstanceId))
            {
                gm.EnsureInstance(heroGearInstanceId, PlayerInventoryType.HeroGear);
            }

            var heroGearInstance = inventory.GetInstance(heroGearInstanceId);
            if (heroGearInstance == null || heroGearInstance.slots == null)
            {
                failReason = "HeroGearInstanceMissing";
                return false;
            }

            if (heroGearSlotIndex < 0 || heroGearSlotIndex >= heroGearInstance.slots.Length)
            {
                failReason = "HeroGearSlotOutOfRange";
                return false;
            }

            var playerGearInstanceId = gm.InstanceIdFor(PlayerInventoryType.Gear);

            // Prüfen, ob im Zielslot bereits Gear liegt (Replace-Case)
            var destStackOpt = inventory.Peek(heroGearInstanceId, heroGearSlotIndex);
            if (destStackOpt.HasValue && destStackOpt.Value.count > 0 && !destStackOpt.Value.IsEmpty)
            {
                var existing = destStackOpt.Value;

                // Ziel für Replace ist immer das Player-Gear-Inventar
                if (!inventory.CanAccept(playerGearInstanceId, existing))
                {
                    failReason = "PlayerGearFull";
                    return false;
                }

                // 1) Hero-Slot leeren
                if (!inventory.TrySetSlot(heroGearInstanceId, heroGearSlotIndex, null))
                {
                    failReason = "FailedClearHeroGearSlot";
                    return false;
                }

                // 2) Altes Gear in Player-Gear packen
                if (!inventory.TryAdd(playerGearInstanceId, existing, out var addResult) || !addResult.success)
                {
                    // Best-Effort-Rollback: altes Gear zurück in Hero-Slot
                    inventory.TryAdd(heroGearInstanceId, existing, out _);
                    failReason = "FailedMoveReplacedGearToPlayer";
                    return false;
                }
            }

            // Jetzt ist der Hero-Gear-Slot frei → normaler Move vom Quell-Inventar
            var req = new MoveRequest
            {
                fromInventory = new ItemMoveObject
                {
                    instanceID = fromInstanceId,
                    slot = fromSlotIndex
                },
                toInventory = new ItemMoveObject
                {
                    instanceID = heroGearInstanceId,
                    slot = heroGearSlotIndex
                },
                amount = null,
                moveMode = MoveMode.Move
            };

            var tx = inventory.TryMove(req, out TransactionResult result);
            if (!result.success)
            {
                failReason = result.reason ?? "MoveFailed";
                return false;
            }

            return true;
        }

        // --------------------------------------------------------------------
        // GEAR: UNEQUIP
        // --------------------------------------------------------------------

        public static bool TryUnequipGear(
            InventoryDomain inventory,
            GameManager gm,
            string heroId,
            int heroGearSlotIndex,
            out string failReason)
        {
            failReason = null;

            if (inventory == null || gm == null)
            {
                failReason = "NoDomainOrGameManager";
                return false;
            }

            if (string.IsNullOrWhiteSpace(heroId))
            {
                failReason = "HeroIdEmpty";
                return false;
            }

            var heroGearInstanceId = BuildHeroGearInstanceId(heroId);
            var heroGearInstance = inventory.GetInstance(heroGearInstanceId);
            if (heroGearInstance == null || heroGearInstance.slots == null)
            {
                failReason = "HeroGearInstanceMissing";
                return false;
            }

            if (heroGearSlotIndex < 0 || heroGearSlotIndex >= heroGearInstance.slots.Length)
            {
                failReason = "HeroGearSlotOutOfRange";
                return false;
            }

            var stackOpt = inventory.Peek(heroGearInstanceId, heroGearSlotIndex);
            if (!stackOpt.HasValue || stackOpt.Value.count <= 0 || stackOpt.Value.IsEmpty)
            {
                failReason = "HeroGearSlotEmpty";
                return false;
            }

            var stack = stackOpt.Value;
            var playerGearInstanceId = gm.InstanceIdFor(PlayerInventoryType.Gear);

            if (!inventory.CanAccept(playerGearInstanceId, stack))
            {
                failReason = "PlayerGearFull";
                return false;
            }

            // 1) Hero-Slot leeren
            if (!inventory.TrySetSlot(heroGearInstanceId, heroGearSlotIndex, null))
            {
                failReason = "FailedClearHeroGearSlot";
                return false;
            }

            // 2) Gear in Player-Gear adden
            if (!inventory.TryAdd(playerGearInstanceId, stack, out var addResult) || !addResult.success)
            {
                // Best-Effort-Rollback
                inventory.TryAdd(heroGearInstanceId, stack, out _);
                failReason = "FailedMoveGearToPlayer";
                return false;
            }

            return true;
        }

        // --------------------------------------------------------------------
        // MODULE: SOCKET
        // --------------------------------------------------------------------

        public static bool TrySocketModule(
            InventoryDomain inventory,
            GameManager gm,
            string heroId,
            string fromInstanceId,
            int fromSlotIndex,
            int heroSocketSlotIndex,
            out string failReason)
        {
            failReason = null;

            if (inventory == null || gm == null)
            {
                failReason = "NoDomainOrGameManager";
                return false;
            }

            if (string.IsNullOrWhiteSpace(heroId))
            {
                failReason = "HeroIdEmpty";
                return false;
            }

            if (string.IsNullOrWhiteSpace(fromInstanceId))
            {
                failReason = "SourceInstanceEmpty";
                return false;
            }

            var sourceStackOpt = inventory.Peek(fromInstanceId, fromSlotIndex);
            if (!sourceStackOpt.HasValue || sourceStackOpt.Value.count <= 0 || sourceStackOpt.Value.IsEmpty)
            {
                failReason = "SourceSlotEmpty";
                return false;
            }

            var sourceStack = sourceStackOpt.Value;
            var sourceType = ItemTypeUtils.FromId(sourceStack.itemID);
            if (sourceType != ItemType.Module)
            {
                failReason = "SourceNotModule";
                return false;
            }

            var heroSocketsInstanceId = BuildHeroSocketsInstanceId(heroId);

            // Sicherstellen, dass das Hero-Socket-Inventar existiert
            if (!inventory.HasInstance(heroSocketsInstanceId))
            {
                gm.EnsureInstance(heroSocketsInstanceId, PlayerInventoryType.HeroSockets);
            }

            var heroSocketInstance = inventory.GetInstance(heroSocketsInstanceId);
            if (heroSocketInstance == null || heroSocketInstance.slots == null)
            {
                failReason = "HeroSocketsInstanceMissing";
                return false;
            }

            if (heroSocketSlotIndex < 0 || heroSocketSlotIndex >= heroSocketInstance.slots.Length)
            {
                failReason = "HeroSocketSlotOutOfRange";
                return false;
            }

            var playerModuleInstanceId = gm.InstanceIdFor(PlayerInventoryType.Module);

            // Replace-Case: im Ziel-Socket liegt bereits ein Modul
            var destStackOpt = inventory.Peek(heroSocketsInstanceId, heroSocketSlotIndex);
            if (destStackOpt.HasValue && destStackOpt.Value.count > 0 && !destStackOpt.Value.IsEmpty)
            {
                var existing = destStackOpt.Value;

                if (!inventory.CanAccept(playerModuleInstanceId, existing))
                {
                    failReason = "PlayerModuleInvFull";
                    return false;
                }

                // 1) Socket leeren
                if (!inventory.TrySetSlot(heroSocketsInstanceId, heroSocketSlotIndex, null))
                {
                    failReason = "FailedClearHeroSocket";
                    return false;
                }

                // 2) Altes Modul ins Player-Module-Inventar verschieben
                if (!inventory.TryAdd(playerModuleInstanceId, existing, out var addResult) || !addResult.success)
                {
                    inventory.TryAdd(heroSocketsInstanceId, existing, out _);
                    failReason = "FailedMoveReplacedModuleToPlayer";
                    return false;
                }
            }

            // Jetzt ist der Socket frei → Move vom Quell-Inventar
            var req = new MoveRequest
            {
                fromInventory = new ItemMoveObject
                {
                    instanceID = fromInstanceId,
                    slot = fromSlotIndex
                },
                toInventory = new ItemMoveObject
                {
                    instanceID = heroSocketsInstanceId,
                    slot = heroSocketSlotIndex
                },
                amount = null,
                moveMode = MoveMode.Move
            };

            TransactionResult result;
            var tx = inventory.TryMove(req, out result);
            if (!result.success)
            {
                failReason = result.reason ?? "MoveFailed";
                return false;
            }

            return true;
        }

        // --------------------------------------------------------------------
        // MODULE: UNSOCKET
        // --------------------------------------------------------------------

        public static bool TryUnsocketModule(
            InventoryDomain inventory,
            GameManager gm,
            string heroId,
            int heroSocketSlotIndex,
            out string failReason)
        {
            failReason = null;

            if (inventory == null || gm == null)
            {
                failReason = "NoDomainOrGameManager";
                return false;
            }

            if (string.IsNullOrWhiteSpace(heroId))
            {
                failReason = "HeroIdEmpty";
                return false;
            }

            var heroSocketsInstanceId = BuildHeroSocketsInstanceId(heroId);
            var heroSocketInstance = inventory.GetInstance(heroSocketsInstanceId);
            if (heroSocketInstance == null || heroSocketInstance.slots == null)
            {
                failReason = "HeroSocketsInstanceMissing";
                return false;
            }

            if (heroSocketSlotIndex < 0 || heroSocketSlotIndex >= heroSocketInstance.slots.Length)
            {
                failReason = "HeroSocketSlotOutOfRange";
                return false;
            }

            var stackOpt = inventory.Peek(heroSocketsInstanceId, heroSocketSlotIndex);
            if (!stackOpt.HasValue || stackOpt.Value.count <= 0 || stackOpt.Value.IsEmpty)
            {
                failReason = "HeroSocketEmpty";
                return false;
            }

            var stack = stackOpt.Value;
            var playerModuleInstanceId = gm.InstanceIdFor(PlayerInventoryType.Module);

            if (!inventory.CanAccept(playerModuleInstanceId, stack))
            {
                failReason = "PlayerModuleInvFull";
                return false;
            }

            // 1) Socket leeren
            if (!inventory.TrySetSlot(heroSocketsInstanceId, heroSocketSlotIndex, null))
            {
                failReason = "FailedClearHeroSocket";
                return false;
            }

            // 2) Modul ins Player-Module-Inventar legen
            if (!inventory.TryAdd(playerModuleInstanceId, stack, out var addResult) || !addResult.success)
            {
                inventory.TryAdd(heroSocketsInstanceId, stack, out _);
                failReason = "FailedMoveModuleToPlayer";
                return false;
            }

            return true;
        }
    }
}
