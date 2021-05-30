using RRFull.BaseObjects;
using RRFull.BSPParse;
using RRFull.Events.EventArgs;
using RRFull.Memory;
using System.Collections.Generic;

namespace RRFull.ClientObjects
{
    class MapManager
    {
        public bool VisibleCheckAvailable => Maps.ContainsKey(_currentMap) && Maps[_currentMap] != null;
        public BSPFile m_dwMap => Maps[_currentMap];
        private Dictionary<string, BSPFile> Maps = new Dictionary<string, BSPFile>();

        private string _currentMap = "";

        private MemoryLoader MemoryLoader;

        private Client Client;

        //public event Action<string> OnMapChanged;

        public MapManager(Client _c)
        {
            Client = _c;
            MemoryLoader = MemoryLoader.instance;
        }

        private bool _isBusyLoading = false;
        public void Update()
        {

            if (string.IsNullOrEmpty(g_Globals.MapName))
                return;

            var _nextMap = g_Globals.MapName;

            if (_currentMap != _nextMap)
            {
                new MapChangedEventArgs(_currentMap, _nextMap);
                _currentMap = _nextMap;
                //OnMapChanged?.Invoke(_currentMap);
            }


            if (Maps.ContainsKey(_currentMap) || _isBusyLoading)
                return;

            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                _isBusyLoading = true;
                Maps.Add(_currentMap, Generators.GenerateBSP(MemoryLoader.m_dwpszProcessDirectory, _currentMap));
                _isBusyLoading = false;
            });



        }
        public bool VisibleByMask(BaseEntity _entity)
        {
            return (_entity.m_iSpottedByMask & 1 << Client.m_iLocalPlayerIndex - 1) != 0;
        }

        public bool VisibleCheck(Vector3 from, Vector3 tp)
        {
            //if ((bool)g_Globals.Config.AimbotConfig.Wallbang.Value)
            //{
            //    var _active = Client.LocalPlayer.m_hActiveWeapon;
            //    if (_active == null)
            //        return false;
            //    return m_dwMap.Wallbang(from, tp, _active.m_iItemDefinitionIndex);
            //}

            return m_dwMap.IsVisible(from, tp);


        }

        //public bool BangMyWall(SharpDX.Vector3 _from, SharpDX.Vector3 _to)
        //{
        //    var _active = Client.LocalPlayer.m_hActiveWeapon;
        //    if (_active == null)
        //        return false;
        //    return m_dwMap.Wallbang(new Vector3(_from.X, _from.Y, _from.Z), new Vector3(_to.X, _to.Y, _to.Z), _active.m_iItemDefinitionIndex);
        //}

        //public bool VisibleCheckEx(SharpDX.Vector3 _from, SharpDX.Vector3 _to)
        //{
        //    return m_dwMap.IsVisibleEx(new Vector3(_from.X,_from.Y, _from.Z), new Vector3(_to.X, _to.Y, _to.Z));
        //}
    }
}
