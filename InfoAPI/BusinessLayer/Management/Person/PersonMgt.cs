using DataAccessLayer.DBContext;
using DataModel.ViewModel;
using Domains.IRepository;
using Domains.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Management.Person
{
    public class PersonMgt : IPersonalInfo
    {
        InfonetDBContext _ctx = null;
        public PersonMgt()
        {

        }

        public async Task<object> Delete(int id, string webRootPath)
        {
            bool isSuccess = false; string message = "";
            try
            {
                using (var _ctx = new InfonetDBContext())
                {
                    using (var transaction = await _ctx.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            var objInfo = await _ctx.TblPersonalInfos.Where(x => x.UserId == id).FirstOrDefaultAsync();
                            var objSkills = await _ctx.TblPersonalSkills.Where(x => x.UserId == id).ToListAsync();
                            var objAttachments = await _ctx.TblPersonalAttachments.Where(x => x.UserId == id).ToListAsync();
                            if (objInfo != null)
                            {
                                if (objAttachments != null)
                                    _ctx.TblPersonalAttachments.RemoveRange(objAttachments);
                                if (objSkills != null)
                                    _ctx.TblPersonalSkills.RemoveRange(objSkills);
                                _ctx.TblPersonalInfos.Remove(objInfo);

                                await _ctx.SaveChangesAsync();
                                await transaction.CommitAsync();

                                foreach (var item in objAttachments)
                                {
                                    fileDelete(item.FileName, webRootPath);
                                }
                                isSuccess = true;
                                message = "Data is deleted successfully.";
                            }
                        }
                        catch (Exception)
                        {
                            await transaction.RollbackAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                message = "Error occured.Please try again.";
            }
            return new { isSuccess, message };
        }

        public async Task<object> GetById(int id)
        {
            object resData = null; _ctx = new InfonetDBContext();
            try
            {
                resData = await _ctx.TblPersonalInfos.Where(x => x.UserId == id).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
            return new { resData };
        }

        public async Task<object> GetList(vmCommon param)
        {
            List<vmPersonalInfo> list = new List<vmPersonalInfo>(); _ctx = new InfonetDBContext(); int total = 0; int pageNumber = param.pageNumber == 0 ? 1 : param.pageNumber;
            try
            {
                list = await (from info in _ctx.TblPersonalInfos
                              where
                              (info.IsActive == true)
                              &&
                              (!string.IsNullOrEmpty(param.values) ? (info.FullName.Contains(param.values) || info.UserCode.Contains(param.values) || info.City.CityName.Contains(param.values) || info.Country.CountryName.Contains(param.values)) : true)
                              &&
                              (param.id > 0 ? info.UserId == param.id : true)
                              select new vmPersonalInfo
                              {
                                  UserId = info.UserId,
                                  FullName = info.FullName,
                                  CityId = info.CityId,
                                  CityName = info.City.CityName,
                                  CountryId = info.CountryId,
                                  CountryName = info.Country.CountryName,
                                  DateOfBirth = info.DateOfBirth,
                                  skilList = (from skil in _ctx.TblPersonalSkills
                                              join sk in _ctx.TblSkills on skil.SkillId equals sk.SkillId
                                              where skil!.UserId == info!.UserId
                                              select new vmPersonalSkill
                                              {
                                                  UserId = skil!.UserId,
                                                  SkillId = skil!.SkillId,
                                                  IsActive = skil!.IsActive,
                                                  SkillName = sk.SkillName,
                                                  IsDelete = false
                                              }
                                                       ).ToList(),
                                  attachments = (from atach in _ctx.TblPersonalAttachments
                                                 where atach!.UserId == info!.UserId
                                                 select new vmAttachment
                                                 {
                                                     UserId = atach!.UserId,
                                                     FileName = atach!.FileName,
                                                     IsActive = atach!.IsActive,
                                                     FilePath = atach!.FilePath
                                                 }
                                                       ).ToList()
                              }
                        )
                        .OrderByDescending(x => x.UserId)
                        .Skip((pageNumber - 1) * param.pageSize).Take(param.pageSize).ToListAsync();


                total = await (from info in _ctx.TblPersonalInfos
                               where
                               (info.IsActive == true)
                  &&
                  (!string.IsNullOrEmpty(param.values) ? (info.FullName.Contains(param.values) || info.UserCode.Contains(param.values) || info.City.CityName.Contains(param.values) || info.Country.CountryName.Contains(param.values)) : true)
                  &&
                  (param.id > 0 ? info.UserId == param.id : true)
                               select info).CountAsync();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return new { total, list };
        }

        public async Task<object> Save(vmPersonalInfo info)
        {
            bool isSuccess = false; string message = "";
            try
            {
                using (_ctx = new InfonetDBContext())
                {
                    using (var transaction = await _ctx.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            TblPersonalInfo pInfo = new TblPersonalInfo();
                            pInfo.FullName = info.FullName;
                            pInfo.CountryId = info.CountryId;
                            pInfo.CityId = info.CityId;
                            pInfo.DateOfBirth = info.DateOfBirth;
                            pInfo.CreateDate = DateTime.Now;
                            pInfo.CreateBy = 1;
                            pInfo.IsActive = true;
                            await _ctx.TblPersonalInfos.AddAsync(pInfo);
                            await _ctx.SaveChangesAsync();

                            List<TblPersonalSkill> skilList = new List<TblPersonalSkill>();
                            foreach (var item in info.skilList)
                            {
                                var objSkill = new TblPersonalSkill
                                {
                                    UserId = pInfo.UserId,
                                    SkillId = item.SkillId,
                                    IsActive = true
                                };
                                skilList.Add(objSkill);
                            }

                            List<TblPersonalAttachment> attachments = new List<TblPersonalAttachment>();
                            foreach (var item in info.attachments)
                            {
                                var objAttachment = new TblPersonalAttachment
                                {
                                    UserId = pInfo.UserId,
                                    FileName = item.FileName,
                                    FilePath = item.FilePath,
                                    IsActive = true
                                };
                                attachments.Add(objAttachment);
                            }

                            if (skilList.Count > 0)
                            {
                                await _ctx.TblPersonalSkills.AddRangeAsync(skilList);
                            }
                            if (attachments.Count > 0)
                            {
                                await _ctx.TblPersonalAttachments.AddRangeAsync(attachments);
                            }

                            await _ctx.SaveChangesAsync();

                            foreach (var item in info.attachments)
                            {
                                string fileSavePath = info.WebRootPath + "/Files/PersonalInfo/";
                                fileSave(item, fileSavePath);
                            }

                            await transaction.CommitAsync();

                            isSuccess = true;
                            message = "Data is submitted successfully.";
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            isSuccess = false;
                            message = "Data submission is failed.";

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                isSuccess = false;
                message = "Data submission is failed.";
            }
            return new { isSuccess, message };
        }
        private void fileSave(vmAttachment attachedFile, string fileSavePath)
        {
            try
            {
                if (attachedFile != null)
                {
                    if (!Directory.Exists(fileSavePath))
                    {
                        Directory.CreateDirectory(fileSavePath);
                    }
                    if (!string.IsNullOrEmpty(attachedFile.FileData))
                    {
                        string fullPath = Path.Combine(fileSavePath, attachedFile.FileName);

                        byte[] temp_backToBytes = Convert.FromBase64String(attachedFile.FileData);

                        File.WriteAllBytes(fullPath, temp_backToBytes);
                    }
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        private void fileDelete(string fileName, string fileSavePath)
        {
            try
            {
                fileSavePath += "/Files/PersonalInfo/";
                string fullPath = Path.Combine(fileSavePath, fileName);
                if (File.Exists(fullPath))
                {

                    File.Delete(fullPath);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        public async Task<object> Update(vmPersonalInfo info)
        {
            bool isSuccess = false; string message = "";
            List<TblPersonalSkill> skilListAdd = new List<TblPersonalSkill>();
            List<TblPersonalSkill> skilListRemove = new List<TblPersonalSkill>();
            try
            {
                using (_ctx = new InfonetDBContext())
                {
                    using (var transaction = await _ctx.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            TblPersonalInfo pInfo = await _ctx.TblPersonalInfos.Where(x => x.UserId == info.UserId).FirstOrDefaultAsync();
                            if (pInfo != null)
                            {
                                pInfo.FullName = info.FullName;
                                pInfo.CountryId = info.CountryId;
                                pInfo.CityId = info.CityId;
                                pInfo.DateOfBirth = info.DateOfBirth;
                                pInfo.UpdateDate = DateTime.Now;
                                pInfo.UpdateBy = 1;
                            }
                            List<TblPersonalSkill> pSkillList = await _ctx.TblPersonalSkills.Where(x => x.UserId == info.UserId).ToListAsync();
                            //New
                            foreach (var item in info.skilList)
                            {
                                var objSkill = pSkillList.Where(s => s.SkillId == item.SkillId).FirstOrDefault();
                                if (objSkill == null)
                                {
                                    objSkill = new TblPersonalSkill
                                    {
                                        UserId = pInfo.UserId,
                                        SkillId = item.SkillId,
                                        IsActive = true
                                    };
                                    skilListAdd.Add(objSkill);
                                }
                            }
                            //Remove 
                            foreach (var item in pSkillList)
                            {
                                var sk = skilListAdd.Where(y => y.SkillId == item.SkillId).FirstOrDefault();
                                if (sk==null)
                                {
                                    var choosed = info.skilList.Where(sk => sk.SkillId == item.SkillId).FirstOrDefault();
                                    if (choosed==null)
                                    {
                                        skilListRemove.Add(item);
                                    }
                                }
                            }

                            List<TblPersonalAttachment> attachments = new List<TblPersonalAttachment>();
                            foreach (var item in info.attachments)
                            {
                                var objAttachment = new TblPersonalAttachment
                                {
                                    UserId = pInfo.UserId,
                                    FileName = item.FileName,
                                    FilePath = item.FilePath,
                                    IsActive = true
                                };
                                attachments.Add(objAttachment);
                            }

                            if (skilListAdd.Count > 0)
                            {
                                await _ctx.TblPersonalSkills.AddRangeAsync(skilListAdd);
                            }
                            if (skilListRemove.Count > 0)
                            {
                                _ctx.TblPersonalSkills.RemoveRange(skilListRemove);
                            }
                            if (attachments.Count > 0)
                            {
                                await _ctx.TblPersonalAttachments.AddRangeAsync(attachments);
                            }

                            foreach (var item in info.attachments)
                            {
                                string fileSavePath = info.WebRootPath + "/Files/PersonalInfo/";
                                fileSave(item, fileSavePath);
                            }

                            await _ctx.SaveChangesAsync();
                            await transaction.CommitAsync();

                            isSuccess = true;
                            message = "Data is updated successfully.";
                        }
                        catch (Exception ex)
                        {
                            ex.ToString();
                            await transaction.RollbackAsync();
                            isSuccess = false;
                            message = "Data updating is failed.";

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                isSuccess = false;
                message = "Data updating is failed.";
            }
            return new { isSuccess, message };
        }

        public async Task<object> GetCountryAndCity()
        {
            object listCountry = null; _ctx = new InfonetDBContext();
            try
            {

                listCountry = await (from cntry in _ctx.TblCountries

                                     select new vmCountries
                                     {
                                         CountryId = cntry.CountryId,
                                         CountryName = cntry.CountryName,
                                         cities = (from city in _ctx.TblCities

                                                   where city!.CountryId == cntry!.CountryId
                                                   select new vmCities
                                                   {
                                                       CityId = city.CityId,
                                                       CityName = city.CityName,
                                                       CountryId = city.CountryId
                                                   }
                                                              ).ToList()
                                     }
                        ).ToListAsync();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return new { listCountry };
        }
        public async Task<object> GetSkills()
        {
            object list = null; _ctx = new InfonetDBContext();
            try
            {
                //list = await _ctx.TblSkills.ToListAsync();
                list = await (from sk in _ctx.TblSkills

                              select new vmPersonalSkill
                              {
                                  SkillId = sk.SkillId,
                                  SkillName = sk.SkillName,
                                  IsChecked = false
                              }
                      ).ToListAsync();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return new { list };
        }
        public async Task<byte[]> getFile(string fileSavePath, string fileName)
        {
            byte[] file = null;
            try
            {
                await Task.Yield();
                fileName = fileSavePath + "/Files/PersonalInfo/" + fileName;
                file = File.ReadAllBytes(fileName);
            }
            catch (Exception ex)
            {
                ex.ToString(); ;
            }
            return file;
        }

    }
}
