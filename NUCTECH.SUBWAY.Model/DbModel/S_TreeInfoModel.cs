
using System;
namespace NUCTECH.LSS.BusinessIscService.DbModel
{
	public partial class S_TreeInfoModel
    {
		public S_TreeInfoModel()
		{}
		#region Model
		private int _treeid;
		private string _treename;
		private int? _parentid;
		private string _treeimg;
		private decimal? _treesort;
		private string _treeurl;
		private DateTime? _createtime;
		private string _treenote;
		private string _treecode;
		private string _treepagecode;
		private string _treedesc;
		/// <summary>
		/// 菜单ID
		/// </summary>
		public int TreeID
		{
			set{ _treeid=value;}
			get{return _treeid;}
		}
		/// <summary>
		/// 菜单名称
		/// </summary>
		public string TreeName
		{
			set{ _treename=value;}
			get{return _treename;}
		}
		/// <summary>
		/// 所属父类
		/// </summary>
		public int? ParentID
		{
			set{ _parentid=value;}
			get{return _parentid;}
		}
		/// <summary>
		/// 菜单图标
		/// </summary>
		public string TreeImg
		{
			set{ _treeimg=value;}
			get{return _treeimg;}
		}
		/// <summary>
		/// 菜单排序
		/// </summary>
		public decimal? TreeSort
		{
			set{ _treesort=value;}
			get{return _treesort;}
		}
		/// <summary>
		/// 菜单URL
		/// </summary>
		public string TreeUrl
		{
			set{ _treeurl=value;}
			get{return _treeurl;}
		}
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime? CreateTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		/// <summary>
		/// 菜单备注
		/// </summary>
		public string TreeNote
		{
			set{ _treenote=value;}
			get{return _treenote;}
		}
		/// <summary>
		/// 菜单编号
		/// </summary>
		public string TreeCode
		{
			set{ _treecode=value;}
			get{return _treecode;}
		}
		/// <summary>
		/// 父类编号
		/// </summary>
		public string TreePageCode
		{
			set{ _treepagecode=value;}
			get{return _treepagecode;}
		}
		/// <summary>
		/// 菜单描述
		/// </summary>
		public string TreeDesc
		{
			set{ _treedesc=value;}
			get{return _treedesc;}
		}
		#endregion Model

	}
}

