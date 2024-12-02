using Exiled.API.Interfaces;
using System.ComponentModel;

namespace YongAnFrame
{
    /// <summary>
    /// 插件的翻译
    /// </summary>
    public sealed class Translation : ITranslation
    {
        /// <summary>
        /// BDNT(Bypass Do Not Track)协议文本
        /// </summary>
        [Description("BDNT(Bypass Do Not Track)协议文本")]
        public string BypassDoNotTrack { get; set; } = "BDNT(Bypass Do Not Track)协议\n根据VSR(Verified Server Rules) 8.11，所以开启DNT(Do Not Track)的玩家不会进行非服务器安全性的游戏数据收集或保存。\n根据VSR 8.11.5，所以只有签署BDNT的玩家才会对DNT相关的规则不适用。\n根据VSR 8.11.5.3，所以欲签署BDNT的玩家有知晓收集或保存数据内容的权利。\n|||如果你看不懂BDNT协议的条例请不要签署|||\n1.你将会被收集SteamID用来保存等级和称号数据，这个条例收集的数据是公开展示的，任何人都可以访问！\n2.签署玩家依然有请求删除收集或保存数据的权利，请求之后你依然有24小时可以撤销请求(注意！删除数据是不可逆的)！";
    }
}
