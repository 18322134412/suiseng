using System.Linq;
using System.Collections.Generic;
using MvcApplication1.DAO;
using MvcApplication1.Model;
namespace MvcApplication1.BLL{
public partial class qqBLL{
/// <summary>
/// ���ݿ��������
/// </summary>
private qqDAO _dao = new qqDAO();
#region �����ݿ�������һ����¼ +bool; Insert(qq model)
/// <summary>
/// �����ݿ�������һ����¼
/// </summary>
/// <param name="model">Ҫ���ӵ�ʵ��</param>
/// <returns>�Ƿ�ɹ�</returns>
public bool Insert(qq model)
{
return _dao.Insert(model);
}
#endregion
#region ɾ��һ����¼ +bool Delete(string objectId)
/// <summary>
/// ɾ��һ����¼
/// </summary>
/// <param name="objectId">����</param>
/// <returns>�Ƿ�ɹ�</returns>
public bool Delete(string objectId)
{
return _dao.Delete(objectId);
}
#endregion
#region ��������ID����һ����¼ +bool Update(qq model)
/// <summary>
/// ������������һ����¼
/// </summary>
/// <param name="model">���º��ʵ��</param>
/// <returns>ִ�н����Ӱ������</returns>
public bool Update(qq model)
{
return _dao.Update(model);
}
#endregion
#region ��ѯ���� +int QueryCount(object wheres)
/// <summary>
/// ��ѯ����
/// </summary>
/// <param name="wheres">��ѯ����</param>
/// <returns>ʵ��</returns>
public int QueryCount(object wheres)
{
return _dao.QueryCount(wheres);
}
#endregion

#region ��ҳ��ѯһ������ +IEnumerable<Users> QueryList(int index, int size, object wheres=null, string orderField=null, bool isDesc = false)
///<summary>
///��ҳ��ѯһ������
///</summary>
///<param name="index">ҳ��</param>
///<param name="size">ҳ��С</param>
///<param name="wheres">����������</param>
///<param name="orderField">�����ֶ�</param>
///<param name="isDesc">�Ƿ�������</param>
///<returns>ʵ�弯��</returns>
public IEnumerable<qq> QueryList(int index, int size=10, object wheres=null, string orderField=null, bool isDesc=false)
{
return _dao.QueryList(index, size, wheres, orderField, isDesc);
}
#endregion
#region ��ҳ��ѯһ������ +IEnumerable<Users> QueryListX(int index, int size,object columns=null, object wheres=null, string orderField=null, bool isDesc = false)
///<summary>
///��ҳ��ѯһ������
///</summary>
///<param name="index">ҳ��</param>
///<param name="size">ҳ��С</param>
///<param name="wheres">����������</param>
///<param name="orderField">�����ֶ�</param>
///<param name="isDesc">�Ƿ�������</param>
///<returns>ʵ�弯��</returns>
public IEnumerable<Dictionary<string, object>> QueryListX(int index, int size=10,object columns=null, object wheres=null, string orderField=null, bool isDesc=false)
{
return _dao.QueryListX(index, size,columns, wheres, orderField, isDesc);
}
#endregion
#region ��ѯ����ģ��ʵ�� +qq QuerySingleByWheres(object wheres)
///<summary>
///��ѯ����ģ��ʵ��
///</summary>
///<param name="wheres">����������</param>
///<returns>ʵ��</returns>
public qq QuerySingleByWheres(object wheres)
{
return _dao.QuerySingleByWheres(wheres);
}
#endregion
#region ��ѯ����ģ��ʵ�� +Dictionary<string,object> QuerySingleByWheresX(object wheres,object columns)
///<summary>
///��ѯ����ģ��ʵ��
///</summary>
///<param name="wheres">����</param>
///<param name="columns">�м���</param>
///<returns>ʵ��</returns>
public Dictionary<string,object> QuerySingleByWheresX(object wheres,object columns)
{
return _dao.QuerySingleByWheresX(wheres,columns);
}
#endregion
#region ��ѯ����ģ��ʵ�� +qq QuerySingleById(string objectId)
///<summary>
///��ѯ����ģ��ʵ��
///</summary>
///<param name="objectId">key</param>
///<returns>ʵ��</returns>
public qq QuerySingleById(string objectId){
return _dao.QuerySingleById(objectId);
}
#endregion
#region ��ѯ����ģ��ʵ�� +Dictionary<string,object> QuerySingleByIdX(string objectId,object columns)///<summary>
///��ѯ����ģ��ʵ��
///</summary>
///<param name=objectId>key</param>
///<returns>ʵ��</returns>
///<returns>ʵ��</returns>
public Dictionary<string,object> QuerySingleByIdX(string objectId, object columns)
{
return _dao.QuerySingleByIdX(objectId,columns);
}
#endregion
#region ���������޸�ָ���� +bool UpdateById(string objectId,object columns)
/// <summary>
/// ������������ָ����¼
/// </summary>
/// <param name="objectId">����</param>
/// <param name="columns">�м��϶���</param>
/// <returns>�Ƿ�ɹ�</returns>
public bool UpdateById(string objectId, object columns)
{
return _dao.UpdateById(objectId, columns);
}
 #endregion
#region �����������¼�¼+bool UpdateByWheres(object wheres, object columns)
/// <summary>
/// �����������¼�¼
/// </summary>
/// <param name="wheres">��������ʵ��ʵ��</param>
/// <param name="columns">�м��϶���</param>
/// <returns>�Ƿ�ɹ�</returns>
public bool UpdateByWheres(object wheres, object columns)
{
return _dao.UpdateByWheres(wheres, columns);
}
 #endregion
}
}