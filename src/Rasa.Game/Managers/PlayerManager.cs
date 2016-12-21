﻿using Rasa.Packets.Game.Server;

namespace Rasa.Managers
{
    using Structures;
    using Packets.MapChannel.Server;

    public class PlayerManager
    {
        public static void AssignPlayer(MapChannel mapChannel, MapChannelClient owner, PlayerData player)
        {
            owner.Client.SendPacket(5, new SetControlledActorIdPacket { EntetyId = owner.Player.Actor.EntityId});

            owner.Client.SendPacket(7, new SetSkyTimePacket {RunningTime = 6666666});   // ToDo add actual time how long map is running

            owner.Client.SendPacket(5, new SetCurrentContextIdPacket {MapContextId = player.Actor.MapContextId});

            owner.Client.SendPacket(owner.Player.Actor.EntityId, new UpdateRegionsPacket { RegionIdList = 0});        // ToDo

            owner.Client.SendPacket(owner.Player.Actor.EntityId, new AllCreditsPacket { Credits = 0, Prestige = 0});  // ToDo

            owner.Client.SendPacket(owner.Player.Actor.EntityId, new AdvancementStatsPacket                           // ToDo
                {
                Level = 1,
                Experience = 0,
                Attributes = 4,
                TrainPts = 0,       // trainPoints (are not used by the client??)
                SkillPts = 4
            } );
            
            owner.Client.SendPacket(owner.Player.Actor.EntityId, new SkillsPacket());        // ToDo

            owner.Client.SendPacket(owner.Player.Actor.EntityId, new AbilitiesPacket());   // ToDo

            owner.Client.SendPacket(owner.Player.Actor.EntityId, new AbilityDrawerPacket {AbilityId = 0, PumpLevel = 0});    //ToDo*/
        }

        public static void CellIntroduceClientToPlayers(MapChannel mapChannel, MapChannelClient client, int playerCount)
        {
            for (var i = 0; i < playerCount; i++)
            {
                client.Client.SendPacket(5, new CreatePyhsicalEntityPacket( (int)client.Player.Actor.EntityId, (int)client.Player.Actor.EntityClassId));
            }

            for (var i = 0; i < playerCount; i++)
            {
                client.Client.SendPacket(client.Player.Actor.EntityId, new AttributeInfoPacket());
            }
            
            // PreloadData
            for (var i = 0; i < playerCount; i++)
            {
                client.Client.SendPacket(client.Player.Actor.EntityId, new PreloadDataPacket());
            }
            // Recv_AppearanceData
            for (var i = 0; i < playerCount; i++)
            {
                client.Client.SendPacket(client.Player.Actor.EntityId, new AppearanceDataPacket());
            }
            // set controller (Recv_ActorControllerInfo )
            for (var i = 0; i < playerCount; i++)
            {
                client.Client.SendPacket(client.Player.Actor.EntityId, new ActorControllerInfoPacket{ IsPlayer = true });
            }
            // set level
            for (var i = 0; i < playerCount; i++)
            {
                client.Client.SendPacket(client.Player.Actor.EntityId, new LevelPacket{ Level = 1 });
            }
            // set class
            for (var i = 0; i < playerCount; i++)
            {
                client.Client.SendPacket(client.Player.Actor.EntityId, new CharacterClassPacket{ CharacterClass = 1 });
            }
            // set charname (name)
            for (var i = 0; i < playerCount; i++)
            {
                client.Client.SendPacket(client.Player.Actor.EntityId, new CharacterNamePacket{ CharacterName = "Krssrb" });
            }
            // set actor name
            for (var i = 0; i < playerCount; i++)
            {
                client.Client.SendPacket(client.Player.Actor.EntityId, new ActorNamePacket{ CharacterFamily = "[GM]" });
            }
            // set running
            for (var i = 0; i < playerCount; i++)
            {
                client.Client.SendPacket(client.Player.Actor.EntityId, new IsRunningPacket{ IsRunning = true });
            }
            // set logos tabula
            for (var i = 0; i < playerCount; i++)
            {
                client.Client.SendPacket(client.Player.Actor.EntityId, new LogosStoneTabulaPacket());
            }
            // Recv_Abilities (id: 10, desc: must only be sent for the local manifestation)
            // We dont need to send ability data to every client, but only the owner (which is done in manifestation_assignPlayer)
            // Skills -> Everything that the player can learn via the skills menu (Sprint, Firearms...) Abilities -> Every skill gained by logos?
            // Recv_WorldLocationDescriptor
            for (var i = 0; i < playerCount; i++)
            {
                client.Client.SendPacket(client.Player.Actor.EntityId, new WorldLocationDescriptorPacket
                {
                    PosX = client.Player.Actor.PosX,
                    PosY = client.Player.Actor.PosY,
                    PosZ = client.Player.Actor.PosZ,
                    RotationX = 0.0f,
                    RotationY = 0.0f,
                    RotationZ = 0.0f,
                    Unknwon = 1.0f          // Camera poss?
                });
            }
            // set target category
            for (var i = 0; i < playerCount; i++)
            {
                client.Client.SendPacket(client.Player.Actor.EntityId, new TargetCategoryPacket { TargetCategory = 0 });    // 0 frendly
            }
            // player flags
            for (var i = 0; i < playerCount; i++)
            {
                client.Client.SendPacket(client.Player.Actor.EntityId, new PlayerFlagsPacket());
            }
            // send inital movement packet
            //netCompressedMovement_t netMovement = { 0 };
            //netMovement.entityId = client->player->actor->entityId;
            //netMovement.posX24b = client->player->actor->posX * 256.0f;
            //netMovement.posY24b = client->player->actor->posY * 256.0f;
            //netMovement.posZ24b = client->player->actor->posZ * 256.0f;
            //for (sint32 i = 0; i < playerCount; i++)
            //{
            //    netMgr_sendEntityMovement(playerList[i]->cgm, &netMovement);
            //}*/
        }

