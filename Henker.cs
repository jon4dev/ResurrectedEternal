
using RRFull.ClientObjects;
using RRFull.ClientObjects.Cvars;
using RRFull.Clockwork;
using RRFull.Configs;
using RRFull.Configs.ConfigSystem;
using RRFull.Events;
using RRFull.GenericObjects;
using RRFull.Memory;
using RRFull.Params.CSHelper;
using RRFull.Skills;
using RRFull.Skills.EnvironmentSkillMods;
using RRFull.Skills.GamePlaySkillMods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace RRFull
{
    class Henker
    {
        //public Menu.Menu Menu;
        public static Henker Singleton;
        //public static RPCMemSlave RPC = new RPCMemSlave();
        //public Offsets Offsets = new Offsets();
        public MemoryLoader Memory;
        public Engine Engine;
        public Client Client;

        public ConvarManager ConvarManager;

        //public Config Config = new Config();

        public long _currentFPS = 0;
        private long _renderedFrames = 0;
        private DateTime _previousFrameUpdate = DateTime.Now;
        private DateTime _previousDeltaUpdate = DateTime.Now;

        private Thread _entityUpdateThread;
        private List<SkillMod> _skillMods = new List<SkillMod>();


        private bool _processActive = false;
        public string[] GetModuleNames()
        {
            List<string> _names = new List<string>();
            foreach (var item in _skillMods)
            {
                _names.Add(string.Format("Module {0} - Standalone", item.ToString().Substring(item.ToString().LastIndexOf('.'))));
            }
            return _names.ToArray();
        }

        private Timer _T;
        public Henker()
        {
            ConsoleHelper.Write("[Resurrected SoL F8 Framework]\n", ConsoleColor.Blue);
            g_Globals.Offset = Offsets.Load();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
            Clock.Initialize();

            if (!System.IO.File.Exists(g_Globals.NickConfig))
                Serializer.SaveJson(ConsoleHelper.ColorCaptureInput("Enter Nickname"), g_Globals.NickConfig);

            g_Globals.Nickname = Serializer.LoadJson<string>(g_Globals.NickConfig);

            ConsoleHelper.ShowAction("User Found! ", 33);
            ConsoleHelper.ConfirmAction("Welcome " + g_Globals.Nickname + "! =)");





            EventManager.OnPanic += EventManager_OnPanic;

            //we might aswell call Start in here and leave it private.
            Singleton = this;
            //Configs.ConfigFactory.CreateNewConfig();
            Start();

        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
           Console.WriteLine(e.ToString());
        }

        private void CurrentDomain_FirstChanceException(object sender, FirstChanceExceptionEventArgs e)
        {
            Console.WriteLine(e.Exception.ToString());
        }

        private Modus previousMode;
        private void EventManager_OnPanic(bool obj)
        {
            if (obj)
            {
                previousMode = StateMachine.ClientModus;
                StateMachine.ClientModus = Modus.leaguemode;
                return;
            }

            StateMachine.ClientModus = previousMode;


        }

        private void Start()
        {
            Memory = new MemoryLoader("csgo");
            Memory.OnProcessLoaded += Memory_OnProcessLoaded;
            Memory.OnProcessExited += Memory_OnProcessExited;
            Memory.Query();

        }

        private void Memory_OnProcessExited()
        {
            Environment.Exit(0);
        }

        private ParamManager paramManager;


        private void Memory_OnProcessLoaded()
        {



            //var _opt = RPC.Request(111);
            //if (!System.IO.File.Exists(g_Globals.Offsets))
            //    Environment.FailFast("Could not load offsets");
            
            ConfigFactory.TryLoadConfig();
            ScanOffsets();
            Walk();
            if (g_Globals.Offset.dwViewMatrix == 0
                || g_Globals.Offset.dwEntityList == 0
                || g_Globals.Offset.dwGameRulesProxy == 0
                || g_Globals.Offset.dwGlowObjectManager == 0
                || g_Globals.Offset.dwRadarBase == 0
                || g_Globals.Offset.dwForceJump == 0
                || g_Globals.Offset.dwForceAttack == 0)
                ConsoleHelper.ConfirmAction("Couldnt catch all Unicorns!\n Starting anyway...");
            ConsoleHelper.ConfirmAction("OK!");

            Engine = new Engine(Memory.Modules["engine.dll"], (uint)g_Globals.Offset.dwClientState);
            Client = new Client(Memory.Modules["client.dll"], (uint)g_Globals.Offset.dwEntityList, Engine);
            ConvarManager = new ConvarManager();
            _processActive = true;
            paramManager = new ParamManager();
            paramManager.ParseParams();
            Create();
            _entityUpdateThread = new Thread(EntityUpdate);
            _entityUpdateThread.Name = "#" + Generators.GetRandomString(10);
            _entityUpdateThread.SetApartmentState(ApartmentState.STA);
            _entityUpdateThread.Start();
            paramManager.Hook();
        }

        private void ScanOffsets()
        {
            ConsoleHelper.ShowAction("Scanning...", 33);

            //var _getOffsetPatterns = RPC.Request(112);

            MemoryManager.PatMod.ModulePattern[] _dmp = Serializer.LoadJson<MemoryManager.PatMod.ModulePattern[]>(g_Globals.Signatures);

            foreach (var item in _dmp)
            {
                if (Memory.Reader.DumpModule(Memory.ProcessModules[item.ModuleName]))
                {
                    foreach (var _off in item.Patterns)
                    {
                        ApplyOffset(_off.Name, Read(_off));
                    }
                }
            }

        }

        private int Read(MemoryManager.PatMod.SerialPattern _pattern)
        {
            if(_pattern.Name == "m_dwGetAllClasses")
            {
                var _address = Memory.Reader.GetAllClasses(_pattern.Pattern, RedRain.ScanFlags.SUBSTRACT_BASE, new int[] { 1, 0 }, _pattern.Extra);
                return _address;
            }
            if (_pattern.SubtractOnly)
                return Memory.Reader.FindPattern(_pattern.Pattern, RedRain.ScanFlags.SUBSTRACT_BASE, _pattern.Offset, _pattern.Extra);
            if (_pattern.Relative)
                return Memory.Reader.FindPattern(_pattern.Pattern, RedRain.ScanFlags.READ | RedRain.ScanFlags.SUBSTRACT_BASE, _pattern.Offset, _pattern.Extra);
            return Memory.Reader.FindPattern(_pattern.Pattern, RedRain.ScanFlags.READ, _pattern.Offset, _pattern.Extra);
        }

        private void ApplyOffset(string name, int offset)
        {
            var _ok = g_Globals.Offset.GetType();
            FieldInfo propertyInfo = g_Globals.Offset.GetType().GetField(name);
            propertyInfo.SetValue(g_Globals.Offset, offset);
        }

        private void Create()
        {
            _skillMods.Add(new SkillModSoundEffects(Engine, Client));
            _skillMods.Add(new SkillModVisible(Engine, Client));
            _skillMods.Add(new SkillModAim(Engine, Client));
            _skillMods.Add(new SkillModNeon(Engine, Client));
            _skillMods.Add(new SkillModHBTrigger(Engine, Client));
            _skillMods.Add(new SkillModDrawing(Engine, Client));
            _skillMods.Add(new SkillModBunny(Engine, Client));
            _skillMods.Add(new SkillModConvar(Engine, Client));
            _skillMods.Add(new SkillModCham(Engine, Client));
            _skillMods.Add(new SkillModSkin(Engine, Client));
            _skillMods.Add(new SkillModEnvironmentControl(Engine, Client));
            _skillMods.Add(new SkillModSpriteController(Engine, Client));
        }

        private void EntityUpdate()
        {


            while (!Memory.Reader.IsDisposed)
            {
                if (Memory.Reader.IsDisposed)
                    break;

                try
                {
                    foreach (var item in _skillMods)
                    {
                        item.Start();
                    }
                    var _updateResult = Client.Update();
                    switch (_updateResult)
                    {
                        case UpdateResult.OK:
                            RunModules();
                            break;
                        case UpdateResult.NewState:
                        //Thread.Sleep(100);
                        //break;
                        case UpdateResult.LevelChanged:
                        case UpdateResult.Pending:
                        case UpdateResult.None:
                        default:
                            Thread.Sleep(1);
                            break;
                    }
                    foreach (var item in _skillMods)
                    {
                        item.End();
                    }

                    _previousDeltaUpdate = DateTime.Now;
                    CalculateFramesPerSecond();

                }
                catch (Exception e)
                {
                    //throw e;
                    //Program.Log(e.ToString());
                    Client.Dirty();
                }

            }
            //Console.WriteLine("Process Exited?");
            Environment.Exit(0);
        }

        private void RunModules()
        {
            foreach (var item in _skillMods)
                item.Before();

            foreach (var item in _skillMods)
                item.Update();

            foreach (var item in _skillMods)
                item.AfterUpdate();
        }

        private void CalculateFramesPerSecond()
        {
            _renderedFrames++;
            if ((DateTime.Now - _previousFrameUpdate).TotalSeconds >= 1)
            {
                _currentFPS = _renderedFrames;
                _renderedFrames = 0;
                _previousFrameUpdate = DateTime.Now;
            }
        }

        public void Walk()
        {
            //dont deref the pointer for all classes
            var _firstclass = Memory.Modules["client.dll"] + g_Globals.Offset.m_dwGetAllClasses;
            //0xDB601C
            //0x3AFDFA
            List<ClientClass_t> _classes = new List<ClientClass_t>();

            do
            {
                var _n = Memory.Reader.Read<ClientClass_t>(_firstclass);
                var _recvTable = Memory.Reader.Read<RecvTable>(_n.m_pRecvTable);
                ReadTableEx(_recvTable);

                _firstclass = _n.m_pNext; // Memory.Reader.Read<IntPtr>();
            } while (_firstclass != IntPtr.Zero);

            var s = new StringBuilder();
            foreach (var item in g_Globals.NetVars)
            {
                s.AppendLine(item.Key + " : " + "0x" + item.Value.ToString("x").ToUpper());
            }
            System.IO.File.WriteAllText("offsets.txt", s.ToString());

        }

        private Dictionary<ClientClass_t, DataTable> Tables = new Dictionary<ClientClass_t, DataTable>();


        //if we have a proxy table we can reiterate the props by getting the others.
        private void ReadTableEx(RecvTable _table)
        {
            var _tblname = Memory.Reader.ReadString(_table.m_pNetTableName, Encoding.UTF8);
            //_table.
            //List<RecvProp> _props = new List<RecvProp>();
            //RecvProp[] _props = new RecvProp[_table.m_nProps];
            for (int i = 0; i < _table.m_nProps; i++)
            {
                //if (!_tblname.StartsWith("DT_")) continue;
                var _prop = Memory.Reader.Read<RecvProp>(new IntPtr((int)_table.m_pProps + (i * 0x3C)));
                var _name = Memory.Reader.ReadString(new IntPtr(_prop.m_pVarName), Encoding.UTF8);

                //Console.WriteLine(_name);


                //_props.Add(_prop);
                //_props[i] 
                if (_prop.m_RecvType == ePropType.DataTable)
                    ReadTableEx(Memory.Reader.Read<RecvTable>((IntPtr)_prop.m_pDataTable));
                if (_prop.m_Offset == 0)
                    continue;
                if (!g_Globals.NetVars.ContainsKey(_tblname + "::" + _name))
                {
                    g_Globals.NetVars.Add(_tblname + "::" + _name, _prop.m_Offset);
                }
            }
        }
        private RecvProp[] ReadTable(RecvTable _table)
        {
            var _tblname = Memory.Reader.ReadString(_table.m_pNetTableName, Encoding.UTF8);
            //_table.
            List<RecvProp> _props = new List<RecvProp>();
            //RecvProp[] _props = new RecvProp[_table.m_nProps];
            for (int i = 0; i < _table.m_nProps; i++)
            {
                var _prop = Memory.Reader.Read<RecvProp>(new IntPtr((int)_table.m_pProps + (i * 0x3C)));
                var _name = Memory.Reader.ReadString(new IntPtr(_prop.m_pVarName), Encoding.UTF8);

                Console.WriteLine(_name);

                if (_prop.m_Offset == 0)
                    continue;
                if (!g_Globals.NetVars.ContainsKey(_tblname + "::" + _name))
                {
                    g_Globals.NetVars.Add(_tblname + "::" + _name, _prop.m_Offset);
                }
                _props.Add(_prop);
                //_props[i] 
                if (_prop.m_RecvType == ePropType.DataTable)
                    _props.AddRange(ReadTable(Memory.Reader.Read<RecvTable>((IntPtr)_prop.m_pDataTable)));

            }
            return _props.ToArray();
        }
        //private Dictionary<string, Dictionary<string, int>> _omg = new Dictionary<string, Dictionary<string, int>>();
    }

    public class DataTable
    {
        public ClientClass_t ClientClass;
        public RecvTable Table;
        public List<RecvProp> Props;
    }
}
