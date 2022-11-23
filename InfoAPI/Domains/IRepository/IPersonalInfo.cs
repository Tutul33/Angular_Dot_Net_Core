using DataModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.IRepository
{
    public interface IPersonalInfo
    {
        /// <summary>
        /// This method will be used to insert list to database
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Task<object> Save(vmPersonalInfo info);
        /// <summary>
        /// This method will be used to update list to database
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Task<object> Update(vmPersonalInfo info);
        /// <summary>
        /// This method will be used to delete data from database
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Task<object> Delete(int id,string webRootPath);
        /// <summary>
        /// This method will be used to retrive data list from database
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<object> GetList(vmCommon param);
        /// <summary>
        /// This method will be used to retrieve a person information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<object> GetById(int id);
        /// <summary> 
        /// This method will be used to retrieve city and its country from DB
        /// </summary>
        /// <returns></returns>
        Task<object> GetCountryAndCity();
        /// <summary>
        /// This method will be used to retrieve skils from DB
        /// </summary>
        /// <returns></returns>
        Task<object> GetSkills();
        /// <summary>
        /// Ths
        /// </summary>
        /// <param name="fileSavePath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Task<byte[]> getFile(string fileSavePath, string fileName);
    }
}