        public static void CellIntroducePlayersToClient(MapChannel mapChannel, MapChannelClient client, int playerCount)
        {
            for (var i = 0; i < playerCount; i++)
            {
                if (mapChannel.PlayerList[i].ClientEntityId == client.ClientEntityId)
                    continue;

                var tempClient = mapChannel.PlayerList[i];

                client.Client.SendPacket(5, new CreatePyhsicalEntityPacket((int)tempClient.Player.Actor.EntityId, (int)tempClient.Player.Actor.EntityClassId));

                client.Client.SendPacket(tempClient.Player.Actor.EntityId, new AttributeInfoPacket());
                // doesnt seem important (its only for loading gfx early?)
                //PreloadData
                //client.Client.SendPacket(tempClient.Player.Actor.EntityId, new PreloadDataPacket());
                // Recv_AppearanceData
                client.Client.SendPacket(tempClient.Player.Actor.EntityId, new AppearanceDataPacket());
                // set controller
                client.Client.SendPacket(tempClient.Player.Actor.EntityId, new ActorControllerInfoPacket{IsPlayer = true});
                // set level
                client.Client.SendPacket(tempClient.Player.Actor.EntityId, new LevelPacket{ Level = 1 });
                // set class
                client.Client.SendPacket(tempClient.Player.Actor.EntityId, new CharacterClassPacket{ CharacterClass = 1 });
                // set charname (name)
                client.Client.SendPacket(tempClient.Player.Actor.EntityId, new CharacterNamePacket{ CharacterName = "Krssrb" });
                // set actor name (familyName)
                client.Client.SendPacket(tempClient.Player.Actor.EntityId, new ActorNamePacket{ CharacterFamily = "[GM]" });
                // set running
                client.Client.SendPacket(tempClient.Player.Actor.EntityId, new IsRunningPacket{ IsRunning = true });
                // Recv_WorldLocationDescriptor
                client.Client.SendPacket(tempClient.Player.Actor.EntityId, new WorldLocationDescriptorPacket
                {
                    PosX = tempClient.Player.Actor.PosX,
                    PosY = tempClient.Player.Actor.PosY,
                    PosZ = tempClient.Player.Actor.PosZ,
                    RotationX = 0.0f,
                    RotationY = 0.0f,
                    RotationZ = 0.0f,
                    Unknwon = 1.0f          // Camera poss?
                });
                // set target category
                client.Client.SendPacket(tempClient.Player.Actor.EntityId, new TargetCategoryPacket{ TargetCategory = 0 });
                // send inital movement packet
                //netCompressedMovement_t netMovement = { 0 };
                //netMovement.entityId = tempClient->player->actor->entityId;
                //netMovement.posX24b = tempClient->player->actor->posX * 256.0f;
                //netMovement.posY24b = tempClient->player->actor->posY * 256.0f;
                //netMovement.posZ24b = tempClient->player->actor->posZ * 256.0f;
                //netMgr_sendEntityMovement(client->cgm, &netMovement);
            }
        }
    }
}
