

using System;
namespace NUCTECH.LSS.BusinessIscService.DbModel
{

	public partial class S_TreeRoleModel
	{
		public S_TreeRoleModel()
		{}
		#region Model
		private int? _roleid;
		private int? _treeid;
		/// <summary>
		/// 角色ID
		/// </summary>
		public int? RoleID
		{
			set{ _roleid=value;}
			get{return _roleid;}
		}
		/// <summary>
		/// 菜单ID
		/// </summary>
		public int? TreeID
		{
			set{ _treeid=value;}
			get{return _treeid;}
		}
		#endregion Model

	}
}

