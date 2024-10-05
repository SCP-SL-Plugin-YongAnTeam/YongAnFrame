using Exiled.API.Interfaces;

namespace YongAnFrame
{
    public sealed class YongAnFrameConfig : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; }
        public string BypassDoNotTrackText { get; set; } = "BDNT(Bypass Do Not Track)协议\r\n根据VSR(Verified Server Rules) 8.11，开启DNT(Do Not Track)的玩家不会进行非服务器安全性的游戏数据收集或保存。\r\n根据VSR 8.11.5，只有签署BDNT的玩家才会对DNT相关的规则不适用。\r\n根据VSR 8.11.5.3，欲签署BDNT的玩家有知晓收集或保存数据内容的权利。\r\n|||如果你看不懂BDNT协议的条例请不要签署|||\r\n1.你将会被收集SteamID用来保存等级和称号数据，这个条例收集的数据是公开展示的，任何人都可以访问！\r\n2.签署玩家依然有请求删除收集或保存数据的权利，请求之后你依然有24小时可以撤销请求(注意！删除数据是不可逆的)！";
    }
}
