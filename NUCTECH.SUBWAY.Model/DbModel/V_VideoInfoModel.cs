using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUCTECH.SUBWAY.Model.DbModel
{
    public class V_VideoInfoModel
    {
        #region Model
        private int _videoid;
        private int? _devicetypeid;
        private string _videoname;
        private string _videoip;
        private int _videoport;
        private string _videolinks;
        private string _videousername;
        private string _videopassword;
        private string _videourl;
        private string _videodesc;
        private string _videojoin;
        private DateTime? _createtime;
        private DateTime? _updatetime;
        private string _videocode;
        private string _provincecode;
        private string _citycode;
        private string _countrycode;
        private int? _areaid;
        private int _securitystationid;
        private int _channel;
        /// <summary>
        /// 视频ID
        /// </summary>
        public int VideoID
        {
            set { _videoid = value; }
            get { return _videoid; }
        }
        /// <summary>
        /// 类型ID
        /// </summary>
        public int? DeviceTypeID
        {
            set { _devicetypeid = value; }
            get { return _devicetypeid; }
        }
        /// <summary>
        /// 视频名称
        /// </summary>
        public string VideoName
        {
            set { _videoname = value; }
            get { return _videoname; }
        }
        /// <summary>
        /// 视频IP
        /// </summary>
        public string VideoIP
        {
            set { _videoip = value; }
            get { return _videoip; }
        }
        public int VideoPort
        {
            set { _videoport = value; }
            get { return _videoport; }
        }
        /// <summary>
        /// 视频连接串
        /// </summary>
        public string VideoLinks
        {
            set { _videolinks = value; }
            get { return _videolinks; }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string VideoUserName
        {
            set { _videousername = value; }
            get { return _videousername; }
        }
        /// <summary>
        /// 视频密码
        /// </summary>
        public string VideoPassword
        {
            set { _videopassword = value; }
            get { return _videopassword; }
        }
        /// <summary>
        /// 视频URL
        /// </summary>
        public string VideoURL
        {
            set { _videourl = value; }
            get { return _videourl; }
        }
        /// <summary>
        /// 视频描述
        /// </summary>
        public string VideoDesc
        {
            set { _videodesc = value; }
            get { return _videodesc; }
        }
        /// <summary>
        /// 关联设备
        /// </summary>
        public string VideoJoin
        {
            set { _videojoin = value; }
            get { return _videojoin; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime
        {
            set { _updatetime = value; }
            get { return _updatetime; }
        }
        /// <summary>
        /// 视频编号
        /// </summary>
        public string VideoCode
        {
            set { _videocode = value; }
            get { return _videocode; }
        }
        /// <summary>
        /// 所在省编号
        /// </summary>
        public string ProvinceCode
        {
            set { _provincecode = value; }
            get { return _provincecode; }
        }
        /// <summary>
        /// 所在市编号
        /// </summary>
        public string CityCode
        {
            set { _citycode = value; }
            get { return _citycode; }
        }
        /// <summary>
        /// 所在县编号
        /// </summary>
        public string CountryCode
        {
            set { _countrycode = value; }
            get { return _countrycode; }
        }
        /// <summary>
        /// 所在地区id
        /// </summary>
        public int? AreaId
        {
            set { _areaid = value; }
            get { return _areaid; }
        }
        /// <summary>
        /// 监管点ID
        /// </summary>
        public int SecurityStationId
        {
            set { _securitystationid = value; }
            get { return _securitystationid; }
        }
        /// <summary>
        /// 通道号
        /// </summary>
        public int Channel
        {
            get { return _channel; }
            set { _channel = value; }
        }

        #endregion Model
    }
}
