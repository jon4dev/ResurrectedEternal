using RRFull.BaseObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RRWAPI;
//using MemoryShare;
using RRFull.ClientObjects;
using RRFull.Events;

namespace RRFull.Skills
{
    class SkillMod
    {
        //public Operandi Operandi = Operandi.None;


        public Modus ClientModus => StateMachine.ClientModus;

        public Config Config => g_Globals.Config;

        public Client Client;
        public Engine Engine;

        //public LocalPlayer _localPlayer { private set; get; }
        //public List<BaseEntity> _playerList { private set; get; }

        //public BaseEntity PlantedBomb;
        //public BaseEntity C4;

        public SkillMod(Engine engine, Client client)
        {
            Client = client;
            Engine = engine;
            //_startValue = (bool)Config.ExtraConfig.FOVChanger.Value;
        }

        public virtual void Start()
        {

        }
        public virtual void Before()
        {

        }

        public virtual bool Update()
        {
            return true;
        }

        public virtual void AfterUpdate()
        {

        }

        public virtual void End()
        {

        }
    }
}
