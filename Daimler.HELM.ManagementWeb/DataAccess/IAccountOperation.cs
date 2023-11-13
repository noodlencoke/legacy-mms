using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Daimler.HELM.BizObjects;

namespace Daimler.HELM.ManagementWeb.DataAccess
{
    interface IAccountOperation
    {
        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        IEnumerable<InterfaceAccount> GetListAll();

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        InterfaceAccount GetByID(int id);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="InterfaceAccount"></param>
        /// <returns></returns>
        bool Add(InterfaceAccount interfaceAccount);

        /// <summary>
        /// 根据ID删除
        /// </summary>
        /// <param name="id"></param>
        bool Remove(Guid id);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="InterfaceAccount"></param>
        /// <returns></returns>
        bool Update(InterfaceAccount interfaceAccount);
    }
}
